﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VoronoiLib;
using VoronoiLib.Structures;

public class MapDisplay : MonoBehaviour
{
    public enum DrawMode { NoiseMap, ColorNoise, Plates, NoiseMesh, VoronoiMesh };
    public enum PlateDrawMode { Plates, OceanVsCont };
    public enum VoronoiMeshMode { Delaunay, Voronoi, Both, DelaunayTrianglation, VoronoiTrianglation, Sphere };

    [Header("Display Settings")]
    public DrawMode drawMode;
    public PlateDrawMode plateDrawMode;
    public VoronoiMeshMode voronoiMeshMode;
    public bool autoUpdate;
    public bool heightLines;
    public bool erosion;
    [Range(0, 4)]
    public int levelOfDetail;
    // can be multiples of 1*2*3*4=24 or 24, 48, 72, 96, 120
    public int chunkSize;
    public int planetSize;
    public int numPlates;
    public int numCenters;
    [Range(0, 5)]
    public int relaxation;
    public bool randomPoints;
    public Color[] plateColors;

    public Renderer textureRenderer;
    public Renderer textureRenderer2;
    public MeshFilter meshFilter;
    public MeshRenderer meshRenderer;

    public EndlessTerrain endlessTerrain;
    public MapData mapData;
    public PlanetData planetData;

    float updateInterval = 1f;
    float nextTimeForUpdate;

    public VoronoiGraph worldGraph;

