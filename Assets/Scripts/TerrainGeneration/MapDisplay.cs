using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapDisplay : MonoBehaviour
{
    public enum DrawMode { NoiseMap, ColorNoise, Plates, Mesh };
    public enum PlateDrawMode { Plates, OceanVsCont };

    [Header("Display Settings")]
    public DrawMode drawMode;
    public PlateDrawMode plateDrawMode;
    public bool autoUpdate;
    public bool heightLines;
    public bool erosion;
    [Range(0, 4)]
    public int levelOfDetail;
    // can be multiples of 1*2*3*4=24 or 24, 48, 72, 96, 120
    public int chunkSize;
    public int planetSize;
    public int numPlates;
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
            Debug.Log("Moving Plates");
            nextTimeForUpdate += updateInterval;
            Planet.MovePlates(planetData);
            DrawMap();
        }
    }

    public void DrawMapInEditor()
    {
        endlessTerrain.heightMapSettings.erosionSettings.erode = erosion;
        endlessTerrain.heightMapSettings.hasElevationLines = heightLines;

        mapData = MapGenerator.GenerateMapData(chunkSize + 2, Vector2.zero, endlessTerrain.heightMapSettings);
        planetData = Planet.GenerateTectonicPlates(planetSize, numPlates);
        //Planet.MovePlates(planetData);

        for (int i = 0; i < numPlates; i++)
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
            case DrawMode.Mesh:
                Color[] colors = ColorsFromHeightMap(mapData.heightMap, endlessTerrain.heightMapSettings.heightMultiplier, drawMode, heightLines);
                DrawMesh(MeshGenerator.GenerateTerrainMesh(mapData.heightMap, levelOfDetail), colors);
                textureRenderer.gameObject.SetActive(false);
                textureRenderer2.gameObject.SetActive(false);
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
}
