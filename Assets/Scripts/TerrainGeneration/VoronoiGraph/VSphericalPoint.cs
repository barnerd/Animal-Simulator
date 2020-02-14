using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Delaunay
{
    public class VSphericalPoint : VPoint
    {
        public Vector3 euclidean;
        public new float x { get => euclidean.x; }
        public new float y { get => euclidean.y; }
        public float z { get => euclidean.z; }

        public Vector3 spherical;
        public float r { get => spherical.x; }
        public float phi { get => spherical.y; }
        public float theta { get => spherical.z; }

        public VSphericalPoint(float _phi = 0, float _theta = 0)
        {
            euclidean = VSphericalPoint.SphericalToEuclidean(1, _phi, _theta);
            Normalize();
        }

        public VSphericalPoint(VPoint p)
        {
            euclidean = VSphericalPoint.SphericalToEuclidean(1, p.x, p.y);
            Normalize();
        }

        public VSphericalPoint(Vector3 euclid)
        {
            euclidean = euclid;
            Normalize();
        }

        public new float magnitude
        {
            get
            {
                return (float)Mathf.Sqrt(x * x + y * y + z * z);
            }
        }

        public new void Normalize()
        {
            euclidean /= magnitude;
            spherical = EuclideanToSpherical(euclidean);
        }

        public new string ToString(string format = "")
        {
            return string.Format("Spherical: (" + r.ToString(format) + ", " + phi.ToString(format) + ", " + theta.ToString(format) + ")" + " Euclidean: (" + x.ToString(format) + ", " + y.ToString(format) + ", " + z.ToString(format) + ")");
        }

        public VPoint ToVPoint()
        {
            return new VPoint(phi, theta);
        }

        public VSphericalPoint OppositePoint()
        {
            return new VSphericalPoint((phi < 0) ? phi + 180f : phi - 180f, -theta);
        }

        public bool IsOpposite(VSphericalPoint a)
        {
            return Mathf.Abs(Vector3.Dot(euclidean, a.euclidean) + 1) < 1e-10;
        }

        public bool IsOrthogonal(VSphericalPoint a)
        {
            return Mathf.Abs(Vector3.Dot(euclidean, a.euclidean)) < 1e-10;
        }

        public bool IsSame(VSphericalPoint a)
        {
            return Mathf.Abs(Vector3.Dot(euclidean, a.euclidean) - 1) < 1e-10;
        }

        public static Vector3 SphericalToEuclidean(Vector3 point)
        {
            // Spherical is (r, phi, theta)

            return SphericalToEuclidean(point.x, point.y, point.z);
        }

        public static Vector3 SphericalToEuclidean(float radius, float phi, float theta)
        {
            float _x = radius * Mathf.Cos(theta * Mathf.Deg2Rad) * Mathf.Cos(phi * Mathf.Deg2Rad);
            float _y = radius * Mathf.Sin(theta * Mathf.Deg2Rad);
            float _z = radius * Mathf.Cos(theta * Mathf.Deg2Rad) * Mathf.Sin(phi * Mathf.Deg2Rad);

            return new Vector3(_x, _y, _z);
        }

        public static Vector3 EuclideanToSpherical(Vector3 point)
        {
            float r = Mathf.Sqrt(point.x * point.x + point.y * point.y + point.z * point.z);
            float theta = Mathf.Atan2(point.y, Mathf.Sqrt(point.x * point.x + point.z * point.z)) * Mathf.Rad2Deg;
            float phi = Mathf.Atan2(point.z, point.x) * Mathf.Rad2Deg;

            return new Vector3(r, phi, theta);
        }
    }
}
