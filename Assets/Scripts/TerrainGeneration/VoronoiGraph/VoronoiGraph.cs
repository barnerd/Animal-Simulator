using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Delaunay
{
    public class VoronoiGraph
    {
        // graph pointers
        private Dictionary<VPoint, VSite> sites;
        private List<VHalfEdge> edges;

        private List<VSite>[,] centersInGridLookup;
        private int gridLookupSize = 32;

        // Mesh variables
        /*
        Dictionary<Vector2, int> vertexIndices = new Dictionary<Vector2, int>();
        int numVertices;
        List<Vector3> vertices = new List<Vector3>();
        List<Color> colors = new List<Color>();
        List<Vector2> uvs = new List<Vector2>();
        List<int> triangles = new List<int>();
        public Mesh mesh;*/

        // points need to in the set x = [0, 360), y = [0,180)
        public VoronoiGraph(List<VSphericalPoint> points)
        {
            // from https://courses.cs.washington.edu/courses/cse326/00wi/projects/voronoi.html
            // [1] Initialize edges and sites to be empty.
            sites = new Dictionary<VPoint, VSite>();
            edges = new List<VHalfEdge>();

            // [2] Add three or four "points at infinity" to sites, to bound the diagram.
            // No bounds

            List<VSphericalPoint> intersections = new List<VSphericalPoint>();

            //[3] For each point p in points, do:
            foreach (var p in points)
            {
                //Debug.Log("checking point " + p.ToString("0.####"));

                //[4] Create new site newSite, with p as its center.
                VSite newSite = new VSite(p.ToVPoint());

                //[5] For each existing site site in sites.Values, do:
                foreach (var site in sites.Values)
                {
                    var siteCenter = new VSphericalPoint(site.x, site.y);
                    //Debug.Log("checking site " + siteCenter.ToString("0.####"));
                    var pbNormal = new VSphericalPoint(siteCenter.euclidean - p.euclidean);

                    //[6] Find the halfway line between newSite and site(this is the perpendicular bisector of the line segment connecting the two sites). Call this pb.
                    VSphericalPoint pb;

                    if (p.IsOpposite(siteCenter))
                    {
                        // from: https://sciencing.com/vector-perpendicular-8419773.html
                        Debug.Log("antipoles: be careful, p and site are opposite. currently picking arbitrary point on the great circle");
                        // pick random pb, but all points on the great circle are valid
                        pb = new VSphericalPoint(new Vector3(1, (-pbNormal.euclidean.x - pbNormal.euclidean.z) / pbNormal.euclidean.y, 1));
                    }
                    else
                    {
                        pb = new VSphericalPoint((p.euclidean + siteCenter.euclidean) * 0.5f);
                    }

                    //Debug.Log("pb: " + pb.ToString("0.####"));
                    //Debug.Log("pbNormal: " + pbNormal.ToString("0.####"));

                    //[7] Create a data structure X to hold the critical points (see step 11).
                    intersections.Clear();

                    //Debug.Log("Checking " + site.edges.Count + " edges");
                    //[8] For each edge e of site, do:
                    if (site.edges.Count == 0)
                    {
                        //Debug.Log("Checking " + site.edges.Count + " edges. no good if happening more than once");
                        // First site only
                        // great circle pb is the edge
                        // create edges from pb to opposite pb and from opposite pb to pb

                        var pbQuarter = new VSphericalPoint(Vector3.Cross(p.euclidean, siteCenter.euclidean));
                        var pbHalf = pb.OppositePoint();
                        var pbQuarterO = pbQuarter.OppositePoint();

                        if (Vector3.Dot(pbNormal.euclidean, Vector3.Normalize(Vector3.Cross(pb.euclidean, pbQuarterO.euclidean))) < 1e-5 ||
                            Vector3.Dot(pbNormal.euclidean, Vector3.Normalize(Vector3.Cross(pbQuarterO.euclidean, pbHalf.euclidean))) < 1e-5 ||
                            Vector3.Dot(pbNormal.euclidean, Vector3.Normalize(Vector3.Cross(pbHalf.euclidean, pbQuarter.euclidean))) < 1e-5 ||
                            Vector3.Dot(pbNormal.euclidean, Vector3.Normalize(Vector3.Cross(pbQuarter.euclidean, pb.euclidean))) < 1e-5)
                        {
                            Debug.Log("First Great Circle is pointing wrong direction. this will never happen. picking points correctly");
                        }

                        AddEdge(pb.ToVPoint(), pbQuarterO.ToVPoint(), site, newSite);
                        AddEdge(pbQuarterO.ToVPoint(), pbHalf.ToVPoint(), site, newSite);
                        AddEdge(pbHalf.ToVPoint(), pbQuarter.ToVPoint(), site, newSite);
                        AddEdge(pbQuarter.ToVPoint(), pb.ToVPoint(), site, newSite);
                    }
                    else
                    {
                        for (int i = site.edges.Count - 1; i >= 0; i--)
                        {
                            var e = site.edges[i];

                            //[9] Test the spatial relationship between e and pb, using pbNormal.
                            VSphericalPoint intersection = CheckSpacialRelationship(e, pbNormal.euclidean);

                            if (intersection != null)
                            {
                                //Debug.Log("intersections: intersection: " + intersection.ToString("0.####"));
                                //[11b] and store the point of intersection in X.
                                intersections.Add(intersection);
                            }
                        }
                    }

                    //[12] X should now have 0 or 2 points. If it has 2, create a new edge to connect them. Add this edge to site, newSite, and edges.
                    if (intersections.Count == 2)
                    {
                        //Debug.Log("intersections.count == 2. Count: " + intersections.Count);
                        //Debug.Log("order check: pbNormal: " + pbNormal.ToString("0.####"));
                        //Debug.Log("intersections: vector3 dot: " + (Vector3.Dot(intersections[0].euclidean, intersections[1].euclidean) + 1).ToString("0.############0"));
                        //Debug.Log("intersections: abs dot: " + (intersections[0].euclidean.x * intersections[1].euclidean.x + intersections[0].euclidean.y * intersections[1].euclidean.y + intersections[0].euclidean.z * intersections[1].euclidean.z + 1).ToString("0.############0"));
                        //Debug.Log("intersections: IsOpposite: " + intersections[0].IsOpposite(intersections[1]));

                        if (intersections[0].IsOpposite(intersections[1]))
                        {
                            Debug.Log("New edge has antipoles; need to add a midpoint");
                            // use pb as midpoint
                            // TODO: if p and site are antipoles as well, pb may not be choosen correctly
                            if (p.IsOpposite(siteCenter))
                            {
                                Debug.Log("order check: site and newSite are antipoles, so pb is no good. this should never be happening");
                            }
                            else
                            {
                                if (sites.Count == 2)
                                {
                                    VSphericalPoint otherSite = null;
                                    foreach (var s in sites.Values)
                                    {
                                        if (s != site)
                                        {
                                            otherSite = new VSphericalPoint(s.center);
                                        }
                                    }

                                    if (otherSite != null)
                                    {
                                        // for visualization: https://www.mathopenref.com/trianglecircumcenter.html
                                        // pb to otherSite dot pb to positiveIntersection. if negative use pb, else use pb0
                                        VSphericalPoint positiveIntersectionPoint = (Vector3.Dot(pb.euclidean, intersections[0].euclidean) > 0) ? intersections[0] : intersections[1];

                                        VSphericalPoint pbOtherSite = new VSphericalPoint(otherSite.euclidean - pb.euclidean);
                                        VSphericalPoint pbPositiveIntersection = new VSphericalPoint(positiveIntersectionPoint.euclidean - pb.euclidean);

                                        VSphericalPoint intermediatePoint;
                                        if (pb.IsSame(positiveIntersectionPoint)) Debug.Log("pb and +i are not in the same direction: " + pb.IsSame(positiveIntersectionPoint));
                                        if (Mathf.Abs(pbPositiveIntersection.magnitude) < 1e-8)
                                        {
                                            Debug.Log("pb and +i are in the same direction: " + pb.IsSame(positiveIntersectionPoint));
                                            intermediatePoint = new VSphericalPoint(Vector3.Cross(pb.euclidean, pbNormal.euclidean));
                                        }
                                        else
                                        {
                                            intermediatePoint = (Vector3.Dot(pbOtherSite.euclidean, pbPositiveIntersection.euclidean) > 0) ? pb : pb.OppositePoint();
                                        }

                                        if (intermediatePoint.IsSame(positiveIntersectionPoint) || intermediatePoint.IsSame(positiveIntersectionPoint.OppositePoint()))
                                        {
                                            Debug.Log("order check: intermediate points is same as +I or +IO");
                                        }

                                        // TODO: figure out why +Ii and +IOi are opposite directions
                                        if (Vector3.Dot(pbNormal.euclidean, Vector3.Cross(positiveIntersectionPoint.euclidean, intermediatePoint.euclidean)) > 0)
                                        {
                                            AddEdge(positiveIntersectionPoint.ToVPoint(), intermediatePoint.ToVPoint(), site, newSite);
                                            AddEdge(intermediatePoint.ToVPoint(), positiveIntersectionPoint.OppositePoint().ToVPoint(), site, newSite);
                                        }
                                        else
                                        {
                                            AddEdge(positiveIntersectionPoint.OppositePoint().ToVPoint(), intermediatePoint.ToVPoint(), site, newSite);
                                            AddEdge(intermediatePoint.ToVPoint(), positiveIntersectionPoint.ToVPoint(), site, newSite);
                                        }
                                    }
                                }
                            }
                        }
                        else
                        {
                            if (intersections[0].IsOpposite(intersections[1]))
                            {
                                Debug.Log("new edge from intersections are opposites. this should never happen. it's in the other if statement");
                            }
                            if (!intersections[0].IsSame(intersections[1]))
                            {
                                if (Vector3.Dot(pbNormal.euclidean, Vector3.Cross(intersections[0].euclidean, intersections[1].euclidean)) > 0)
                                {
                                    AddEdge(intersections[0].ToVPoint(), intersections[1].ToVPoint(), site, newSite);
                                }
                                else
                                {
                                    AddEdge(intersections[1].ToVPoint(), intersections[0].ToVPoint(), site, newSite);
                                }
                            }
                            else
                            {
                                Debug.Log("new edge from intersections have same point: dot: " + (Mathf.Abs(Vector3.Dot(intersections[0].euclidean, intersections[1].euclidean) + 1)).ToString("0.####"));
                            }

                        }
                    }
                    else if (intersections.Count != 0)
                    {
                        // TODO: figure out why this is sometimes happening
                        Debug.Log("intersections.count != 0 or 2, which is no good. Count: " + intersections.Count);
                    }
                    else
                    {
                        //Debug.Log("intersections.count == 0. Count: " + intersections.Count);
                    }

                    //Debug.Log("done checking site " + siteCenter.ToString("0.####"));
                }

                //[14] Add newSite to sites.
                if (!sites.ContainsKey(newSite.center))
                {
                    sites.Add(newSite.center, newSite);
                }
                //Debug.Log("done checking point " + p.ToString("0.####"));
            }
            Debug.Log("num final sites: " + sites.Count);
            Debug.Log("num final edges: " + edges.Count);
        }

        void AddEdge(VPoint start, VPoint end, VSite a, VSite b)
        {
            var _s = new VSphericalPoint(start);
            var _e = new VSphericalPoint(end);
            if (_s.IsSame(_e))
            {
                Debug.Log("AddEdge: start & end are same: start: " + _s.ToString("0.############0") + " end: " + _e.ToString("0.############0"));
            }
            if (_s.IsOpposite(_e))
            {
                Debug.Log("AddEdge: start & end are opposite: start: " + _s.ToString("0.############0") + " end: " + _e.ToString("0.############0"));
            }
            Debug.Log("AddEdge: edge: start: " + _s.ToString("0.############0") + " end: " + _e.ToString("0.############0"));

            var newEdge = new VHalfEdge(start, end);
            if (VPoint.IsClockwise(newEdge.Start, newEdge.End, a.center))
            {
                newEdge.leftSite = b;
                newEdge.rightSite = a;
            }
            else
            {
                newEdge.leftSite = a;
                newEdge.rightSite = b;
            }

            //a.neighbors.Add(b);
            //b.neighbors.Add(a);

            a.edges.Add(newEdge);
            b.edges.Add(newEdge);
            edges.Add(newEdge);
        }

        void RemoveEdge(VHalfEdge e)
        {
            // TODO: remove neighbors
            //e.leftSite.neighbors.Add(e.rightSite);
            //e.rightSite.neighbors.Add(e.leftSite);

            e.leftSite.edges.Remove(e);
            e.rightSite.edges.Remove(e);
            edges.Remove(e);
        }

        public VoronoiGraph(List<VPoint> points, VSite s, bool includeCenter = true)
        {
            // from https://courses.cs.washington.edu/courses/cse326/00wi/projects/voronoi.html
            sites = new Dictionary<VPoint, VSite>();
            edges = new List<VHalfEdge>();

            VSite newS = new VSite(s);
            newS.neighbors.Clear();
            List<VHalfEdge> DoNotDelete = new List<VHalfEdge>();

            if (includeCenter)
            {
                //points.Add(s.center);
                sites.Add(newS.center, newS);
            }
            //points.Sort((p1, p2) => { return VPoint.CompareByXThenY(p1, p2); });

            foreach (var edge in newS.edges)
            {
                edges.Add(edge);
                DoNotDelete.Add(edge);
            }

            // add bounds "at infinity"
            /*foreach (var n in s.neighbors)
            {
                sites.Add(n.center, n);
                foreach (var e in n.edges)
                {
                    if (!edges.Contains(e))
                    {
                        edges.Add(e);
                    }
                }
            }*/
            Debug.Log("num starting edges: " + edges.Count);

            List<VPoint> intersections = new List<VPoint>();

            foreach (var p in points)
            {
                Debug.Log("checking point " + (Vector2)p);

                VSite newSite = new VSite(p);
                VHalfEdge newEdge;

                foreach (var site in sites.Values)
                {
                    Debug.Log("checking site " + (Vector2)site.center);

                    VPoint pb = (p + site.center) * 0.5f;
                    // slope is -1/m, where m is the slope of site.center to newSite.center
                    float pbSlope;
                    if (Mathf.Abs(site.y - p.y) < 1e-10)
                    {
                        pbSlope = float.PositiveInfinity;
                    }
                    else
                    {
                        pbSlope = -(site.x - p.x) / (site.y - p.y);
                    }
                    Debug.Log("pb: " + (Vector2)pb + " pb slope: " + pbSlope);

                    intersections.Clear();
                    newEdge = null;

                    for (int i = site.edges.Count - 1; i >= 0; i--)
                    {
                        var e = site.edges[i];
                        Debug.Log("checking edge " + (Vector2)e.Start + " to " + (Vector2)e.End);

                        VPoint intersection = CheckSpacialRelationship(e, pb, pbSlope, newSite, DoNotDelete);
                        //Debug.Log("e after function call: " + (Vector2)e.Start + " to " + (Vector2)e.End);

                        Debug.Log("numEdges: " + edges.Count);

                        if (intersection != null)
                        {
                            Debug.Log("There's an intersection at " + (Vector2)intersection);
                            intersections.Add(intersection);
                        }
                        // if no intersection, check both endpoints and if e is closer, then use p&m instead by deleting e
                        // e is closer than p&m, if there's an intersection between pb & p
                        else
                        {
                            Debug.Log("No intersection");

                            // if e.start & e.end are closer to p than site, then give e to p
                            if (e.Start.DistanceSquare(p) <= e.Start.DistanceSquare(site.center) &&
                                e.End.DistanceSquare(p) <= e.End.DistanceSquare(site.center))
                            {
                                Debug.Log("edge is closer");
                                if (DoNotDelete.Contains(e))
                                {
                                    Debug.Log("swaping from " + (Vector2)site.center + " to " + (Vector2)p);
                                    site.edges.Remove(e);
                                    newSite.edges.Add(e);
                                    if (e.leftSite == site)
                                    {
                                        e.leftSite = newSite;
                                    }
                                    else
                                    {
                                        e.rightSite = newSite;
                                    }
                                }
                                else
                                {
                                    // TODO: figure out what to remove
                                    Debug.Log("removing");
                                    site.edges.Remove(e);
                                    newSite.edges.Remove(e);
                                    edges.Remove(e);
                                }
                            }
                        }
                        Debug.Log("numEdges: " + edges.Count);
                        Debug.Log("done checking edge " + (Vector2)e.Start + " to " + (Vector2)e.End);
                    }

                    if (newEdge == null)
                    {
                        if (intersections.Count == 2)
                        {
                            Debug.Log("creating edge at: " + (Vector2)intersections[0] + " to " + (Vector2)intersections[1]);

                            // TODO: figure out which should be start and end, which is left and right
                            newEdge = new VHalfEdge(intersections[0], intersections[1]);
                            newEdge.leftSite = site;
                            newEdge.rightSite = newSite;

                            site.edges.Add(newEdge);
                            newSite.edges.Add(newEdge);
                            edges.Add(newEdge);
                        }
                    }
                    Debug.Log("numEdges: " + edges.Count);
                    Debug.Log("intersections: " + intersections.Count);
                    Debug.Log("done checking site " + (Vector2)site.center);
                }

                if (!sites.ContainsKey(p))
                {
                    sites.Add(newSite.center, newSite);
                }
                Debug.Log("done checking point " + (Vector2)p);
            }

            Debug.Log("Removing colinear lines on the DND list");
            foreach (var site in sites.Values)
            {
                for (int i = site.edges.Count - 1; i >= 1; i--)
                {
                    VHalfEdge edgeToKeep = site.edges[i];
                    if (DoNotDelete.Contains(edgeToKeep))
                    {
                        Debug.Log("i is on DND");
                        for (int j = i - 1; j >= 0; j--)
                        {
                            if (DoNotDelete.Contains(site.edges[j]))
                            {
                                Debug.Log("j is on DND");
                                VPoint a = null;
                                VPoint b = null;
                                VPoint c = null;
                                // if i and j are colinear, then combine
                                Debug.Log("i: " + i + " j: " + j + " site.edges.count: " + site.edges.Count);
                                if (Mathf.Abs(edgeToKeep.Start.DistanceSquare(site.edges[j].Start)) < 1e-10)
                                {
                                    a = edgeToKeep.Start;
                                    b = edgeToKeep.End;
                                    c = site.edges[j].End;
                                    if (a != null && b != null && c != null && VPoint.IsColinear(a, b, c))
                                    {
                                        edgeToKeep.Start = site.edges[j].End;
                                        site.edges[j] = edgeToKeep;
                                        site.edges.Remove(site.edges[j]);
                                        j--; i--;
                                        Debug.Log("Remove edge");
                                    }
                                }
                                else if (Mathf.Abs(edgeToKeep.End.DistanceSquare(site.edges[j].Start)) < 1e-10)
                                {
                                    a = edgeToKeep.Start;
                                    b = edgeToKeep.End;
                                    c = site.edges[j].End;
                                    if (a != null && b != null && c != null && VPoint.IsColinear(a, b, c))
                                    {
                                        edgeToKeep.End = site.edges[j].End;
                                        site.edges[j] = edgeToKeep;
                                        site.edges.Remove(site.edges[j]);
                                        j--; i--;
                                        Debug.Log("Remove edge");
                                    }
                                }
                                else if (Mathf.Abs(edgeToKeep.Start.DistanceSquare(site.edges[j].End)) < 1e-10)
                                {
                                    a = edgeToKeep.Start;
                                    b = edgeToKeep.End;
                                    c = site.edges[j].Start;
                                    if (a != null && b != null && c != null && VPoint.IsColinear(a, b, c))
                                    {
                                        edgeToKeep.Start = site.edges[j].Start;
                                        site.edges[j] = edgeToKeep;
                                        site.edges.Remove(site.edges[j]);
                                        j--; i--;
                                        Debug.Log("Remove edge");
                                    }
                                }
                                else if (Mathf.Abs(edgeToKeep.End.DistanceSquare(site.edges[j].End)) < 1e-10)
                                {
                                    a = edgeToKeep.Start;
                                    b = edgeToKeep.End;
                                    c = site.edges[j].Start;
                                    if (a != null && b != null && c != null && VPoint.IsColinear(a, b, c))
                                    {
                                        // if i.end && j.start match
                                        edgeToKeep.End = site.edges[j].Start;
                                        site.edges[j] = edgeToKeep;
                                        site.edges.Remove(site.edges[j]);
                                        j--; i--;
                                        Debug.Log("Remove edge");
                                    }
                                }
                            }
                        }
                    }
                }
            }
            Debug.Log("Done removing colinear lines on the DND list");

            // remove bounds "at infinity"
            /*foreach (var n in s.neighbors)
            {
                sites.Remove(n.center);
                foreach (var e in n.edges)
                {
                    edges.Remove(e);
                }
            }*/
            Debug.Log("num final Edges: " + edges.Count);
            foreach (var edge in edges)
            {
                Debug.Log("edge: " + (Vector2)edge.Start + " to " + (Vector2)edge.End);
            }
            foreach (var site in sites.Values)
            {
                Debug.Log("site: " + (Vector2)site.center + " has " + site.edges.Count + " edges");
                foreach (var edge in site.edges)
                {
                    Debug.Log("site: " + (Vector2)site.center + "edge: " + (Vector2)edge.Start + " to " + (Vector2)edge.End);
                }
            }
        }

        VSphericalPoint CheckSpacialRelationship(VHalfEdge e, Vector3 n)
        {
            var start = new VSphericalPoint(e.Start);
            var end = new VSphericalPoint(e.End);
            //[10] If e is on the near side of pb (closer to newsite than to site), mark it to be deleted (or delete it now provided that doing so will not disrupt your enumeration).
            //Debug.Log("check: e: start: " + ((Vector2)e.Start).ToString("0.####") + " VSP: " + start.ToString("0.####"));
            //Debug.Log("check: e: end: " + ((Vector2)e.End).ToString("0.####") + " VSP: " + end.ToString("0.####"));

            if (start.IsSame(end))
            {
                Debug.Log("check: start and end are the same: " + Mathf.Abs(Vector3.Dot(start.euclidean, end.euclidean) - 1).ToString("0.####") + " which should be caught elsewhere and not happen");
            }

            if (Mathf.Abs(Vector3.Dot(Vector3.Cross(start.euclidean, end.euclidean), n) - 1) < 1e-8)
            {
                Debug.Log("check: e.normal and pbNormal are pointing in the same direction");
            }
            if (Mathf.Abs(Vector3.Dot(Vector3.Cross(start.euclidean, end.euclidean), n) + 1) < 1e-8)
            {
                Debug.Log("check: e.normal and pbNormal are pointing in the opposite direction");
            }
            if (Mathf.Abs(Vector3.Dot(start.euclidean, end.euclidean) + 1) < 1e-6)
            {
                Debug.Log("check: start and end are pointing in the opposite direction: start: " + start.ToString("0.####") + " end: " + end.ToString("0.####") + " dot: " + (Mathf.Abs(Vector3.Dot(start.euclidean, end.euclidean) + 1)).ToString("0.####"));
            }
            VSphericalPoint intersection = VHalfArc.Intersect(new VHalfArc(start, end), n);

            //Debug.Log("check: n dot start: " + Vector3.Dot(n, start.euclidean) + " n dot end: " + Vector3.Dot(n, end.euclidean));
            // take n dot start && n dot end. if both are negative, then delete e
            if ((Mathf.Abs(Vector3.Dot(n, start.euclidean)) < 1e-10 || Mathf.Abs(Vector3.Dot(n, end.euclidean)) < 1e-10) && intersection == null)
            {
                Debug.Log("edge endpoints are 0 but there's no intersection. need to figure out why an intersection wasn't caught");
            }


            if (Vector3.Dot(n, start.euclidean) <= 0 && Vector3.Dot(n, end.euclidean) <= 0)
            {
                //Debug.Log("check: delete e");
                RemoveEdge(e);
            }
            // if there's an intersection, clip e to that point
            else if (intersection != null)
            {
                var interStart = start.euclidean - intersection.euclidean;
                var interEnd = end.euclidean - intersection.euclidean;
                //Debug.Log("check: e.normal: " + Vector3.Normalize(Vector3.Cross(start.euclidean, end.euclidean)).ToString("0.####"));
                //Debug.Log("check: pb.normal: " + n.ToString("0.####"));
                Debug.Log("check: intersection: " + intersection.ToString("0.####"));

                //[11] If e intersects pb, clip e to the far side of pb, and store the point of intersection in X.
                //Debug.Log("check: e before clip: " + e.Start.ToString("0.####") + " to " + e.End.ToString("0.####"));
                //Debug.Log("check: dot start: " + Vector3.Dot(n, interStart) + " dot end: " + Vector3.Dot(n, interEnd));

                // for visualization: https://www.jasondavies.com/maps/intersect/
                // if start is on far side of p
                if (Vector3.Dot(n, start.euclidean) > 0)
                {
                    if (!start.IsSame(intersection))
                    {
                        e.End = intersection.ToVPoint();
                    }
                    else
                    {
                        Debug.Log("check: edge would have had 0 length: " + Mathf.Abs(Vector3.Dot(start.euclidean, intersection.euclidean) - 1).ToString("0.############0"));
                        Debug.Log("check: start: " + start.ToString("0.####") + " end: " + end.ToString("0.####"));
                        Debug.Log("check: intersection: " + intersection.ToString("0.####"));
                    }
                }
                // if end is on far side of p
                else if (Vector3.Dot(n, end.euclidean) > 0)
                {
                    if (!end.IsSame(intersection))
                    {
                        e.Start = intersection.ToVPoint();
                    }
                    else
                    {
                        Debug.Log("check: edge would have had 0 length: " + Mathf.Abs(Vector3.Dot(end.euclidean, intersection.euclidean) - 1).ToString("0.############0"));
                        Debug.Log("check: start: " + start.ToString("0.####") + " end: " + end.ToString("0.####"));
                        Debug.Log("check: intersection: " + intersection.ToString("0.####"));
                    }
                }
                //Debug.Log("check: e is now clipped to: " + e.Start.ToString("0.####") + " to " + e.End.ToString("0.####"));
            }
            // check the other side of the circle pb
            else
            {
            }

            return intersection;
        }

        VPoint CheckSpacialRelationship(VHalfEdge e, VPoint pb, float m, VSite s, List<VHalfEdge> DND)
        {
            /*
[10] If e is on the near side of pb (closer to newsite than to site), mark it to be deleted (or delete it now provided that doing so will not disrupt your enumeration).
[11] If e intersects pb, clip e to the far side of pb, and store the point of intersection in X.
 */

            VPoint intersection = VHalfEdge.Intersect(e, pb, m);

            // if there's an intersection, clip e to that point
            if (intersection != null)
            {
                float startDistance = e.Start.DistanceSquare(s.center);
                float endDistance = e.End.DistanceSquare(s.center);
                VHalfEdge newEdge = null;

                // clip e
                Debug.Log("e before clip: " + (Vector2)e.Start + " to " + (Vector2)e.End);

                // if start is closer
                if (startDistance > endDistance)
                {
                    if (Mathf.Abs(e.End.DistanceSquare(intersection)) > 1e-10)
                    {
                        if (DND.Contains(e))
                        {
                            newEdge = new VHalfEdge(intersection, e.End);
                            newEdge.leftSite = s;
                            newEdge.rightSite = e.rightSite;
                            s.edges.Add(newEdge);
                            edges.Add(newEdge);
                            DND.Add(newEdge);
                            Debug.Log("newEdge is now : " + (Vector2)newEdge.Start + " to " + (Vector2)newEdge.End);
                        }
                        else
                        {
                            // sow back together
                            // by removing point e.End
                            VSite vs = (e.leftSite == s) ? e.rightSite : e.leftSite;
                            Debug.Log("Need to sow 1line together: at " + (Vector2)e.End);
                            foreach (var edge in s.edges)
                            {
                                if (Mathf.Abs(edge.Start.DistanceSquare(e.End)) < 1e-10 || Mathf.Abs(edge.End.DistanceSquare(e.End)) < 1e-10)
                                {
                                    Debug.Log("edge to sow: " + (Vector2)edge.Start + " to " + (Vector2)edge.End);
                                }
                            }
                        }

                        e.End = intersection;
                    }
                }
                // if end is closer
                else
                {
                    if (Mathf.Abs(e.Start.DistanceSquare(intersection)) > 1e-10)
                    {
                        if (DND.Contains(e))
                        {
                            newEdge = new VHalfEdge(e.Start, intersection);
                            newEdge.leftSite = s;
                            newEdge.rightSite = e.rightSite;
                            s.edges.Add(newEdge);
                            edges.Add(newEdge);
                            DND.Add(newEdge);
                            Debug.Log("newEdge is now : " + (Vector2)newEdge.Start + " to " + (Vector2)newEdge.End);
                        }
                        else
                        {
                            // sow back together
                            // by removing point e.End
                            VSite vs = (e.leftSite == s) ? e.rightSite : e.leftSite;
                            Debug.Log("Need to sow 2line together: " + (Vector2)e.Start + (Vector2)e.End + (Vector2)s.center);
                        }


                        e.Start = intersection;
                    }
                }
                Debug.Log("e is now clipped to: " + (Vector2)e.Start + " to " + (Vector2)e.End);
            }

            return intersection;
        }

        /*public void CreateGraph(int numPoints, int relaxation, bool wrapX = false)
        {
            gridSize = _size;

            System.Random prng = new System.Random(0);
            centers = new List<VPoint>();
            centersInGridLookup = new List<VPoint>[Mathf.FloorToInt(((wrapX) ? 1f + 2f * ghostPercent : 1f) * 2 * gridSize / gridLookupSize), Mathf.FloorToInt(gridSize / gridLookupSize)];
            for (int i = 0; i < numPoints; i++)
            {
                var x = prng.Next(0, 2 * gridSize);
                var y = prng.Next(0, gridSize);

                AddCenter(new Vector2(x, y), wrapX);
            }
            //Debug.Log("wrapX: " + wrapX + ", point count: " + centers.Count + ", gridSize: " + gridSize);

            voronoiGraph = new Voronoi(centers, new Rectf((wrapX ? -ghostPercent : 0) * gridSize, 0, (wrapX ? 1f + 2f * ghostPercent : 1f) * 2 * gridSize, gridSize));
            sites = voronoiGraph.SitesIndexedByLocation;
            edges = voronoiGraph.Edges;

            for (int y = 0; y < centersInGridLookup.GetLength(1); y++)
            {
                for (int x = 0; x < centersInGridLookup.GetLength(0); x++)
                {
                    if (centersInGridLookup[x, y] != null)
                    {
                        Debug.Log("[" + x + "," + y + "] -> " + centersInGridLookup[x, y].Count);
                    }
                    else
                    {
                        Debug.Log("[" + x + "," + y + "] -> empty");
                    }
                }
            }

            // remove ghost points
            foreach (var site in sites.Values)
            {
                if (wrapX)
                {
                    if (site.Coord.x < 0)
                    {
                        Site parent = sites[new VPoint(site.Coord.x + gridSize, site.Coord.y)];
                        Debug.Log(site.Coord.x + " && " + parent.Coord);

                        foreach (var n in site.Edges)
                        {
                            Debug.Log(site.Coord.x + " -> " + n.LeftVertex + " to " + n.RightVertex);
                        }
                        foreach (var n in parent.Edges)
                        {
                            Debug.Log(site.Coord.x + " ->> " + n.LeftVertex + " to " + n.RightVertex);
                        }
                    }
                    if (site.Coord.x > gridSize)
                    {
                        Debug.Log(site.Coord.x + " && " + sites[new VPoint(site.Coord.x - gridSize, site.Coord.y)].Coord);
                    }
                }
            }

            voronoiGraph.LloydRelaxation(relaxation);
        }*/

        public Dictionary<VPoint, VSite> Sites() { return sites; }
        public List<VHalfEdge> Edges() { return edges; }

        /*private void AddCenter(Vector2 point)
        {
            var gridX = Mathf.FloorToInt(point.x / gridLookupSize);
            var gridY = Mathf.FloorToInt(point.y / gridLookupSize);

            // add point
            centers.Add((VPoint)point);

            // add point to grid lookup
            if (centersInGridLookup[gridX, gridY] == null)
            {
                centersInGridLookup[gridX, gridY] = new List<VPoint>();
            }
            centersInGridLookup[gridX, gridY].Add((VPoint)point);
        }*/

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

        /*public Mesh CreateMesh(VoronoiMeshMode meshMode)
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
                }*//*
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
        }*/
    }
}
