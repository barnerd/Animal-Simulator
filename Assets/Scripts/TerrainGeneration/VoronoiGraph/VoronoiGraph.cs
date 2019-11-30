using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using csDelaunay;

public enum VoronoiMeshMode { Delaunay, Voronoi, Edges };

[CreateAssetMenu(menuName = "VoronoiGraph/New Graph")]
public class VoronoiGraph : ScriptableObject
{
    // graph pointers
    private Dictionary<Vector2f, Site> sites;
    private List<Edge> edges;
    private Voronoi voronoiGraph;

    // Mesh variables
    Dictionary<Vector2, int> vertexIndices = new Dictionary<Vector2, int>();
    int numVertices;
    List<Vector3> vertices = new List<Vector3>();
    List<Color> colors = new List<Color>();
    List<Vector2> uvs = new List<Vector2>();
    List<int> triangles = new List<int>();
    public Mesh mesh;
    int gridSize;

    public void CreateGraph(int _size, int numPoints, int relaxation)
    {
        gridSize = _size;

        System.Random prng = new System.Random(0);
        List<Vector2f> points = new List<Vector2f>();
        for (int i = 0; i < numPoints; i++)
        {
            var x = prng.Next(0, gridSize);
            var y = prng.Next(0, gridSize);
            points.Add(new Vector2f(x, y));
        }

        voronoiGraph = new Voronoi(points, new Rectf(0, 0, gridSize, gridSize), relaxation);
        sites = voronoiGraph.SitesIndexedByLocation;
        edges = voronoiGraph.Edges;
    }

    public Mesh CreateMesh(VoronoiMeshMode meshMode)
    {
        numVertices = 0;
        vertexIndices.Clear();
        vertices.Clear();
        colors.Clear();
        uvs.Clear();
        triangles.Clear();

        switch (meshMode)
        {
            //case VoronoiMeshMode.Corners:
            /*foreach (var s in graphCenters)
            {
                s.SortCorners();
                Debug.Log((Vector2)s.Center);
                Debug.Log(s.CornersComplete);
                if (s.CornersComplete)
                {
                    if (s.Corners.Count >= 2)
                    {
                        for (int corner = 0; corner < s.Corners.Count; corner++)
                        {
                            AddTriangleIndex(s.Center, true);
                            AddTriangleIndex(s.Corners[(corner == 0) ? s.Corners.Count - 1 : corner - 1]);
                            AddTriangleIndex(s.Corners[corner]);
                        }
                    }
                }
            }*/
            //    break;
            case VoronoiMeshMode.Delaunay:
                foreach (var s in sites)
                {
                    var center = s.Key;
                    var neighbors = s.Value.NeighborSites();
                    if (neighbors.Count >= 2)
                    {
                        for (int i = 0; i < neighbors.Count; i++)
                        {
                            var thirdIndex = (i == 0) ? neighbors.Count - 1 : i - 1;
                            if (Vector2f.Clockwise(center, neighbors[i].Coord, neighbors[thirdIndex].Coord))
                            {
                                AddTriangleIndex(center, true);
                                AddTriangleIndex(neighbors[i].Coord, true);
                                AddTriangleIndex(neighbors[thirdIndex].Coord, true);
                            }
                            else
                            {
                                AddTriangleIndex(center, true);
                                AddTriangleIndex(neighbors[thirdIndex].Coord, true);
                                AddTriangleIndex(neighbors[i].Coord, true);
                            }
                        }
                    }
                }
                break;
            case VoronoiMeshMode.Edges:
                foreach (var edge in edges)
                {
                    //Debug.Log(edge.ToString());
                    if (edge.ClippedEnds != null)
                    {
                        var start = edge.ClippedEnds[LR.LEFT];
                        var end = edge.ClippedEnds[LR.RIGHT];

                        if (Vector2f.Clockwise(edge.LeftSite.Coord, start, end))
                        {
                            AddTriangleIndex(edge.LeftSite.Coord, true);
                            AddTriangleIndex(start);
                            AddTriangleIndex(end);
                        }
                        else
                        {
                            AddTriangleIndex(edge.LeftSite.Coord, true);
                            AddTriangleIndex(end);
                            AddTriangleIndex(start);
                        }

                        if (Vector2f.Clockwise(edge.RightSite.Coord, start, end))
                        {
                            AddTriangleIndex(edge.RightSite.Coord, true);
                            AddTriangleIndex(start);
                            AddTriangleIndex(end);
                        }
                        else
                        {
                            AddTriangleIndex(edge.RightSite.Coord, true);
                            AddTriangleIndex(end);
                            AddTriangleIndex(start);
                        }
                    }
                }
                break;
            case VoronoiMeshMode.Voronoi:
                foreach (var site in sites)
                {
                    var center = site.Key;
                    foreach (var edge in site.Value.Edges)
                    {
                        if (edge.ClippedEnds != null)
                        {
                            var start = edge.ClippedEnds[LR.LEFT];
                            var end = edge.ClippedEnds[LR.RIGHT];

                            AddTriangleIndex(center, true);
                            AddTriangleIndex(start, true);
                            AddTriangleIndex(end, true);
                        }
                    }
                }
                break;
            default:
                break;
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
            uvs.Add(new Vector2((float)_point.x / gridSize, (float)_point.y / gridSize));
            colors.Add((center) ? Color.blue : Color.white);
            numVertices++;
        }
        triangles.Add(vertexIndices[_point]);
    }
}
