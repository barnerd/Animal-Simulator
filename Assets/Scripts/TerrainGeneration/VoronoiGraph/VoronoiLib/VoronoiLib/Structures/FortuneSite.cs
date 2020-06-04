using System.Collections.Generic;

namespace VoronoiLib.Structures
{
    public class FortuneSite
    {
        public double X { get; }
        public double Y { get; }

        public VPoint Site { get; }

        public List<VEdge> Cell { get; private set; }

        public List<FortuneSite> Neighbors { get; private set; }

        public VoronoiGraph ChildGraph { get; private set; }

        public FortuneSite(double x, double y)
        {
            X = x;
            Y = y;
            Site = new VPoint(x, y);

            Cell = new List<VEdge>();

            Neighbors = new List<FortuneSite>();

            ChildGraph = null;
        }

        public void GenerateChildGraph()
        {
            System.Random prng = new System.Random(1);
            List<VPoint> points = new List<VPoint>();

            for (int i = 0; i < 64; i++)
            {
                //points.Add(new VSphericalPoint(Random.Range(-179.99f, 180f), Random.Range(-89.99f, 89.99f)));
                points.Add(new VPoint(prng.NextDouble() * 800f - 400f, prng.NextDouble() * 800f - 400f));
            }

            ChildGraph = new VoronoiGraph(points);
        }
    }
}
