using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlantMesh : MonoBehaviour
{
    const float PI2 = Mathf.PI * 2f;

    public Tree tree;

    public Mesh mesh;

    [Range(4, 10)]
    public int radialSegments;

    List<Vector3> vertices;
    List<Vector3> normals;
    List<Vector2> uvs;
    List<int> triangles;

    // Start is called before the first frame update
    void Start()
    {
        CreateMesh();
        AssignMesh();
    }

    // Update is called once per frame
    void Update()
    {
        //UpdateMesh();
        CreateMesh();
        AssignMesh();
    }

    void AssignMesh()
    {
        GetComponent<MeshFilter>().mesh = mesh;
        GetComponent<MeshCollider>().sharedMesh = mesh;
    }

    void InitVariables()
    {
        vertices = new List<Vector3>();
        normals = new List<Vector3>();
        uvs = new List<Vector2>();
        triangles = new List<int>();
    }

    void CreateMesh()
    {
        InitVariables();

        foreach (var bud in tree.buds)
        {
            float v = bud.lengthFromRoot;

            // create radialSegments number of vertices around bud at radius distance around bud.direction
            for (int i = 0; i < radialSegments; i++)
            {
                // TODO: Figure out if u should go from 0 to 1 around the branch, or 0 to 1 for each meter of circumference
                float u = 1f * i / radialSegments;
                float rad = PI2 * u;

                var normal = Quaternion.AngleAxis(rad * Mathf.Rad2Deg, (bud.parent == null) ? bud.direction : (bud.position - bud.parent.position).normalized) * tree.transform.forward;
                normal.Normalize();
                normals.Add(normal);

                bud.vertexIndices.Add(vertices.Count);
                vertices.Add(bud.position + normal * bud.branchRadius);

                uvs.Add(new Vector2(u, v));
            }

            // if tip, create a cap around the bud
            if (bud.type == BudType.Tip || bud.type == BudType.RootTip)
            {
                var normal = bud.direction.normalized;
                normals.Add(normal);

                bud.tipIndex = vertices.Count;
                vertices.Add(bud.position + normal * bud.branchRadius / 2f);

                uvs.Add(new Vector2(.5f, v + bud.branchRadius / 2f));
            }

            // if leaf, also add planes (mesh?) for leaves from bud to bud.parent
            // (this depends on the plant, i.e.
            // some will be from bud to bud like pine trees
            // some will be just planes around the bud like flowers
            // some will be planes for whole twigs like willow trees
            // if fruit, instantiate a fruit object. probably not here, but in plant class
        }

        foreach (var bud in tree.buds)
        {
            // create radialSegments number of vertices around bud at radius distance around bud.direction
            if (bud.parent != null)
            {
                // connect those vertices with the parent vertices
                MakeBranchTriangles(bud, radialSegments - 1, 0);

            }
            if (bud.type == BudType.Tip || bud.type == BudType.RootTip)
            {
                // if tip, create a cap around the bud
                MakeTipTriangles(bud, radialSegments - 1, 0);
            }

            for (int i = 1; i < radialSegments; i++)
            {
                if (bud.parent != null)
                {
                    // connect those vertices with the parent vertices
                    MakeBranchTriangles(bud, i - 1, i);
                }
                if (bud.type == BudType.Tip || bud.type == BudType.RootTip)
                {
                    // if tip, create a cap around the bud
                    MakeTipTriangles(bud, i - 1, i);
                }
            }
        }

        mesh = new Mesh();
        mesh.vertices = vertices.ToArray();
        mesh.normals = normals.ToArray();
        mesh.uv = uvs.ToArray();
        mesh.triangles = triangles.ToArray();
    }

    void UpdateMesh()
    {

    }

    void MakeTipTriangles(PlantBud _bud, int _i1, int _i2)
    {
        var a = _bud.vertexIndices[_i1]; // lowerLeft
        var b = _bud.vertexIndices[_i2]; // lowerRight
        var c = _bud.tipIndex; // tip

        triangles.Add(a); triangles.Add(b); triangles.Add(c);
    }

    void MakeBranchTriangles(PlantBud _bud, int _i1, int _i2)
    {
        var a = _bud.vertexIndices[_i1]; // upperLeft
        var b = _bud.vertexIndices[_i2]; // upperRight
        var c = _bud.parent.vertexIndices[_i1]; // lowerLeft
        var d = _bud.parent.vertexIndices[_i2]; // lowerRight

        triangles.Add(a); triangles.Add(d); triangles.Add(b);
        triangles.Add(a); triangles.Add(c); triangles.Add(d);
    }
}
