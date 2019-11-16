using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapDisplay : MonoBehaviour
{
    public enum DrawMode { NoiseMap, ColorNoise, Mesh };

    [Header("Display Settings")]
    public DrawMode drawMode;
    public bool autoUpdate;
    public bool heightLines;
    public bool erosion;
    [Range(0, 4)]
    public int levelOfDetail;
    // can be multiples of 1*2*3*4=24 or 24, 48, 72, 96, 120
    public int mapSize;

    public Renderer textureRenderer;
    public MeshFilter meshFilter;
    public MeshRenderer meshRenderer;

    public EndlessTerrain endlessTerrain;

    // Start is called before the first frame update
    void Start()
    {
        textureRenderer.gameObject.SetActive(false);
        meshRenderer.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void DrawMapInEditor()
    {
        endlessTerrain.heightMapSettings.erosionSettings.erode = erosion;
        endlessTerrain.heightMapSettings.hasElevationLines = heightLines;

        MapData mapData = MapGenerator.GenerateMapData(mapSize + 2, Vector2.zero, endlessTerrain.heightMapSettings);

        switch (drawMode)
        {
            case DrawMode.NoiseMap:
                DrawTexture(GenerateTexture(ColorsFromHeightMap(mapData.heightMap, endlessTerrain.heightMapSettings.heightMultiplier, drawMode, heightLines), mapData.heightMap.GetLength(0)), endlessTerrain.mapSizeMultiplier * 15f);
                textureRenderer.gameObject.SetActive(true);
                meshRenderer.gameObject.SetActive(false);
                break;
            case DrawMode.ColorNoise:
                DrawTexture(GenerateTexture(ColorsFromHeightMap(mapData.heightMap, endlessTerrain.heightMapSettings.heightMultiplier, drawMode, heightLines), mapData.heightMap.GetLength(0)), endlessTerrain.mapSizeMultiplier * 15f);
                textureRenderer.gameObject.SetActive(true);
                meshRenderer.gameObject.SetActive(false);
                break;
            case DrawMode.Mesh:
                Color[] colors = ColorsFromHeightMap(mapData.heightMap, endlessTerrain.heightMapSettings.heightMultiplier, drawMode, heightLines);
                DrawMesh(MeshGenerator.GenerateTerrainMesh(mapData.heightMap, levelOfDetail), colors);
                textureRenderer.gameObject.SetActive(false);
                meshRenderer.gameObject.SetActive(true);
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

    public Texture2D GenerateTexture(Color[] colorMap, int size)
    {
        Texture2D texture = new Texture2D(size, size);

        texture.filterMode = FilterMode.Point;
        texture.wrapMode = TextureWrapMode.Clamp;
        texture.SetPixels(colorMap);
        texture.Apply();

        return texture;
    }

    public Color[] ColorsFromHeightMap(float[,] heightMap, float heightScale, DrawMode _mode, bool heightLines)
    {
        int size = heightMap.GetLength(0) - ((_mode == DrawMode.Mesh) ? 2 : 0);

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
                if (_mode == DrawMode.ColorNoise || _mode == DrawMode.Mesh)
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
}
