using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapDisplay : MonoBehaviour
{
    public enum DrawMode { NoiseMap, Mesh };

    [Header("Display Settings")]
    public DrawMode drawMode;
    public bool autoUpdate;
    [Range(0, 4)]
    public int levelOfDetail;
    public float mapSizeMultiplier;
    public HeightMapSettings heightMapSettings;
    // can be multiples of 1*2*3*4=24 or 24, 48, 72, 96, 120
    public int mapSize = 119;

    public Renderer textureRenderer;
    public MeshFilter meshFilter;
    public MeshRenderer meshRenderer;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void DrawMapInEditor()
    {
        MapData mapData = MapGenerator.GenerateMapData(mapSize, Vector2.zero, heightMapSettings);

        switch (drawMode)
        {
            case DrawMode.NoiseMap:
                DrawTexture(TextureFromHeightMap(mapData.heightMap));
                break;
            case DrawMode.Mesh:
                DrawMesh(MeshGenerator.GenerateTerrainMesh(mapData.heightMap, levelOfDetail), mapSizeMultiplier);
                break;
            default:
                break;
        }
    }

    public void DrawTexture(Texture2D texture)
    {
        textureRenderer.sharedMaterial.mainTexture = texture;
        textureRenderer.transform.localScale = new Vector3(texture.width, 1, texture.height);
    }

    public void DrawMesh(MeshData meshData, float _scale)
    {
        meshFilter.sharedMesh = meshData.CreateMesh();

        meshFilter.transform.localScale = Vector3.one * _scale;
    }

    public static Texture2D TextureFromColorMap(Color[] colorMap, int width, int height)
    {
        Texture2D texture = new Texture2D(width, height);

        texture.filterMode = FilterMode.Point;
        texture.wrapMode = TextureWrapMode.Clamp;
        texture.SetPixels(colorMap);
        texture.Apply();

        return texture;
    }

    public static Texture2D TextureFromHeightMap(float[,] heightMap)
    {
        int width = heightMap.GetLength(0);
        int height = heightMap.GetLength(1);

        Color[] colorMap = new Color[width * height];
        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                colorMap[y * width + x] = Color.Lerp(Color.black, Color.white, heightMap[x, y]);
            }
        }

        return TextureFromColorMap(colorMap, width, height);
    }

}
