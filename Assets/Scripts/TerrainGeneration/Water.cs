using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Water : MonoBehaviour
{
    // grid of cells
    // each cell has a volume of water
    // each cell has a max capacity of water
    // each cell has a water direction
    // each cell has a water speed

    private int mapSize;
    public MapData mapData;
    public EndlessTerrain endlessTerrain;

    WaterData[,] waterDatas;
    public Color[] waterTexture;

    float updateInterval = 5f;
    float nextTimeForUpdate;

    MeshFilter meshFilter;

    // Start is called before the first frame update
    void Start()
    {
        meshFilter = GetComponent<MeshFilter>();

        nextTimeForUpdate = Time.time + updateInterval;
    }

    // Update is called once per frame
    void Update()
    {
        // cells gain water from weather (or spells?)
        // some water evaporates
        // some water seeps into the soil
        // the rest flows to the lowest neighbor
        // if enough water on a cell, display it x height above the ground, where x depends on water volume

        if (Time.time > nextTimeForUpdate)
        {
            nextTimeForUpdate += updateInterval;
            ProcessWater();
        }
    }

    public void setMapData(MapData _map)
    {
        mapData = _map;
        mapSize = mapData.heightMap.GetLength(0);
        waterDatas = new WaterData[mapSize, mapSize];
        InitializeWaterData();
        waterTexture = GenerateTexture(0);
    }

    void InitializeWaterData()
    {
        for (int y = 0; y < mapSize; y++)
        {
            for (int x = 0; x < mapSize; x++)
            {
                waterDatas[x, y] = new WaterData(1 - mapData.heightMap[x, y] / mapData.heightMultiplier, Vector2.zero);
            }
        }
    }

    public Color[] GenerateTexture(int lod)
    {
        Debug.Log("Converting water to colors");

        int size = EndlessTerrain.mapChunkSize;
        Color[] colorMap = new Color[size * size];
        int incrementStep = (lod == 0) ? 1 : lod * 2;

        for (int y = 0; y < size; y += incrementStep)
        {
            for (int x = 0; x < size; x += incrementStep)
            {
                colorMap[y * size + x] = Color.blue * waterDatas[x, y].volume;
            }
        }

        return colorMap;
    }

    public void ProcessWater()
    {
        Debug.Log("Processing water");
        int size = EndlessTerrain.mapChunkSize;
        // create empty array
        // this won't be empty of it's raining (or raining will have it's own function)
        float[,] newWater = new float[size, size];
        float waterVol;

        // move water one cycle
        for (int y = 0; y < size; y++)
        {
            for (int x = 0; x < size; x++)
            {
                waterVol = waterDatas[x, y].volume;

                // assume lowest point is at current point
                float lowestHeight = mapData.heightMap[x, y];
                int lowestX = x;
                int lowestY = y;

                // find lowest neighbor
                for (int j = y - 1; j < y + 1; j++)
                {
                    for (int i = x - 1; i < x + 1; i++)
                    {
                        if (i > 0 && j > 0 && i < size && j < size)
                        {
                            if (mapData.heightMap[i, j] < lowestHeight)
                            {
                                lowestX = i;
                                lowestY = j;
                            }
                        }
                    }
                }

                // move half the water from here to lowest point
                newWater[lowestX, lowestY] += waterVol / 2f;
                waterVol *= .5f;

                newWater[x, y] += waterVol;
            }
        }

        // save new cycle back to waterDatas
        for (int y = 0; y < size; y++)
        {
            for (int x = 0; x < size; x++)
            {
                // evaporate some of the water
                waterDatas[x, y].volume = Mathf.Clamp01(newWater[x, y] * (1 - endlessTerrain.heightMapSettings.erosionSettings.evaporateSpeed));
            }
        }

        meshFilter.mesh.colors = GenerateTexture(0);
    }
}

class WaterData
{
    public float volume;
    public Vector2 direction;
    float speed;

    public WaterData(float _volume, Vector2 _direction, float _speed = 0f)
    {
        volume = _volume;
        direction = _direction;
        speed = _speed;
    }
}
