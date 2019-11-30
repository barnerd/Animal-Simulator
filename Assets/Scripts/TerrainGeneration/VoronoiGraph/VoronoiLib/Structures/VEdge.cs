using UnityEngine;

namespace VoronoiLib.Structures
{
    public class VEdge
    {
        public VPoint Start { get; internal set; }
        public VPoint End { get; internal set; }
        public FortuneSite Left { get; set; }
        public FortuneSite Right { get; set; }
        public VPoint Midpoint { get; } // Midpoint between Left/Right, which is on edge
        internal double SlopeRise { get; }
        internal double SlopeRun { get; }
        internal double? Slope { get; }
        internal double? Intercept { get; }

        public VEdge Neighbor { get; internal set; }

        internal VEdge(VPoint start, FortuneSite left, FortuneSite right)
        {
            if(left == null || right == null) { Debug.Log("left: " + left + " , right: " + right + " is null"); }
            Start = start;
            Left = left;
            Left.AddEdge(this);
            Right = right;
            Right.AddEdge(this);

            //for bounding box edges
            if (left == null || right == null)
                return;

            Midpoint = new VPoint(
                (Left.Center.X + Right.Center.X) / 2 - start.X,
                (Left.Center.Y + Right.Center.Y) / 2 - start.Y);

            //from negative reciprocal of slope of line from left to right
            //ala m = (left.y -right.y / left.x - right.x)
            SlopeRise = left.Center.X - right.Center.X;
            SlopeRun = -(left.Center.Y - right.Center.Y);
            Intercept = null;

            //if (SlopeRise.ApproxEqual(0) || SlopeRun.ApproxEqual(0)) return;
            if (SlopeRun.ApproxEqual(0)) return; // slope can be 0
            Slope = SlopeRise / SlopeRun;
            Intercept = start.Y - Slope * start.X;
        }

        public bool Contains(VPoint _point) { return _point == Start || _point == End; }
        public bool Contains(FortuneSite _node) { return _node == Left || _node == Right; }
    }
}
