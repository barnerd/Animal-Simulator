using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class MapGenerator
{
    public static MapData GenerateMapData(int size, Vector2 center, HeightMapSettings _settings)
    {
        // generate noise
        float[,] values = Noise.GeneratePerlinNoiseMap(size, center, _settings.noiseSettings);

        // hydraulic erosion
        if (_settings.erosionSettings.erode)
        {
            Erosion.Erode(values, size, _settings.erosionSettings);
        }

        // set height of map
        for (int y = 0; y < size; y++)
        {
            for (int x = 0; x < size; x++)
            {
                values[x, y] *= _settings.heightMultiplier;
            }
        }

        return new MapData(values, _settings.heightMultiplier);
    }
}

public struct MapData
{
    // values from [0,1], doubles the memory needed
    // public readonly float[,] normalizedMap;

    // values from [0,heightMultiplier]
    public readonly float[,] heightMap;
    public readonly float heightMultiplier;

    public MapData(float[,] _heightMap, float _scale)
    {
        heightMap = _heightMap;
        heightMultiplier = _scale;
    }
}