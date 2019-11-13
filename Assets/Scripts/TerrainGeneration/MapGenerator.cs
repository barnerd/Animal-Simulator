using System;
using System.Threading;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class MapGenerator
{
    public static MapData GenerateMapData(int size, Vector2 center, HeightMapSettings _settings)
    {
        float[,] values = Noise.GeneratePerlinNoiseMap(size + 2, center, _settings.noiseSettings);

        for (int y = 0; y < size; y++)
        {
            for (int x = 0; x < size; x++)
            {
                values[x, y] *= _settings.heightMultiplier;
            }
        }

        return new MapData(values);
    }
}

public struct MapData
{
    public readonly float[,] heightMap;

    public MapData(float[,] _heightMap)
    {
        heightMap = _heightMap;
    }
}