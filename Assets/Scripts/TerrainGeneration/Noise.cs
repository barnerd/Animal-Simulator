using System.Collections;
using UnityEngine;

public static class Noise
{
    public static float[,] GeneratePerlinNoiseMap(int _size, Vector2 offset, NoiseMapSettings settings)
    {
        float[,] noiseMap = new float[_size, _size];
        float maxPossibleHeight = 0;
        float minPossibleHeight = 0;
        float amplitude = 1;
        float frequency = 1;

        System.Random prng = new System.Random(settings.seed);
        Vector2[] octaveOffsets = new Vector2[settings.octaves];
        for (int o = 0; o < settings.octaves; o++)
        {
            float offsetX = prng.Next(-100000, 100000) + offset.x;
            float offsetY = prng.Next(-100000, 100000) - offset.y;
            octaveOffsets[o] = new Vector2(offsetX, offsetY);

            maxPossibleHeight += amplitude;
            minPossibleHeight -= amplitude;
            amplitude *= settings.persistance;
        }

        if (settings.scale <= 0)
        {
            settings.scale = 0.0001f;
        }

        float halfWidth = _size / 2f;
        float halfHeight = _size / 2f;

        for (int y = 0; y < _size; y++)
        {
            for (int x = 0; x < _size; x++)
            {
                amplitude = 1;
                frequency = 1;
                float noiseHeight = 0;

                for (int o = 0; o < settings.octaves; o++)
                {
                    float multiplier = frequency / settings.scale;
                    float sampleX = multiplier * (octaveOffsets[o].x + x - halfWidth);
                    float sampleY = multiplier * (octaveOffsets[o].y + y - halfHeight);

                    // generates random noise in [-1,1]
                    float perlinValue = Mathf.PerlinNoise(sampleX, sampleY) * 2 - 1;
                    noiseHeight += perlinValue * amplitude;

                    amplitude *= settings.persistance;
                    frequency *= settings.lacunarity;
                }

                // normalize noiseMap across all noiseMaps
                noiseMap[x, y] = (noiseHeight - minPossibleHeight) / (2f * maxPossibleHeight);
            }
        }

        return noiseMap;
    }
}
