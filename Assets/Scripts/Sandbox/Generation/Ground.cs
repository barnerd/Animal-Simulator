using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ground : MonoBehaviour
{
    public int resolution = 8;
    public int size = 256;
    public int heightScale = 8;
    private float[,] heights;

    public NoiseMapSettings noiseSettings;

    // mesh variables
    private Vector3[] vertices;
    private int[] triangles;
    private Vector2[] uvs;
    private Vector3[] bakedNormals;
    private Mesh mesh;

    // Start is called before the first frame update
    void Start()
    {
        GenerateHeights();
        GenerateMeshVariables();

        // create mesh
        mesh = new Mesh();
        mesh = CreateMesh();

        GetComponent<MeshFilter>().sharedMesh = mesh;
        GetComponent<MeshCollider>().sharedMesh = mesh;


        CreateWalls();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public float GetHeightAtXZ(float _x, float _z)
    {
        if (heights == null) GenerateHeights();

        int x = Mathf.RoundToInt(_x);
        int z = Mathf.RoundToInt(_z);

        return heights[x, z] * heightScale;
    }

    private void CreateWalls()
    {
        float edgeDelta = 0.01f;

        // setup 4 walls
        CreateWall("north", 0f * Vector3.up, (size - edgeDelta) * Vector3.forward);
        CreateWall("south", 180f * Vector3.up, edgeDelta * Vector3.forward + size * Vector3.right);
        CreateWall("west", 270f * Vector3.up, edgeDelta * Vector3.right);
        CreateWall("east", 90f * Vector3.up, (size - edgeDelta) * Vector3.right + size * Vector3.forward);
    }

    private void CreateWall(string _name, Vector3 _rotation, Vector3 _position)
    {
        Vector3[] vertices = new Vector3[] { new Vector3(-0.5f, -1f), new Vector3(-0.5f, 1f), new Vector3(1.5f, 1f), new Vector3(1.5f, -1f) };
        Vector2[] uvs = new Vector2[] { new Vector2(0, 0), new Vector2(0, 1), new Vector2(1, 1), new Vector2(1, 0) };
        int[] triangles = new int[] { 0, 1, 2, 0, 2, 3 };

        Mesh wallMesh = new Mesh();
        wallMesh.name = _name;
        wallMesh.vertices = vertices;
        wallMesh.uv = uvs;
        wallMesh.triangles = triangles;
        wallMesh.RecalculateNormals();
        GameObject wall = new GameObject(_name + " wall");
        wall.AddComponent<MeshFilter>().mesh = wallMesh;
        wall.AddComponent<MeshCollider>();
        wall.transform.localScale = Vector3.one * size;
        wall.transform.position = _position;
        wall.transform.Rotate(_rotation);
        wall.transform.parent = transform;
    }

    private void GenerateHeights()
    {
        heights = new float[size + 1, size + 1];
        heights = Noise.GeneratePerlinNoiseMap(size + 1, Vector2.zero, noiseSettings);
    }

    private void GenerateMeshVariables()
    {
        int verticesPerLine = size / resolution + 1;
        int triangleIndex = 0;

        vertices = new Vector3[verticesPerLine * verticesPerLine];
        uvs = new Vector2[verticesPerLine * verticesPerLine];
        triangles = new int[(verticesPerLine - 1) * (verticesPerLine - 1) * 6];

        for (int y = 0; y <= size; y += resolution)
        {
            for (int x = 0; x <= size; x += resolution)
            {
                int vertexIndex = x / resolution + y / resolution * verticesPerLine;
                Vector2 percent = new Vector2(x / (float)size, y / (float)size);

                Vector3 vertexPosition = new Vector3(x, heights[x, y] * heightScale, y);

                vertices[vertexIndex] = vertexPosition;

                uvs[vertexIndex] = percent;

                if (x != size && y != size)
                {
                    int a = vertexIndex;
                    int b = a + 1;
                    int c = vertexIndex + verticesPerLine;
                    int d = c + 1;

                    triangles[triangleIndex + 0] = a;
                    triangles[triangleIndex + 1] = c;
                    triangles[triangleIndex + 2] = d;

                    triangleIndex += 3;

                    triangles[triangleIndex + 0] = d;
                    triangles[triangleIndex + 1] = b;
                    triangles[triangleIndex + 2] = a;

                    triangleIndex += 3;
                }
            }
        }
    }

    private Mesh CreateMesh()
    {
        mesh.name = "Ground Mesh";
        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.uv = uvs;
        mesh.RecalculateNormals();

        return mesh;
    }
}
