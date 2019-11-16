using UnityEngine;

public static class Erosion
{
    // indices and weights of erosion brush
    static int[][] erosionBrushIndices;
    static float[][] erosionBrushWeights;

    static int currentErosionRadius;

    static float epsilon = 1.0e-8f;

    static void Initialize(int mapSize, int erosionRadius)
    {

        if (erosionBrushIndices == null || currentErosionRadius != erosionRadius)
        {
            InitializeBrushIndices(mapSize, erosionRadius);
            currentErosionRadius = erosionRadius;
        }
    }

    public static void Erode(float[,] map, int mapSize, ErosionSettings settings)
    {
        System.Random prng = new System.Random(settings.seed);

        Initialize(mapSize, settings.erosionRadius);

        for (int iteration = 0; iteration < settings.numIterations; iteration++)
        {
            // create water dropet at random point on map
            Vector2 position = new Vector2(prng.Next(0, mapSize - 1), prng.Next(0, mapSize - 1));
            Vector2 direction = Vector2.zero;
            float speed = settings.initialSpeed;
            float water = settings.initialWaterVolume;
            float sediment = 0f;

            for (int lifetime = 0; lifetime < settings.maxDropletLifetime; lifetime++)
            {
                int nodeX = (int)position.x;
                int nodeY = (int)position.y;

                // calculate droplet's offset inside the cell (0, 0) = at NW node, (1,1) = at SE node
                Vector2 cellOffset = new Vector2(position.x - nodeX, position.y - nodeY);

                // calculate droplet's height and direction of flow with bilinear interpolation of surrounding heights
                HeightAndGradient heightAndGradient = CalculateHeightAndGradient(map, position);

                // update the droplet's direction and position (move position 1 unit regardless of speed);
                direction.x = (direction.x * settings.inertia - heightAndGradient.gradient.x * (1 - settings.inertia));
                direction.y = (direction.y * settings.inertia - heightAndGradient.gradient.y * (1 - settings.inertia));
                // normalize direction
                direction.Normalize();
                position += direction;

                // stop simulating droplet if it's not moving or has flowed off the map
                if ((direction.x < epsilon && direction.y < epsilon) || position.x < 0 || position.x >= mapSize - 1 || position.y < 0 || position.y >= mapSize - 1)
                {
                    break;
                }

                // find the droplet's new height and calculate the deltaHeight
                float newHeight = CalculateHeightAndGradient(map, position).height;
                float deltaHeight = newHeight - heightAndGradient.height;

                // calculat the droplet's sediment capacity (higher when moving fast down a slope and contains lots of water)
                float sedimentCapacity = Mathf.Max(-deltaHeight * speed * water * settings.sedimentCapacityFactor, settings.minSedimentCapacity);

                // if carrying more sediment than capacity, or if flowing uphill
                if (sediment > sedimentCapacity || deltaHeight > 0)
                {
                    // if moving uphill, try to fill up to the current height, otherwise deposit a fraction of the excess sediment
                    float amountToDeposit = (deltaHeight > 0) ? Mathf.Min(deltaHeight, sediment) : (sediment - sedimentCapacity) * settings.depositySpeed;
                    sediment -= amountToDeposit;

                    // add the sediment to the four nodes of the current cell using bilinear interpolation
                    map[nodeX, nodeY] += amountToDeposit * (1 - cellOffset.x) * (1 - cellOffset.y);
                    map[nodeX + 1, nodeY] += amountToDeposit * cellOffset.x * (1 - cellOffset.y);
                    map[nodeX, nodeY + 1] += amountToDeposit * (1 - cellOffset.x) * cellOffset.y;
                    map[nodeX + 1, nodeY + 1] += amountToDeposit * cellOffset.x * cellOffset.y;
                }
                else
                {
                    // erode a fraction of the droplet's current carry capacity
                    // clamp the erosion to the change in height so that it doesn't dig a hole in the terrain behind the droplet
                    float amountToErode = Mathf.Min((sedimentCapacity - sediment) * settings.erodeSpeed, -deltaHeight);

                    // use erosion brush to erode from all nodes inside the droplet's erosion radius
                    for (int brushPointIndex = 0; brushPointIndex < erosionBrushIndices[nodeY * mapSize + nodeX].Length; brushPointIndex++)
                    {
                        int nodeIndex = erosionBrushIndices[nodeY * mapSize + nodeX][brushPointIndex];

                        float weighedErodeAmount = amountToErode * erosionBrushWeights[nodeY * mapSize + nodeX][brushPointIndex];
                        float deltaSediment = (map[nodeIndex % mapSize, nodeIndex / mapSize] < weighedErodeAmount) ? map[nodeIndex % mapSize, nodeIndex / mapSize] : weighedErodeAmount;
                        map[Mathf.RoundToInt(nodeIndex % mapSize), Mathf.RoundToInt(nodeIndex / mapSize)] -= deltaSediment;
                        sediment += deltaSediment;
                    }
                }

                // update droplet's speed and water content
                speed = Mathf.Sqrt(speed * speed + deltaHeight * settings.gravity);
                water *= (1 - settings.evaporateSpeed);
            }
        }
    }