    // Start is called before the first frame update
    void Start()
    {
        textureRenderer.gameObject.SetActive(true);
        textureRenderer2.gameObject.SetActive(true);
        meshRenderer.gameObject.SetActive(false);

        nextTimeForUpdate = Time.time + updateInterval;
        DrawMapInEditor();
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.time > nextTimeForUpdate)
        {
            nextTimeForUpdate += updateInterval;
            //Planet.MovePlates(planetData);
            DrawMap();
        }
    }

    public void DrawMapInEditor()
    {
        System.Random prng = new System.Random(1);
        //List<VSphericalPoint> points = new List<VSphericalPoint>();
        List<VPoint> points = new List<VPoint>();

        for (int i = 0; i < numCenters; i++)
        {
            //points.Add(new VSphericalPoint(Random.Range(-179.99f, 180f), Random.Range(-89.99f, 89.99f)));
            points.Add(new VPoint(prng.NextDouble() * 800f - 400f, prng.NextDouble() * 800f - 400f));
        }
        if (!randomPoints)
        {
            points.Clear();
            /*points.Add(new VSphericalPoint(0, 90));
            points.Add(new VSphericalPoint(5, 0));
            points.Add(new VSphericalPoint(0, -60));*/
            /*points.Add(new VSphericalPoint(30, 15));
            points.Add(new VSphericalPoint(0, 0));
            points.Add(new VSphericalPoint(-45, 15));*/
        }
        worldGraph = new VoronoiGraph(points);

        //Debug.Log(worldGraph.FindClosestSites(new Vector2(Random.Range(0, planetSize), Random.Range(0, planetSize)), 32f).Count);

        endlessTerrain.heightMapSettings.erosionSettings.erode = erosion;
        endlessTerrain.heightMapSettings.hasElevationLines = heightLines;

        //mapData = MapGenerator.GenerateMapData(chunkSize + 2, Vector2.zero, endlessTerrain.heightMapSettings);
        //planetData = Planet.GenerateTectonicPlates(planetSize, numPlates);
        //Planet.MovePlates(planetData);

        //for (int i = 0; i < numPlates; i++)
        {
            //Debug.Log(planetData.plates[i].direction);
            //Debug.Log(planetData.plates[i].speed);
        }

        DrawMap();
    }

    void DrawMap()
    {
        switch (drawMode)
        {
            case DrawMode.NoiseMap:
                DrawTexture(GenerateTexture(ColorsFromHeightMap(mapData.heightMap, endlessTerrain.heightMapSettings.heightMultiplier, drawMode, heightLines), mapData.heightMap.GetLength(0)), endlessTerrain.mapSizeMultiplier * 15f);
                textureRenderer.gameObject.SetActive(true);
                textureRenderer2.gameObject.SetActive(false);
                meshRenderer.gameObject.SetActive(false);
                break;
            case DrawMode.ColorNoise:
                DrawTexture(GenerateTexture(ColorsFromHeightMap(mapData.heightMap, endlessTerrain.heightMapSettings.heightMultiplier, drawMode, heightLines), mapData.heightMap.GetLength(0)), endlessTerrain.mapSizeMultiplier * 15f);
                textureRenderer.gameObject.SetActive(true);
                textureRenderer2.gameObject.SetActive(false);
                meshRenderer.gameObject.SetActive(false);
                break;
            case DrawMode.Plates:
                DrawPlateTexture(GeneratePlateTexture(ColorsFromPlateMap(planetData.plateMap, plateColors, planetData.plates, plateDrawMode), planetData.plateMap.GetLength(1)), endlessTerrain.mapSizeMultiplier * 15f);
                textureRenderer.gameObject.SetActive(true);
                textureRenderer2.gameObject.SetActive(true);
                meshRenderer.gameObject.SetActive(false);
                break;
            case DrawMode.NoiseMesh:
                Color[] colors = ColorsFromHeightMap(mapData.heightMap, endlessTerrain.heightMapSettings.heightMultiplier, drawMode, heightLines);
                DrawMesh(MeshGenerator.GenerateTerrainMesh(mapData.heightMap, levelOfDetail), colors);
                textureRenderer.gameObject.SetActive(false);
                textureRenderer2.gameObject.SetActive(false);
                meshRenderer.gameObject.SetActive(true);
                break;
            case DrawMode.VoronoiMesh:
                //DrawMesh(worldGraph.CreateMesh(voronoiMeshMode));
                textureRenderer.gameObject.SetActive(false);
                textureRenderer2.gameObject.SetActive(false);
                meshRenderer.gameObject.SetActive(false);
                break;
            default:
                break;
        }
    }

    public void DrawTexture(Texture2D texture, float _scale)
    {
        textureRenderer.sharedMaterial.mainTexture = texture;
        textureRenderer.transform.localScale = Vector3.one * _scale;
    }

    public void DrawPlateTexture(Texture2D[] texture, float _scale)
    {
        textureRenderer.sharedMaterial.mainTexture = texture[0];
        textureRenderer.transform.localScale = Vector3.one * _scale;
        textureRenderer2.sharedMaterial.mainTexture = texture[1];
        textureRenderer2.transform.localScale = Vector3.one * _scale;
    }

    public void DrawMesh(MeshData meshData, Color[] colorMap)
    {
        meshFilter.sharedMesh = meshData.CreateMesh();
        meshFilter.transform.localScale = Vector3.one * endlessTerrain.mapSizeMultiplier;
        meshFilter.sharedMesh.colors = colorMap;
        meshRenderer.sharedMaterial.SetFloat("_MaxHeight", endlessTerrain.mapSizeMultiplier * endlessTerrain.heightMapSettings.heightMultiplier);
        meshRenderer.sharedMaterial.SetFloat("_HeightLines", (endlessTerrain.heightMapSettings.hasElevationLines) ? 1 : 0);
        meshRenderer.sharedMaterial.SetFloat("_ElevationPerMajorLine", endlessTerrain.heightMapSettings.elevationPerMajorLine);
        meshRenderer.sharedMaterial.SetFloat("_WidthOfMajorLine", endlessTerrain.heightMapSettings.widthOfMajorLine);
        meshRenderer.sharedMaterial.SetFloat("_ElevationPerMinorLine", endlessTerrain.heightMapSettings.elevationPerMinorLine);
        meshRenderer.sharedMaterial.SetFloat("_WidthOfMinorLine", endlessTerrain.heightMapSettings.widthOfMinorLine);
    }

    public void DrawMesh(Mesh _mesh)
    {
        meshFilter.sharedMesh = _mesh;
        meshFilter.transform.localScale = Vector3.one * endlessTerrain.mapSizeMultiplier;
        meshRenderer.sharedMaterial.SetFloat("_MaxHeight", endlessTerrain.mapSizeMultiplier * endlessTerrain.heightMapSettings.heightMultiplier);
        meshRenderer.sharedMaterial.SetFloat("_HeightLines", (endlessTerrain.heightMapSettings.hasElevationLines) ? 1 : 0);
        meshRenderer.sharedMaterial.SetFloat("_ElevationPerMajorLine", endlessTerrain.heightMapSettings.elevationPerMajorLine);
        meshRenderer.sharedMaterial.SetFloat("_WidthOfMajorLine", endlessTerrain.heightMapSettings.widthOfMajorLine);
        meshRenderer.sharedMaterial.SetFloat("_ElevationPerMinorLine", endlessTerrain.heightMapSettings.elevationPerMinorLine);
        meshRenderer.sharedMaterial.SetFloat("_WidthOfMinorLine", endlessTerrain.heightMapSettings.widthOfMinorLine);
    }

    public Texture2D GenerateTexture(Color[] colorMap, int size)
    {
        Texture2D texture = new Texture2D(size, size);

        texture.filterMode = FilterMode.Point;
        texture.wrapMode = TextureWrapMode.Clamp;
        texture.SetPixels(colorMap);
        texture.Apply();

        return texture;
    }

    public Texture2D[] GeneratePlateTexture(Color[][] colorMap, int size)
    {
        Texture2D[] textures = new Texture2D[colorMap.Length];

        for (int i = 0; i < colorMap.Length; i++)
        {
            textures[i] = new Texture2D(size, size);

            textures[i].filterMode = FilterMode.Point;
            textures[i].wrapMode = TextureWrapMode.Clamp;
            textures[i].SetPixels(colorMap[i]);
            textures[i].Apply();
        }

        return textures;
    }

    public Color[] ColorsFromHeightMap(float[,] heightMap, float heightScale, DrawMode _mode, bool heightLines)
    {
        int size = heightMap.GetLength(0) - ((_mode == DrawMode.NoiseMesh) ? 2 : 0);

        Color[] colorMap = new Color[size * size];
        int index = 0;
        int i = 0;

        for (int y = 0; y < size; y++)
        {
            for (int x = 0; x < size; x++)
            {
                float heightPercent = heightMap[x, y] / heightScale;
                Color bottomColor = Color.black;
                Color topColor = Color.white;
                if (_mode == DrawMode.ColorNoise || _mode == DrawMode.NoiseMesh)
                {
                    if (heightPercent < .2)
                    {
                        topColor = Color.blue;
                        heightPercent = (heightPercent - 0f) / (.2f - 0f);
                    }
                    else if (heightPercent < .4)
                    {
                        bottomColor = Color.blue;
                        topColor = Color.yellow;
                        heightPercent = (heightPercent - .2f) / (.4f - .2f);
                    }
                    else if (heightPercent < .6)
                    {
                        bottomColor = Color.yellow;
                        topColor = Color.green;
                        heightPercent = (heightPercent - .4f) / (.6f - .4f);
                    }
                    else if (heightPercent < .8)
                    {
                        bottomColor = Color.green;
                        topColor = Color.red;
                        heightPercent = (heightPercent - .6f) / (.8f - .6f);
                    }
                    else
                    {
                        bottomColor = Color.red;
                        heightPercent = (heightPercent - .8f) / (1f - .8f);
                    }
                    heightPercent = Mathf.Clamp01(heightPercent);
                }
                if (heightLines)
                {
                    // if using a plane, only show major lines
                    if ((heightMap[x, y] * endlessTerrain.mapSizeMultiplier) % endlessTerrain.heightMapSettings.elevationPerMajorLine < endlessTerrain.heightMapSettings.widthOfMajorLine)
                    {
                        bottomColor = topColor = Color.black;
                    }
                }
                i++;
                index = y * size + x;
                colorMap[y * size + x] = Color.Lerp(bottomColor, topColor, heightPercent);
            }
        }
        return colorMap;
    }

    public Color[][] ColorsFromPlateMap(int[,] plateMap, Color[] colors, TectonicPlate[] plates, PlateDrawMode drawMode)
    {
        int size = plateMap.GetLength(1);

        Color[][] colorMap = new Color[2][];
        colorMap[0] = new Color[size * size];
        colorMap[1] = new Color[size * size];

        for (int y = 0; y < size; y++)
        {
            for (int x = 0; x < 2 * size; x++)
            {
                Color c;
                if (plateMap[x, y] == 0)
                {
                    c = Color.white;
                }
                else
                {
                    c = colors[plateMap[x, y] - 1];
                    if (drawMode == PlateDrawMode.OceanVsCont)
                    {
                        if (plates[plateMap[x, y] - 1].oceanic)
                        {
                            // shade of blue
                            c = new Color(0, 0, .9f, 1f) * ((float)plateMap[x, y] / numPlates * .3f + .6f);
                        }
                        else
                        {
                            // shade of reddish brown
                            c = new Color(0.7f, .25f, .25f, 1f) * ((float)plateMap[x, y] / numPlates * .3f + .6f);
                        }
                    }
                }

                if (x < size)
                {
                    colorMap[0][y * size + x] = c;
                }
                else
                {
                    colorMap[1][y * size + x - size] = c;
                }
            }
        }
        return colorMap;
    }

    void OnDrawGizmos()
    {
        if (drawMode == DrawMode.VoronoiMesh && worldGraph != null)
        {
            var sites = worldGraph.Sites();
            var edges = worldGraph.Edges();

            if (sites != null)
            {
                if (voronoiMeshMode == VoronoiMeshMode.Delaunay || voronoiMeshMode == VoronoiMeshMode.Both || voronoiMeshMode == VoronoiMeshMode.Voronoi)
                {
                    foreach (var site in sites.Values)
                    {
                        Gizmos.color = Color.red;
                        Gizmos.DrawSphere(new Vector3((float)site.X, 0, (float)site.Y) * endlessTerrain.mapSizeMultiplier, 4f * endlessTerrain.mapSizeMultiplier);

                        if (voronoiMeshMode != VoronoiMeshMode.Voronoi)
                        {
                            Gizmos.color = Color.black;
                            foreach (var neighbor in site.Neighbors)
                            {
                                Gizmos.DrawLine(new Vector3((float)site.X, 0, (float)site.Y) * endlessTerrain.mapSizeMultiplier, new Vector3((float)neighbor.X, 0, (float)neighbor.Y) * endlessTerrain.mapSizeMultiplier);
                            }
                        }
                    }
                }

                foreach (var edge in edges)
                {
                    var start = new Vector3((float)edge.Start.X, 0, (float)edge.Start.Y) * endlessTerrain.mapSizeMultiplier;
                    var end = new Vector3((float)edge.End.X, 0, (float)edge.End.Y) * endlessTerrain.mapSizeMultiplier;

                    if (voronoiMeshMode == VoronoiMeshMode.Voronoi || voronoiMeshMode == VoronoiMeshMode.Both)
                    {
                        Gizmos.color = Color.white;
                        Gizmos.DrawLine(start, end);
                        Gizmos.color = Color.blue;
                        Gizmos.DrawSphere(start, 3f * endlessTerrain.mapSizeMultiplier);
                        Gizmos.DrawSphere(end, 3f * endlessTerrain.mapSizeMultiplier);
                    }
                    if (voronoiMeshMode == VoronoiMeshMode.VoronoiTrianglation)
                    {
                        var leftCenter = new Vector3((float)edge.Left.X, 0, (float)edge.Left.Y) * endlessTerrain.mapSizeMultiplier;
                        var rightCenter = new Vector3((float)edge.Right.X, 0, (float)edge.Right.Y) * endlessTerrain.mapSizeMultiplier;

                        Gizmos.color = Color.white;
                        Gizmos.DrawLine(leftCenter, start);
                        Gizmos.DrawLine(start, end);
                        Gizmos.DrawLine(end, leftCenter);

                        Gizmos.DrawLine(rightCenter, start);
                        Gizmos.DrawLine(start, end);
                        Gizmos.DrawLine(end, rightCenter);
                    }
                }

                if (voronoiMeshMode == VoronoiMeshMode.Sphere)
                {
                    Vector3 start, end;
                    float radius = 400f;

                    // center of sphere
                    Gizmos.color = Color.yellow;
                    Gizmos.DrawSphere(Vector3.zero, 5f * endlessTerrain.mapSizeMultiplier);

                    foreach (var site in sites.Values)
                    {
                        Gizmos.color = Color.red;
                        start = (new VSphericalPoint(site.Site)).euclidean;
                        start *= endlessTerrain.mapSizeMultiplier * radius;
                        Gizmos.DrawSphere(start, 4f * endlessTerrain.mapSizeMultiplier);

                        Gizmos.color = Color.black;
                        foreach (var neighbor in site.Neighbors)
                        {
                            end = (new VSphericalPoint(neighbor.Site)).euclidean;
                            end *= endlessTerrain.mapSizeMultiplier * radius;
                            Gizmos.DrawLine(start, end);
                        }
                    }

                    foreach (var edge in edges)
                    {
                        start = (new VSphericalPoint(edge.Start)).euclidean;
                        start *= endlessTerrain.mapSizeMultiplier * radius;
                        end = (new VSphericalPoint(edge.End)).euclidean;
                        end *= endlessTerrain.mapSizeMultiplier * radius;

                        Gizmos.color = Color.white;
                        Gizmos.DrawLine(start, end);
                        Gizmos.color = Color.blue;
                        Gizmos.DrawSphere(start, 3f * endlessTerrain.mapSizeMultiplier);
                        Gizmos.DrawSphere(end, 3f * endlessTerrain.mapSizeMultiplier);
                    }
                }
            }
        }
    }
}
