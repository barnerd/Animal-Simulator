using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
public class WaterBody : MonoBehaviour
{
    [SerializeField]
    private Ground ground;

    [SerializeField]
    private float seaLevel;

    public int resolution = 8;
    public int size = 256;
    public int heightScale = 8;

    // mesh variables
    private Vector3[] vertices;
    private int[] triangles;
    private Vector2[] uvs;
    private Vector3[] bakedNormals;
    private Mesh mesh;


    // Start is called before the first frame update
    void Start()
    {
        GenerateMeshVariables();

        // create mesh
        mesh = new Mesh();
        mesh = CreateMesh();

        GetComponent<MeshFilter>().sharedMesh = mesh;
        GetComponent<MeshCollider>().sharedMesh = mesh;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public Vector3 GetClosestPointAboveGround(Vector3 _target)
    {
        // iterate over all surface points of the water body.
        // double check that they're above ground
        // find closest point

        return _target;
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

                Vector3 vertexPosition = new Vector3(x, seaLevel * heightScale, y);

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