    static HeightAndGradient CalculateHeightAndGradient(float[,] nodes, Vector2 position)
    {
        int nodeX = (int)position.x;
        int nodeY = (int)position.y;

        // calculate droplet's offset inside the cell (0, 0) = at NW node, (1,1) = at SE node
        Vector2 cellOffset = new Vector2(position.x - nodeX, position.y - nodeY);

        // calculate heights of the four nodes of the droplet's cell
        float heightNW = nodes[nodeX, nodeY];
        float heightNE = nodes[nodeX + 1, nodeY];
        float heightSW = nodes[nodeX, nodeY + 1];
        float heightSE = nodes[nodeX + 1, nodeY + 1];

        // calculate droplet's direction of flow with bilinear interpolation of height difference along the edges
        Vector2 gradient = new Vector2(
            (heightNE - heightNW) * (1 - cellOffset.y) + (heightSE - heightSW) * cellOffset.y,
            (heightSW - heightNW) * (1 - cellOffset.x) + (heightSE - heightNE) * cellOffset.x);

        // calculate height with bilinear interpolation of the heights of the nodes of the cell
        float height =
            heightNW * (1 - cellOffset.x) * (1 - cellOffset.y) + 
            heightNE * cellOffset.x * (1 - cellOffset.y) + 
            heightSW * (1 - cellOffset.x) * cellOffset.y + 
            heightSE * cellOffset.x * cellOffset.y;

        return new HeightAndGradient() { height = height, gradient = gradient };
    }

    static void InitializeBrushIndices(int mapSize, int radius)
    {
        erosionBrushIndices = new int[mapSize * mapSize][];
        erosionBrushWeights = new float[mapSize * mapSize][];

        int[] xOffsets = new int[radius * radius * 4];
        int[] yOffsets = new int[radius * radius * 4];
        float[] weights = new float[radius * radius * 4];
        float weightSum = 0;
        int addIndex = 0;

        for (int i = 0; i < erosionBrushIndices.GetLength(0); i++)
        {
            int centerX = i % mapSize;
            int centerY = i / mapSize;

            if (centerY <= radius || centerY >= mapSize - radius || centerX <= radius + 1 || centerX >= mapSize - radius)
            {
                weightSum = 0;
                addIndex = 0;

                for (int y = -radius; y <= radius; y++)
                {
                    for (int x = -radius; x <= radius; x++)
                    {
                        float sqrDst = x * x + y * y;
                        if (sqrDst < radius * radius)
                        {
                            int coordX = centerX + x;
                            int coordY = centerY + y;

                            if (coordX >= 0 && coordX < mapSize && coordY >= 0 && coordY < mapSize)
                            {
                                float weight = 1 - Mathf.Sqrt(sqrDst) / radius;
                                weightSum += weight;
                                weights[addIndex] = weight;
                                xOffsets[addIndex] = x;
                                yOffsets[addIndex] = y;
                                addIndex++;
                            }
                        }
                    }
                }
            }

            int numEntries = addIndex;
            erosionBrushIndices[i] = new int[numEntries];
            erosionBrushWeights[i] = new float[numEntries];

            for (int j = 0; j < numEntries; j++)
            {
                erosionBrushIndices[i][j] = (yOffsets[j] + centerY) * mapSize + xOffsets[j] + centerX;
                erosionBrushWeights[i][j] = weights[j] / weightSum;
            }
        }

    }

    struct HeightAndGradient
    {
        public float height;
        public Vector2 gradient;
    }
}