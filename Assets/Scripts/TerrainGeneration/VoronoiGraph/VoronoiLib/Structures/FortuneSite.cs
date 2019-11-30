using System.Collections.Generic;

namespace VoronoiLib.Structures
{
    public class FortuneSite
    {
        public VPoint Center { get; }

        public List<VEdge> Edges { get; private set; }

        public List<FortuneSite> Neighbors { get; private set; }

        public FortuneSite GhostParent { get; }

        public List<VPoint> Corners { get; private set; }

        public bool CornersComplete { get; private set; }

        public FortuneSite(double x, double y, FortuneSite parent = null)
        {
            Center = new VPoint(x, y);
            Edges = new List<VEdge>();
            Neighbors = new List<FortuneSite>();
            Corners = new List<VPoint>();

            CornersComplete = false;
            GhostParent = parent;
        }

        public void AddEdge(VEdge _edge)
        {
            if (!Edges.Contains(_edge))
            {
                Edges.Add(_edge);
                AddCorner(_edge.Start);
                AddCorner(_edge.End);
            }
        }

        public void AddCorner(VPoint _corner)
        {
            if (!Corners.Contains(_corner))
            {
                Corners.Add(_corner);
            }
        }

        public bool Contains(VEdge _edge) { return Edges.Contains(_edge); }
        public bool Contains(FortuneSite _node) { return Neighbors.Contains(_node); }
        public bool Contains(VPoint _corner) { return Corners.Contains(_corner); }

        public bool SortCorners(bool clockwise = true)
        {
            // don't execute on a sorted list
            if (!CornersComplete)
            {
                List<VPoint> newCorners = new List<VPoint>();
                List<VEdge> edgesToBeAdded = new List<VEdge>(Edges);

                // pick a starting point
                VPoint point = Corners[0];

                bool foundEdge = false;

                while (edgesToBeAdded.Count > 0)
                {
                    // find the edge with that point at edge.Start
                    foreach (var edge in edgesToBeAdded)
                    {
                        foundEdge = false;

                        if (edge.Contains(point))
                        {
                            foundEdge = true;
                            // add first point in order
                            if (newCorners.Count == 0)
                            {
                                // TODO: figure out why one of these is null
                                if (edge.Start == null || edge.End == null)
                                {
                                    return false;
                                }
                                // Center -> Start -> End is clockwise
                                if (VPoint.Clockwise(Center, edge.Start, edge.End) && clockwise)
                                {
                                    newCorners.Add(edge.Start);
                                    newCorners.Add(edge.End);
                                    point = edge.End;
                                }
                                else
                                {
                                    newCorners.Add(edge.End);
                                    newCorners.Add(edge.Start);
                                }
                            }
                            else
                            {
                                // add the other end of the edge and move to it
                                if (point == edge.Start)
                                {
                                    newCorners.Add(edge.End);
                                    point = edge.End;
                                }
                                else
                                {
                                    newCorners.Add(edge.Start);
                                    point = edge.Start;
                                }
                            }

                            edgesToBeAdded.Remove(edge);
                            break;
                        }
                    }

                    if (!foundEdge)
                    {
                        return false;
                    }
                }

                // if Corner[0] is the same as the last corner, then return true
                if (newCorners[0] == newCorners[newCorners.Count - 1])
                {
                    newCorners.RemoveAt(newCorners.Count - 1);
                    Corners = newCorners;
                    CornersComplete = true;
                }
            }

            return CornersComplete;
        }
    }
}
