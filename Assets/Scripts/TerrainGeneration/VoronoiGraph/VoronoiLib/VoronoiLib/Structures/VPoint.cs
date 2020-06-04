using UnityEngine;

namespace VoronoiLib.Structures
{
    public class VPoint
    {
        public double X { get; }
        public double Y { get; }

        internal VPoint(double x, double y)
        {
            X = x;
            Y = y;
        }

        public static implicit operator Vector2(VPoint p) => new Vector2((float)p.X, (float)p.Y);
        public static explicit operator VPoint(Vector2 p) => new VPoint(p.x, p.y);
    }
}
