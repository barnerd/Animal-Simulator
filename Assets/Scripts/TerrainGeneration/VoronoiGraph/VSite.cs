using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Delaunay
{
    public class VSite
    {
        public float x { get => center.x; }
        public float y { get => center.y; }
        public VPoint center;
        public List<VHalfEdge> edges;
        public List<VSite> neighbors;

        public VSite(VPoint p)
        {
            center = p;
            edges = new List<VHalfEdge>();
            neighbors = new List<VSite>();
        }

        public VSite(float _x, float _y)
        {
            center = new VPoint(_x, _y);
            edges = new List<VHalfEdge>();
            neighbors = new List<VSite>();
        }

        public VSite(VSite s)
        {
            center = new VPoint(s.center);
            edges = new List<VHalfEdge>();
            foreach (var e in s.edges)
            {
                edges.Add(new VHalfEdge(e));
            }
            neighbors = new List<VSite>();
            foreach (var n in s.neighbors)
            {
                neighbors.Add(n);
            }
        }
    }
}
