using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainChunk
{
    public event System.Action<TerrainChunk, bool> onVisibilityChanged;
    public Vector2 coord;
    int size;

    HeightMapSettings settings;

    GameObject meshObject;
    Vector2 position;
    Bounds bounds;

    MeshRenderer meshRenderer;
    MeshFilter meshFilter;
    MeshCollider meshCollider;

    LODInfo[] detailLevels;
    float maxViewDistance;
    LODMesh[] lodMeshes;

    MapData mapData;
    bool mapDataReceived;

    int previousLODIndex = -1;

    Transform viewer;

    public TerrainChunk(Vector2 _coord, int _size, float scale, HeightMapSettings _settings, LODInfo[] _detailLevels, Transform _parent, Transform _viewer, Material _material)
    {
        coord = _coord;
        size = _size;

        position = coord * size;
        bounds = new Bounds(position, Vector2.one * size);
        Vector3 positionV3 = new Vector3(position.x, 0, position.y);

        detailLevels = _detailLevels;

        meshObject = new GameObject("Terrain Chunk");
        meshRenderer = meshObject.AddComponent<MeshRenderer>();
        meshRenderer.material = _material;
        meshFilter = meshObject.AddComponent<MeshFilter>();
        meshCollider = meshObject.AddComponent<MeshCollider>();

        meshObject.transform.position = positionV3 * scale;
        meshObject.transform.parent = _parent;
        meshObject.transform.localScale = Vector3.one * scale;

        settings = _settings;

        viewer = _viewer;

        SetVisible(false);

        lodMeshes = new LODMesh[detailLevels.Length];
        for (int i = 0; i < lodMeshes.Length; i++)
        {
            lodMeshes[i] = new LODMesh(detailLevels[i].lod, UpdateTerrainChunk);
        }

        maxViewDistance = detailLevels[detailLevels.Length - 1].visibleDistanceThreshold;

    }

    public void Load()
    {
        ThreadedDataRequester.RequestData(() => MapGenerator.GenerateMapData(size, position, settings), OnDataReceived);
    }

    void OnDataReceived(object _mapData)
    {
        mapData = (MapData)_mapData;
        mapDataReceived = true;

        UpdateTerrainChunk();
    }

    /*void OnMeshDataReceived(MeshData _meshData)
    {
        meshFilter.mesh = _meshData.CreateMesh();
    }*/

    Vector2 viewerPosition
    {
        get { return new Vector2(viewer.position.x, viewer.position.z); }
    }

    public void UpdateTerrainChunk()
    {
        if (mapDataReceived)
        {
            float viewerDistanceFromNearestEdge = Mathf.Sqrt(bounds.SqrDistance(viewerPosition));
            bool wasVisible = IsVisible();
            SetVisible(viewerDistanceFromNearestEdge <= maxViewDistance);

            if (IsVisible())
            {
                int lodIndex = 0;
                for (int i = 0; i < detailLevels.Length - 1; i++)
                {
                    if (viewerDistanceFromNearestEdge > detailLevels[i].visibleDistanceThreshold)
                    {
                        lodIndex = i + 1;
                    }
                    else
                    {
                        break;
                    }
                }

                if (lodIndex != previousLODIndex)
                {
                    LODMesh lodMesh = lodMeshes[lodIndex];
                    if (lodMesh.hasMesh)
                    {
                        previousLODIndex = lodIndex;
                        meshFilter.mesh = lodMesh.mesh;
                    }
                    else if (!lodMesh.hasRequestedMesh)
                    {
                        lodMesh.RequestMesh(mapData);
                    }
                }

                if (lodIndex == 0)
                {
                    if (lodMeshes[0].hasMesh)
                    {
                        meshCollider.sharedMesh = lodMeshes[0].mesh;
                    }
                    else
                    {
                        lodMeshes[0].RequestMesh(mapData);
                    }
                }

                if (wasVisible != IsVisible())
                {
                    if(onVisibilityChanged != null)
                    {
                        onVisibilityChanged(this, IsVisible());
                    }
                }
            }
        }
    }

    public void SetVisible(bool _visible)
    {
        meshObject.SetActive(_visible);
    }

    public bool IsVisible()
    {
        return meshObject.activeSelf;
    }
}

class LODMesh
{
    public Mesh mesh;
    public bool hasRequestedMesh;
    public bool hasMesh;
    int lod;
    System.Action updateCallback;

    public LODMesh(int _lod, System.Action _updateCallback)
    {
        lod = _lod;
        updateCallback = _updateCallback;
    }

    void OnDataReceived(object _meshData)
    {
        mesh = ((MeshData)_meshData).CreateMesh();
        hasMesh = true;

        updateCallback();
    }

    public void RequestMesh(MapData _mapData)
    {
        hasRequestedMesh = true;

        ThreadedDataRequester.RequestData(() => MeshGenerator.GenerateTerrainMesh(_mapData.heightMap, lod), OnDataReceived);
    }
}
