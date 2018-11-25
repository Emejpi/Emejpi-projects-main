using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RaytracerWF
{
    public class Vector3
    {
        public float x, y, z;
        public Vector3()
        {
            x = 0;
            y = 0;
            z = 0;
        }
        public Vector3(float x, float y, float z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }

        public Vector3 Normalize()
        {
            float magnitude = Magnitude();
            return this / magnitude;
        }

        public float Magnitude()
        {
            return (float)Math.Sqrt(Math.Pow(x, 2) + Math.Pow(y, 2) + Math.Pow(z, 2));
        }

        public static Vector3 operator +(Vector3 v1, Vector3 v2)
        {
            return new Vector3(v1.x + v2.x, v1.y + v2.y, v1.z + v2.z);
        }

        public static Vector3 operator -(Vector3 v1, Vector3 v2)
        {
            return new Vector3(v1.x - v2.x, v1.y - v2.y, v1.z - v2.z);
        }

        public static Vector3 operator *(Vector3 v1, Vector3 v2)
        {
            return new Vector3(v1.x * v2.x, v1.y * v2.y, v1.z * v2.z);
        }

        public static Vector3 operator *(Vector3 vec, float v)
        {
            return new Vector3(vec.x * v, vec.y * v, vec.z * v);
        }

        public static Vector3 operator /(Vector3 vec, float v)
        {
            return new Vector3(vec.x / v, vec.y / v, vec.z / v);
        }

        public float Lenght()
        {
            return x + y + z;
        }

        public static float Dot(Vector3 v1, Vector3 v2)
        {
            return v1.x * v2.x + v1.y * v2.y + v1.z * v2.z;
        }

        public static Vector3 Projection(Vector3 vecTo, Vector3 vecOn)
        {
            return vecOn * ((Vector3.Dot(vecOn, vecTo)) / (Vector3.Dot(vecOn, vecOn)));
        }

        public static Vector3 Cross(Vector3 v1, Vector3 v2)
        {
            return new Vector3(
                v1.y * v2.z - v1.z * v2.y, 
                v1.z * v2.x - v1.x * v2.z,
                v1.x * v2.y - v1.y * v2.x);
        }

        public static float Distance(Vector3 vec1, Vector3 vec2)
        {
            return (vec1 - vec2).Magnitude();
        }

        public MyColor ToMyColor()
        {
            return new MyColor(
                Math.Min((int)(x * 255), 255),
                Math.Min((int)(y * 255), 255),
                Math.Min((int)(z * 255), 255));
        }

        public Matrix ToMatrix(int w)
        {
            Matrix mat = new Matrix(4, 1);
            mat.grid[0, 0] = x;
            mat.grid[1, 0] = y;
            mat.grid[2, 0] = z;
            mat.grid[3, 0] = w;

            return mat;
        }

    }

    public class Matrix3x3
    {
        float[] x;
        float[] y;
        float[] z;

        public Matrix3x3(float x0, float x1, float x2, float y0, float y1, float y2, float z0, float z1, float z2)
        {
            x = new float[3];
            x[0] = x0;
            x[1] = x1;
            x[2] = x2;

            y = new float[3];
            y[0] = y0;
            y[1] = y1;
            y[2] = y2;

            z = new float[3];
            z[0] = z0;
            z[1] = z1;
            z[2] = z2;
        }

        public static Vector3 operator *(Matrix3x3 matrix3x3, Vector3 vec3)
        {
            float x = matrix3x3.x[0] * vec3.x + matrix3x3.x[1] * vec3.y + matrix3x3.x[2] * vec3.z;
            float y = matrix3x3.y[0] * vec3.x + matrix3x3.y[1] * vec3.y + matrix3x3.y[2] * vec3.z;
            float z = matrix3x3.z[0] * vec3.x + matrix3x3.z[1] * vec3.y + matrix3x3.z[2] * vec3.z;
          
            return new Vector3(x, y, z);
        }

        public static Matrix3x3 operator *(Matrix3x3 matrix1, Matrix3x3 matrix2)
        {
            float x0 = matrix1.x[0] * matrix2.x[0] + matrix1.x[1] * matrix2.y[0] + matrix1.x[2] * matrix2.z[0];
            float y0 = matrix1.y[0] * matrix2.x[0] + matrix1.y[1] * matrix2.y[0] + matrix1.y[2] * matrix2.z[0];
            float z0 = matrix1.z[0] * matrix2.x[0] + matrix1.z[1] * matrix2.y[0] + matrix1.z[2] * matrix2.z[0];

            float x1 = matrix1.x[0] * matrix2.x[1] + matrix1.x[1] * matrix2.y[1] + matrix1.x[2] * matrix2.z[1];
            float y1 = matrix1.y[0] * matrix2.x[1] + matrix1.y[1] * matrix2.y[1] + matrix1.y[2] * matrix2.z[1];
            float z1 = matrix1.z[0] * matrix2.x[1] + matrix1.z[1] * matrix2.y[1] + matrix1.z[2] * matrix2.z[1];

            float x2 = matrix1.x[0] * matrix2.x[2] + matrix1.x[1] * matrix2.y[2] + matrix1.x[2] * matrix2.z[2];
            float y2 = matrix1.y[0] * matrix2.x[2] + matrix1.y[1] * matrix2.y[2] + matrix1.y[2] * matrix2.z[2];
            float z2 = matrix1.z[0] * matrix2.x[2] + matrix1.z[1] * matrix2.y[2] + matrix1.z[2] * matrix2.z[2];

            return new Matrix3x3(x0, y0, z0, x1, y1, z1, x2, y2, z2);
        }
    }
}
