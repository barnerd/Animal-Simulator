using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Delaunay
{
    public class VHalfArc
    {
        private VSphericalPoint start;
        public VSphericalPoint Start { get => start; set { start = value; SetABC(); } }
        private VSphericalPoint end;
        public VSphericalPoint End { get => end; set { end = value; SetABC(); } }

        public VSite leftSite;
        public VSite rightSite;

        // used in the plane equation, ax + by + cz = 0, d=0 because plane goes through center at 0
        public float a;
        public float b;
        public float c;

        public VHalfArc(VSphericalPoint _start, VSphericalPoint _end)
        {
            start = _start;
            end = _end;

            SetABC();
        }

        public VHalfArc(VHalfArc copy)
        {
            start = new VSphericalPoint(copy.start);
            end = new VSphericalPoint(copy.end);

            SetABC();
        }

        void SetABC()
        {
            if (start != null && end != null)
            {
                // from https://www.maplesoft.com/support/help/Maple/view.aspx?path=MathApps%2FEquationofaPlane3Points
                // Given two points(d, e, f) and(g, h, i)
                // plane = x(ei − fh) − y(di − fg) + z(dh − eg)
                var cross = Vector3.Normalize(Vector3.Cross(start.euclidean, end.euclidean));

                a = cross.x;
                b = cross.y;
                c = cross.z;

                if(Mathf.Abs(cross.magnitude) < 1e-5)
                {
                    Debug.Log("abc: cross is zero, which means start and end are in the same direciton or opposite directions. no good: start: " + start.ToString("0.####") + " end: " + end.ToString("0.####") + " cross: " + cross.ToString("0.##########"));
                }
            }
        }

        public Vector3 Normal()
        {
            return new Vector3(a, b, c);
        }

        public VPoint ToVector()
        {
            return VPoint.ToVector(start, end);
        }

        public static VSphericalPoint Intersect(VHalfArc halfedge0, Vector3 normal1)
        {
            if (Mathf.Abs(normal1.magnitude) < 1e-5)
            {
                Debug.Log("Intersect: normal1 is zero: " + normal1.ToString("0.####") + " this shouldn't happen, but currently returning null");
            }
            if (halfedge0.Start.IsOpposite(halfedge0.End))
            {
                Debug.Log("Intersect: start: " + halfedge0.Start.ToString("0.####") + " and end: " + halfedge0.End.ToString("0.####") + " are opposite. this shouldn't happen; always using midpoints for edges");
            }
            if (halfedge0.Start.IsSame(halfedge0.End))
            {
                Debug.Log("Intersect: start: " + halfedge0.Start.ToString("0.####") + " and end: " + halfedge0.End.ToString("0.####") + " are same. currentlty causing t to be 0");
            }

            if (halfedge0 != null && normal1 != Vector3.zero)
            {
                // from: http://enrico.spinielli.net/2014/10/19/understanding-great-circle-arcs_57/
                // get cross products of both arcs/planes
                //Debug.Log("Intersect: halfedge: " + (Vector2)halfedge0.Start + " to " + (Vector2)halfedge0.End);
                var p = halfedge0.Normal();
                var q = normal1;
                //Debug.Log("Intersect: p: " + p + " q: " + q);

                // get intersect vector
                var t = Vector3.Cross(p, q);
                //Debug.Log("Intersect: t: " + t);
                if (Mathf.Abs(t.magnitude) < 1e-8)
                {
                    Debug.Log("Intersect: t is zero, which means edge and pbNormal are in the same direciton or opposite directions. no good: p: " + p.ToString("0.####") + " q: " + q.ToString("0.####") + "t: " + t.ToString("0.############0"));
                }

                t = t.normalized;

                // project arc endpoints onto intersection vector
                var s1 = Vector3.Dot(Vector3.Cross(halfedge0.Start.euclidean, p), t);
                var s2 = Vector3.Dot(Vector3.Cross(halfedge0.End.euclidean, p), t);

                /* if (Mathf.Abs(s1) < 1e-5 && Mathf.Abs(s2) < 1e-5) Debug.Log("s1 is 0, s2 is 0: s1: " + s1 + " s2: " + s2);
                if (Mathf.Abs(s1) < 1e-5 && Mathf.Abs(s2 - 1) < 1e-5) Debug.Log("s1 is 0, s2 is 1: s1: " + s1 + " s2: " + s2);
                if (Mathf.Abs(s1 - 1) < 1e-5 && Mathf.Abs(s2) < 1e-5) Debug.Log("s1 is 1, s2 is 0: s1: " + s1 + " s2: " + s2);
                if (Mathf.Abs(s1) < 1e-5 && Mathf.Abs(s2 + 1) < 1e-5) Debug.Log("s1 is 0, s2 is -1: s1: " + s1 + " s2: " + s2);
                if (Mathf.Abs(s1 + 1) < 1e-5 && Mathf.Abs(s2) < 1e-5) Debug.Log("s1 is -1, s2 is 0: s1: " + s1 + " s2: " + s2);*/

                if (Mathf.Abs(t.magnitude) < 1e-8)
                {
                    Debug.Log("Intersect: t is 0: s1: " + s1 + " s2: " + s2 + " t: " + t);
                }
                if (Mathf.Abs(s1) < 1e-5 && Mathf.Abs(s2) < 1e-5 && Mathf.Abs(t.magnitude) > 1e-5)
                {
                    Debug.Log("Intersect: both s1 & s2 are 0: s1: " + s1 + " s2: " + s2 + " t: " + t + " this shouldn't happen, unless start/end are same");
                }

                // there exists two intersection points:
                if (-s1 > -1e-5 && s2 > -1e-5)
                {
                    //Debug.Log("Intersect: positive: s1: " + s1 + " s2: " + s2);
                    return new VSphericalPoint(t);
                }
                else if (-s1 < 1e-5 && s2 < 1e-5)
                {
                    //Debug.Log("Intersect: negative: s1: " + s1 + " s2: " + s2);
                    return new VSphericalPoint(-t);
                }
                else
                {
                    //Debug.Log("Intersect: none: s1: " + s1 + " s2: " + s2);
                    //Debug.Log("Intersect: no intersection");
                }
            }
            else
            {
                Debug.Log("Intersect: intersection is null because one of the segments is null");
            }

            // line(s) are null or they are parallel
            return null;
        }
    }
}
