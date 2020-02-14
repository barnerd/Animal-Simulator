using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Delaunay
{
    public class VHalfEdge
    {
        private VPoint start;
        public VPoint Start { get => start; set { start = value; SetABC(); } }
        private VPoint end;
        public VPoint End { get => end; set { end = value; SetABC(); } }

        public VSite leftSite;
        public VSite rightSite;

        // used in the line equation, ax + by = c
        public float a;
        public float b;
        public float c;

        public VHalfEdge(VPoint _start, VPoint _end)
        {
            start = _start;
            end = _end;

            SetABC();
        }

        public VHalfEdge(VHalfEdge copy)
        {
            start = new VPoint(copy.start);
            end = new VPoint(copy.end);

            SetABC();
        }

        void SetABC()
        {
            if (start != null && end != null)
            {
                // from https://math.stackexchange.com/questions/422602/convert-two-points-to-line-eq-ax-by-c-0
                // (y1 – y2)x + (x2 – x1)y + (x1y2 – x2y1) = 0
                a = start.y - end.y;
                b = end.x - start.x;
                // c = start.x * end.y - end.x * start.y
                // or c = a * start.x + b * start.y
                c = a * start.x + b * start.y;

                /*if(Mathf.Abs(a) < 1e-10)
                {
                    if(b < 0)
                    {
                        b *= -1;
                        c *= -1;
                    }
                }
                else if(a < 0)
                {
                    a *= -1;
                    b *= -1;
                    c *= -1;
                }*/
            }
            // (0,256) -> (256,256)
            // a= 0
            // b=256
            // c= -256 * 256
        }

        public bool IsVertical()
        {
            return Mathf.Abs(b) < 1e-10;
        }

        public bool IsHorizontal()
        {
            return Mathf.Abs(a) < 1e-10;
        }

        public VPoint ToVector()
        {
            return VPoint.ToVector(start, end);
        }

        public static VPoint Intersect(VHalfEdge halfedge0, VHalfEdge halfedge1)
        {
            if (halfedge0 != null && halfedge1 != null)
            {
                if (VPoint.IsClockwise(halfedge0.start, halfedge0.end, halfedge1.start) != VPoint.IsClockwise(halfedge0.start, halfedge0.end, halfedge1.end))
                {
                    if (VPoint.IsClockwise(halfedge1.start, halfedge1.end, halfedge0.start) != VPoint.IsClockwise(halfedge1.start, halfedge1.end, halfedge0.end))
                    {
                        //Debug.Log("Intersect: There should be an intersection, because orientations aren't the same");
                    }
                }

                //Debug.Log("Intersect: halfedge0: " + halfedge0.Start + " -> " + halfedge0.End);
                //Debug.Log("Intersect: halfedge0: a: " + halfedge0.a + " b " + halfedge0.b + " c: " + halfedge0.c);
                //Debug.Log("Intersect: halfedge1: " + halfedge1.Start + " -> " + halfedge1.End);
                //Debug.Log("Intersect: halfedge1: a: " + halfedge1.a + " b: " + halfedge1.b + " c: " + halfedge1.c);

                // from http://www.cs.swan.ac.uk/~cssimon/line_intersection.html
                // determinant is (x4−x3)(y1−y2)−(x1−x2)(y4−y3)
                var determinant = halfedge0.a * halfedge1.b - halfedge0.b * halfedge1.a;
                // make sure lines are not parallel
                if (Mathf.Abs(determinant) > 1E-10)
                {
                    // solving matrix equations
                    // halfedge1.a * (x1−x3)+halfedge1.b(y1−y3)
                    var ta = halfedge1.a * (halfedge0.Start.x - halfedge1.Start.x) + halfedge1.b * (halfedge0.Start.y - halfedge1.Start.y);
                    ta /= determinant;
                    // halfedge0.a * (x1−x3)+halfedge0.b(y1−y3)
                    var tb = halfedge0.a * (halfedge0.Start.x - halfedge1.Start.x) + halfedge0.b * (halfedge0.Start.y - halfedge1.Start.y);
                    tb /= determinant;

                    //Debug.Log("Intersect: ta: " + ta + " tb: " + tb);
                    //Debug.Log("Intersect: ta intersection at: " + (halfedge0.Start + ta * (halfedge0.End - halfedge0.Start)));
                    //Debug.Log("Intersect: -ta intersection at: " + (halfedge0.Start + -ta * (halfedge0.End - halfedge0.Start)));
                    //Debug.Log("Intersect: tb intersection at: " + (halfedge1.Start + tb * (halfedge1.End - halfedge1.Start)));
                    //Debug.Log("Intersect: -tb intersection at: " + (halfedge1.Start + -tb * (halfedge1.End - halfedge1.Start)));
                    // make sure the intersection is on the segment
                    if (ta >= 0 && ta <= 1 && tb >= 0 && tb <= 1)
                    {
                        return halfedge0.Start + ta * (halfedge0.End - halfedge0.Start);
                    }
                }
                else
                {
                    Debug.Log("Intersect: determinant: " + determinant);
                    Debug.Log("Intersect: lines are parallel");
                }
            }
            else
            {
                Debug.Log("Intersect: intersection is null because one of the segments is null");
            }

            // line(s) are null or they are parallel
            return null;
        }

        public static VPoint Intersect(VHalfEdge halfedge, VPoint p, float m)
        {
            // only need a, b, and Start on 2nd halfedge, which is the full line, to calculate intersection
            // p is Start
            // a = m
            // b = -1
            // c = m * p.x - p.y

            float a, b;

            // vertical line
            if (float.IsPositiveInfinity(m) || float.IsNegativeInfinity(m))
            {
                // divide a by m to "move" it to c, making c = p.x
                a = 1f;
                b = 0f;
            }
            else
            {
                a = m;
                b = -1f;
            }

            if (halfedge != null)
            {
                // from http://www.cs.swan.ac.uk/~cssimon/line_intersection.html
                // determinant is (x4−x3)(y1−y2)−(x1−x2)(y4−y3)
                var determinant = halfedge.a * b - halfedge.b * a;
                // make sure lines are not parallel
                if (Mathf.Abs(determinant) > 1E-10)
                {
                    var ta = a * (halfedge.Start.x - p.x) + b * (halfedge.Start.y - p.y);
                    ta /= determinant;

                    // make sure the intersection is on the segment
                    if (ta >= 0 && ta <= 1)
                    {
                        return halfedge.Start + ta * (halfedge.End - halfedge.Start);
                    }
                }
            }

            // line(s) are null or they are parallel
            return null;
        }
    }
}
