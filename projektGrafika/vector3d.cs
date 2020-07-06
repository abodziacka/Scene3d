using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace projektGrafika
{
    public class Vector3D
    {
        public float x, y, z;
        public float w = 1;

        public static float VectorLength(Vector3D v)
        {
            return (float)Math.Sqrt(v.x * v.x + v.y * v.y + v.z * v.z);
        }

        public static Vector3D VectorNormalise(Vector3D v)
        {
            float length = VectorLength(v);
            Vector3D normal = new Vector3D
            {
                x = v.x / length,
                y = v.y / length,
                z = v.z / length
            };
            return normal;
        }

        public static float VectorDot(Vector3D v1, Vector3D v2)
        {
            float dot = v1.x * v2.x + v1.y * v2.y + v1.z * v2.z;
            return dot;
        }

        public static Vector3D VectorCrossProduct(Vector3D v1, Vector3D v2)
        {
            Vector3D crossed = new Vector3D();

            crossed.x = v1.y * v2.z - v1.z * v2.y;
            crossed.y = v1.z * v2.x - v1.x * v2.z;
            crossed.z = v1.x * v2.y - v1.y * v2.x;

            return crossed;
        }

        public static Vector3D AddVectors(Vector3D v1, Vector3D v2)
        {
            Vector3D added = new Vector3D
            {
                x = v1.x + v2.x,
                y = v1.y + v2.y,
                z = v1.z + v2.z

            };
            return added;
        }

        public static Vector3D SubVectors(Vector3D v1, Vector3D v2)
        {
            Vector3D sub = new Vector3D
            {
                x = v1.x - v2.x,
                y = v1.y - v2.y,
                z = v1.z - v2.z

            };
            return sub;
        }

        public static  Vector3D MulNumber(Vector3D v, float a)
        {
            Vector3D mul = new Vector3D
            {
                x = v.x * a,
                y = v.y * a,
                z = v.z * a

            };
            return mul;
        }

        public static Vector3D DivNumber(Vector3D v, float a)
        {
            Vector3D div = new Vector3D
            {
                x = v.x / a,
                y = v.y / a,
                z = v.z / a

            };
            return div;
        }
    }
}
