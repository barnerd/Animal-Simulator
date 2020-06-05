using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VoronoiLib;
using VoronoiLib.Structures;

public class VoronoiGraph
{
    // graph pointers
    private Dictionary<VPoint, FortuneSite> sites;
    private LinkedList<VEdge> edges;

    private List<FortuneSite>[,] centersInGridLookup;
    private int gridLookupSize;// = 32;

    // Mesh variables
    Dictionary<Vector2, int> vertexIndices = new Dictionary<Vector2, int>();
    int numVertices;
    List<Vector3> vertices = new List<Vector3>();
    List<Color> colors = new List<Color>();
    List<Vector2> uvs = new List<Vector2>();
    List<int> triangles = new List<int>();
    public Mesh mesh;

    public Dictionary<VPoint, FortuneSite> Sites() { return sites; }
    public LinkedList<VEdge> Edges() { return edges; }

    public VoronoiGraph(List<VPoint> points)
    {
        sites = new Dictionary<VPoint, FortuneSite>();

        List<FortuneSite> fortunePoints = new List<FortuneSite>();

        /* convert points to sites
         * can sort the sites before sending
         * can errorcheck the points, such as too close to each other
         */
        foreach (var point in points)
        {
            fortunePoints.Add(new FortuneSite(point.X, point.Y));
        }

        // run Fortune's Algorithm to get Voronoi diagram
        edges = FortunesAlgorithm.Run(fortunePoints, -400f, -400f, 400f, 400f);

        foreach (var edge in edges)
        {
            if (!sites.ContainsKey(edge.Left.Site))
            {
                sites.Add(edge.Left.Site, edge.Left);
            }
            if (!sites.ContainsKey(edge.Right.Site))
            {
                sites.Add(edge.Right.Site, edge.Right);
            }
        }
    }

    /*public List<VSite> FindClosestSites(Vector2 point, float radius)
    {
        List<VSite> closeSites = new List<VSite>();

        // find grids within radius
        Vector2 radiusV = new Vector2(radius, radius);
        Vector2 min = point - radiusV;
        Vector2 max = point + radiusV;

        for (int y = Mathf.FloorToInt(min.y / gridLookupSize); y <= Mathf.FloorToInt(max.y / gridLookupSize); y++)
        {
            for (int x = Mathf.FloorToInt(min.x / gridLookupSize); x <= Mathf.FloorToInt(max.x / gridLookupSize); x++)
            {
                // check x,y is within bounds
                // TODO: Define the bounds better
                if (x >= 0 && y >= 0 && x < Mathf.FloorToInt(gridSize / gridLookupSize) && y < Mathf.FloorToInt(gridSize / gridLookupSize))
                {
                    if (centersInGridLookup[x, y] != null)
                    {
                        foreach (var site in centersInGridLookup[x, y])
                        {
                            if (Vector2.Distance(point, site) < radius)
                            {
                                closeSites.Add(sites[site]);
                            }
                        }
                    }
                }
            }
        }

        return closeSites;
    }*/

    public Mesh CreateMesh()
    {
        numVertices = 0;
        vertexIndices.Clear();
        vertices.Clear();
        colors.Clear();
        uvs.Clear();
        triangles.Clear();

        foreach (var edge in edges)
        {
            var start = edge.Start;
            var end = edge.End;

            // left triangle
            AddTriangleIndex(edge.Left.Site, true);
            AddTriangleIndex(start);
            AddTriangleIndex(end);

            // right triangle
            AddTriangleIndex(edge.Right.Site, true);
            AddTriangleIndex(end);
            AddTriangleIndex(start);
        }

        mesh = new Mesh();

        mesh.vertices = vertices.ToArray();
        mesh.triangles = triangles.ToArray();
        mesh.uv = uvs.ToArray();
        mesh.colors = colors.ToArray();
        mesh.RecalculateNormals();
        //mesh.normals = bakedNormals;

        return mesh;
    }

    private void AddTriangleIndex(Vector2 _point, bool center = false)
    {
        if (!vertexIndices.ContainsKey(_point))
        {
            vertexIndices.Add(_point, numVertices);
            vertices.Add(new Vector3((float)_point.x, 1f, (float)_point.y));
            uvs.Add(new Vector2((float)_point.x / 800f, (float)_point.y / 800f));
            colors.Add((center) ? Color.blue : Color.white);
            numVertices++;
        }
        triangles.Add(vertexIndices[_point]);
    }
}
