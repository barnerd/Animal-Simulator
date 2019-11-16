using System.CodeDom;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndlessTerrain : MonoBehaviour
{
    const float viewerMoveThresholdToUpdate = 50f;
    const float sqrViewerMoveThresholdToUpdate = viewerMoveThresholdToUpdate * viewerMoveThresholdToUpdate;

    public float mapSizeMultiplier;
    public HeightMapSettings heightMapSettings;

    public LODInfo[] detailLevels;
    public Transform viewer;
    public Material mapMaterial;

    public static Vector2 viewerPosition;
    private Vector2 viewerPositionOld;
    // can be multiples of 1*2*3*4=24 or 24, 48, 72, 96, 120
    public const int mapChunkSize = 119;
    int chunkSize;
    int chunksVisibleInViewDistance;

    Dictionary<Vector2, TerrainChunk> terrainChunkDictionary = new Dictionary<Vector2, TerrainChunk>();
    List<TerrainChunk> visibleTerrainChunks = new List<TerrainChunk>();

    // Start is called before the first frame update
    void Start()
    {
        SetMaterialParameters();

        float maxViewDistance = detailLevels[detailLevels.Length - 1].visibleDistanceThreshold;
        chunkSize = mapChunkSize - 1;
        chunksVisibleInViewDistance = Mathf.RoundToInt(maxViewDistance / chunkSize);

        UpdateVisibleChunks();
    }

    // Update is called once per frame
    void Update()
    {
        // TODO: move the player is scaled, causing the wrong chucks to be displayed
        viewerPosition = (new Vector2(viewer.position.x, viewer.position.z));// / mapSizeMultiplier;

        if ((viewerPositionOld - viewerPosition).sqrMagnitude > sqrViewerMoveThresholdToUpdate)
        {
            viewerPositionOld = viewerPosition;
            UpdateVisibleChunks();
        }
    }

    void SetMaterialParameters()
    {
        mapMaterial.SetFloat("_MaxHeight", mapSizeMultiplier * heightMapSettings.heightMultiplier);
        mapMaterial.SetFloat("_HeightLines", (heightMapSettings.hasElevationLines) ? 1 : 0);
        mapMaterial.SetFloat("_ElevationPerMajorLine", heightMapSettings.elevationPerMajorLine);
        mapMaterial.SetFloat("_WidthOfMajorLine", heightMapSettings.widthOfMajorLine);
        mapMaterial.SetFloat("_ElevationPerMinorLine", heightMapSettings.elevationPerMinorLine);
        mapMaterial.SetFloat("_WidthOfMinorLine", heightMapSettings.widthOfMinorLine);
    }

    void UpdateVisibleChunks()
    {
        HashSet<Vector2> alreadyUpdatedChunkCoords = new HashSet<Vector2>();

        for (int i = visibleTerrainChunks.Count - 1; i >= 0; i--)
        {
            alreadyUpdatedChunkCoords.Add(visibleTerrainChunks[i].coord);
            visibleTerrainChunks[i].UpdateTerrainChunk();
        }

        int currentChunkCoordX = Mathf.RoundToInt(viewerPosition.x / (chunkSize * mapSizeMultiplier));
        int currentChunkCoordY = Mathf.RoundToInt(viewerPosition.y / (chunkSize * mapSizeMultiplier));

        Debug.Log("current: " + new Vector2(currentChunkCoordX, currentChunkCoordY) + " viewable: " + new Vector2(currentChunkCoordX- chunksVisibleInViewDistance, currentChunkCoordY- chunksVisibleInViewDistance) + " to " + new Vector2(currentChunkCoordX + chunksVisibleInViewDistance, currentChunkCoordY + chunksVisibleInViewDistance));
        for (int yOffset = -chunksVisibleInViewDistance; yOffset <= chunksVisibleInViewDistance; yOffset++)
        {
            for (int xOffset = -chunksVisibleInViewDistance; xOffset <= chunksVisibleInViewDistance; xOffset++)
            {
                Vector2 viewedchunkCoord = new Vector2(currentChunkCoordX + xOffset, currentChunkCoordY + yOffset);

                if (!alreadyUpdatedChunkCoords.Contains(viewedchunkCoord))
                {
                    if (terrainChunkDictionary.ContainsKey(viewedchunkCoord))
                    {
                        terrainChunkDictionary[viewedchunkCoord].UpdateTerrainChunk();
                    }
                    else
                    {
                        TerrainChunk newChunk = new TerrainChunk(viewedchunkCoord, chunkSize, mapSizeMultiplier, heightMapSettings, detailLevels, transform, viewer, mapMaterial);
                        terrainChunkDictionary.Add(viewedchunkCoord, newChunk);
                        newChunk.onVisibilityChanged += OnChunkVisibilityChanged;
                        newChunk.Load();
                    }
                }
            }
        }
    }

    void OnChunkVisibilityChanged(TerrainChunk chunk, bool isVisible)
    {
        if (isVisible)
        {
            visibleTerrainChunks.Add(chunk);
        }
        else
        {
            visibleTerrainChunks.Remove(chunk);
        }
    }
}

[System.Serializable]
public struct LODInfo
{
    [Range(0, 4)]
    public int lod;
    public float visibleDistanceThreshold;
}
