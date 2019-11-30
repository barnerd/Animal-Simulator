using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Planet
{
    /* stats
     * Earth is 40,000 km "wide"
     * Earth is 20,000 km "high"
     * so let's aim for 100th of earth
     * game is 400 km, or 400,000 m wide
     * game is 200 km, or 200,000 m high
     * if each cell is 10m, and each chunk is 119 x 119,
     * then game has 40,000 x 20,000 cells
     * and 333 x 167 chunks
     *
     * let's go the other way
     * game has 512 x 256 chunks
     * each chunk is 119 x 119
     * game is 609,280 m x 304,640 m
     * or 609 km x 304 km
     * the game is roughly the size of Oregon
     * 
     * */
    static System.Random prng;

    public static PlanetData GenerateTectonicPlates(int size, int numPlates, int seed = 0)
    {
        var sw = new System.Diagnostics.Stopwatch();
        sw.Start();
        prng = new System.Random(seed);

        int[,] map = new int[2 * size, size];
        int[] index = ShuffleArrayIndices(2 * size * size);
        int emptyCells = 2 * size * size;

        TectonicPlate[] plates = new TectonicPlate[numPlates];

        // starting points
        for (int i = 0; i < numPlates; i++)
        {
            int indexX = index[i] % (2 * size);
            int indexY = index[i] / (2 * size);

            map[indexX, indexY] = i + 1;
            emptyCells--;
            plates[i] = new TectonicPlate(new Vector2(
                (float)prng.NextDouble() * 2 * TectonicPlate.maxSpeed - TectonicPlate.maxSpeed,
                (float)prng.NextDouble() * 2 * TectonicPlate.maxSpeed - TectonicPlate.maxSpeed),
                (float)prng.NextDouble() * 2 * TectonicPlate.maxRotation - TectonicPlate.maxRotation, // range of [-360,360]
                prng.Next(100000) % 2 == 0);

            plates[i].size++;
        }


        while (emptyCells > 0 && numPlates > 0 && size < 500)
        {
            for (int i = 0; i < 2 * size * size; i++)
            {
                int indexX = index[i] % (2 * size);
                int indexY = index[i] / (2 * size);

                if (map[indexX, indexY] == 0)
                {
                    List<int> neighbors = GetNeighborPlates(map, index[i], 2 * size, size);
                    if (neighbors.Count > 0)
                    {
                        int r = prng.Next(0, neighbors.Count);
                        map[indexX, indexY] = neighbors[r];
                        emptyCells--;
                        plates[map[indexX, indexY] - 1].size++;

                    }
                }
            }
        }

        // find Center of each plate
        TectonicPlate thisPlate;

        for (int y = 0; y < size; y++)
        {
            for (int x = 0; x < 2 * size; x++)
            {
                thisPlate = plates[map[x, y] - 1];
                thisPlate.center += new Vector2(x, y);
            }
        }
        for (int i = 0; i < numPlates; i++)
        {
            plates[i].center /= plates[i].size;
            //Debug.Log(plates[i].center);
        }

        //Debug.Log((int)sw.ElapsedMilliseconds);
        return new PlanetData(size, numPlates, map, plates);
    }

    static List<int> GetNeighborPlates(int[,] map, int index, int width, int height)
    {
        int centerX = index % width;
        int centerY = index / width;

        List<int> neighbors = new List<int>();

        // search 2 around the center for a faster implmentation, but with noisier boundaries
        for (int y = centerY - 1; y <= centerY + 1; y++)
        {
            for (int x = centerX - 1; x <= centerX + 1; x++)
            {
                int neighborX = (x + width) % width;

                if (y >= 0 && y < height && !(x == centerX && y == centerY))
                {
                    if (map[neighborX, y] != 0)
                    {
                        neighbors.Add(map[neighborX, y]);
                    }
                }
            }
        }

        return neighbors;
    }

    public static void MovePlates(PlanetData _data)
    {
        int[,] newMap = new int[2 * _data.size, _data.size];
        Vector2 newPosition;
        float sine, cosine;

        for (int y = 0; y < _data.size; y++)
        {
            for (int x = 0; x < 2 * _data.size; x++)
            {
                if (_data.plateMap[x, y] != 0)
                //if (_data.plateMap[x, y] == 3)
                {
                    TectonicPlate currentPlate = _data.plates[_data.plateMap[x, y] - 1];
                    newPosition = new Vector2(x, y);

                    // rotate plate
                    sine = Mathf.Sin(currentPlate.rotationAngle);
                    cosine = Mathf.Cos(currentPlate.rotationAngle);
                    Vector2 rotationOrigin = new Vector2(x, y) - currentPlate.center;

                    //newPosition.x = rotationOrigin.x * cosine - rotationOrigin.y * sine + currentPlate.center.x;
                    //newPosition.y = rotationOrigin.x * sine + rotationOrigin.y * cosine + currentPlate.center.y;

                    // move plate
                    float theta = Mathf.Atan2(newPosition.y, newPosition.x) + currentPlate.direction.x * currentPlate.speed;
                    float phi = Mathf.Atan2(Mathf.Sqrt(newPosition.x * newPosition.x), newPosition.y);
                    float newX = Mathf.Cos(theta) * Mathf.Sin(phi);
                    newPosition += currentPlate.direction * currentPlate.speed;
                    currentPlate.center += currentPlate.direction * currentPlate.speed;

                    newPosition.x = (newPosition.x + 2 * _data.size) % (2 * _data.size);

                    if ((int)newPosition.y >= 0 && (int)newPosition.y < _data.size)
                    {
                        newMap[(int)newPosition.x, (int)newPosition.y] = _data.plateMap[x, y];
                    }
                }
            }
        }

        for (int y = 0; y < _data.size; y++)
        {
            for (int x = 0; x < 2 * _data.size; x++)
            {
                _data.plateMap[x, y] = newMap[x, y];
            }
        }
    }

    static int[] ShuffleArrayIndices(int size)
    {
        int[] indices = new int[size];
        for (int i = 0; i < size; i++)
        {
            indices[i] = i;
        }

        for (int i = 0; i < size - 1; i++)
        {
            int tmp = indices[i];
            int r = prng.Next(i, size);
            indices[i] = indices[r];
            indices[r] = tmp;
        }

        return indices;
    }
}

public struct PlanetData
{
    public readonly int size;
    public readonly int numPlates;
    public int[,] plateMap;
    public readonly TectonicPlate[] plates;

    public PlanetData(int _size, int _numPlates, int[,] _plateMap, TectonicPlate[] _plates)
    {
        size = _size;
        numPlates = _numPlates;
        plateMap = _plateMap;
        plates = _plates;
    }
}
