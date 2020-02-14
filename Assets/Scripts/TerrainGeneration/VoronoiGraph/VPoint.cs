using System;
using UnityEngine;

namespace Delaunay
{
    // Recreation of the UnityEngine.Vector2, so it can be used in other thread
    public class VPoint
    {
        public float x, y;

        public static readonly VPoint zero = new VPoint(0, 0);
        public static readonly VPoint one = new VPoint(1, 1);

        public static readonly VPoint right = new VPoint(1, 0);
        public static readonly VPoint left = new VPoint(-1, 0);

        public static readonly VPoint up = new VPoint(0, 1);
        public static readonly VPoint down = new VPoint(0, -1);

        public VPoint(float x = 0, float y = 0)
        {
            this.x = x;
            this.y = y;
        }
        public VPoint(double x, double y)
        {
            this.x = (float)x;
            this.y = (float)y;
        }

        public VPoint(VPoint p)
        {
            this.x = p.x;
            this.y = p.y;
        }

        public float magnitude
        {
            get
            {
                return (float)Math.Sqrt(x * x + y * y);
            }
        }

        public void Normalize()
        {
            x /= magnitude;
            y /= magnitude;
        }

        public static VPoint Normalize(VPoint a)
        {
            float magnitude = a.magnitude;
            return new VPoint(a.x / magnitude, a.y / magnitude);
        }

        public override bool Equals(object obj)
        {
            if (!(obj is VPoint))
            {
                return false;
            }
            VPoint v = (VPoint)obj;
            return x == v.x &&
                y == v.y;
        }

        public string ToString(string format = "")
        {
            return string.Format("[Vector2f]" + x.ToString(format) + "," + y.ToString(format));
        }

        public override int GetHashCode()
        {
            return x.GetHashCode() ^ y.GetHashCode() << 2;
        }

        public float DistanceSquare(VPoint v)
        {
            return VPoint.DistanceSquare(this, v);
        }

        public static float DistanceSquare(VPoint a, VPoint b)
        {
            float dx = b.x - a.x;
            float dy = b.y - a.y;
            return dx * dx + dy * dy;
        }

        public float Dot(VPoint a)
        {
            return Dot(this, a);
        }

        public static float Dot(VPoint a, VPoint b)
        {
            return a.x * b.x + a.y * b.y;
        }

        /*public float Cross(VPoint a)
        {
            return Cross(this, a);
        }

        public static float Cross(VPoint a, VPoint b)
        {
            return a.x * b.y - a.y * b.x;
        }*/

        public static bool operator ==(VPoint a, VPoint b)
        {
            if (object.ReferenceEquals(a, null))
            {
                return object.ReferenceEquals(b, null);
            }

            return a.Equals(b);
        }

        public static bool operator !=(VPoint a, VPoint b)
        {
            return !(a == b);
        }

        public static VPoint operator -(VPoint a, VPoint b) { return new VPoint(a.x - b.x, a.y - b.y); }
        public static VPoint operator +(VPoint a, VPoint b) { return new VPoint(a.x + b.x, a.y + b.y); }
        public static VPoint operator *(VPoint a, int i) { return new VPoint(a.x * i, a.y * i); }
        public static VPoint operator *(int i, VPoint a) { return a * i; }
        public static VPoint operator *(VPoint a, float f) { return new VPoint(a.x * f, a.y * f); }
        public static VPoint operator *(float f, VPoint a) { return a * f; }

        public static VPoint Min(VPoint a, VPoint b)
        {
            return new VPoint(Math.Min(a.x, b.x), Math.Min(a.y, b.y));
        }
        public static VPoint Max(VPoint a, VPoint b)
        {
            return new VPoint(Math.Max(a.x, b.x), Math.Max(a.y, b.y));
        }

        public static float Orientation(VPoint a, VPoint b, VPoint c)
        {
            // clockwise is positive
            // counterclockwise is negative
            // colinear is 0
            return (b.y - a.y) * (c.x - b.x) - (b.x - a.x) * (c.y - b.y);
        }

        public static bool IsClockwise(VPoint a, VPoint b, VPoint c) { return Orientation(a, b, c) > 0; }
        public static bool IsCClockwise(VPoint a, VPoint b, VPoint c) { return Orientation(a, b, c) < 0; }
        public static bool IsColinear(VPoint a, VPoint b, VPoint c) { return Mathf.Abs(Orientation(a, b, c)) < 1e-10; }

        public static VPoint ToVector(VPoint a, VPoint b)
        {
            return b - a;
        }

        public static int CompareByXThenY(VPoint p1, VPoint p2)
        {
            if (p1.x < p2.x) return -1;
            if (p1.x > p2.x) return 1;
            if (p1.y < p2.y) return -1;
            if (p1.y > p2.y) return 1;
            return 0;
        }

        public static implicit operator Vector2(VPoint p) => new Vector2((float)p.x, (float)p.y);
        public static explicit operator VPoint(Vector2 p) => new VPoint(p.x, p.y);
    }
}
