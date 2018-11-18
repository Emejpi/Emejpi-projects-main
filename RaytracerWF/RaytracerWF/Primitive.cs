using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Drawing;

namespace RaytracerWF
{
    public class Sphere : Primitive
    {
        public Vector3 center;
        public float radius;

        public Sphere(Vector3 c, float r)
        {
            center = c;
            radius = r;
        }

        public override bool Intersect(Ray ray, out Vector3 pointOfContact, out Vector3 normal)
        {
            //x = 0;
            Vector3 distance = ray.origin - center;
            float a = Vector3.Dot(ray.direction, ray.direction);
            float b = Vector3.Dot(distance, ray.direction) * 2;
            float c = Vector3.Dot(distance, distance) - radius * radius;
            float delta = b * b - 4 * a * c;
            if (delta < 0)
            {
                pointOfContact = new Vector3();
                normal = new Vector3();
                return false;
            }
            else
            {
                float deltaSqrt = (float)Math.Sqrt(delta);
                float x1 = (-b - deltaSqrt) / (2 * a);
                float x2 = (-b + deltaSqrt) / (2 * a);

                float x = x1 > x2 ? x2 : x1;
                pointOfContact = ray.origin + ray.direction * x;
                normal = (pointOfContact - center).Normalize();
                //pointOfContact = transform.Reverse() * pointOfContact;
                //if (Vector3.Dot(pointOfContact - center, pointOfContact - center) - radius * radius != 0)
                //    return false;
                return true;
            }
        }
    }

    public class Triangle : Primitive
    {
        public Vector3 vert1;
        public Vector3 vert2;
        public Vector3 vert3;

        public Vector3 normal;

        public Triangle(Vector3 v1, Vector3 v2, Vector3 v3)
        {
            vert1 = v1;
            vert2 = v2;
            vert3 = v3;
        }

        public override bool Intersect(Ray ray, out Vector3 pointOfContact, out Vector3 normal)
        {
            pointOfContact = GetPointOfContact(ray);
            normal = this.normal;
            //pointOfContact = transform.Reverse() * pointOfContact;

            float bery1 = GetBaryCentricCoord(vert3, vert2, vert1, pointOfContact);
            if (bery1 > 1 || bery1 < 0)
                return false;

            float bery2 = GetBaryCentricCoord(vert2, vert1, vert3, pointOfContact);
            if (bery2 > 1 || bery2 < 0)
                return false;

            float bery3 = GetBaryCentricCoord(vert1, vert3, vert2, pointOfContact);
            if (bery3 > 1 || bery3 < 0)
                return false;

            return true;
        }

        public static float GetBaryCentricCoord(Vector3 A, Vector3 B, Vector3 C, Vector3 I)
        {
            Vector3 AB = B - A;
            Vector3 CB = B - C;
            Vector3 AI = I - A;
            Vector3 V = AB - Vector3.Projection(AB, CB);
            return 1 - (Vector3.Dot(V, AI) / Vector3.Dot(V, AB));
        }

        public Vector3 Normal()
        {
            return Vector3.Cross(vert1 - vert2, vert3 - vert1);
        }

        public Vector3 GetPointOfContact(Ray ray)
        {
            Vector3 fromCameraToVect1 = vert1 - ray.origin;
            return ray.origin + ray.direction 
                * ((Vector3.Dot(fromCameraToVect1, normal)) 
                / (Vector3.Dot(ray.direction, normal)));
        }
    }

    public class Primitive
    {
        public Color ambient;
        public Matrix transform;
        public Matrix reverseTransform;

        public void SetAmbient(Color color)
        {
            ambient = color;
        }

        public virtual bool Intersect(Ray ray, out Vector3 pointOfContact, out Vector3 normal)
        {
            pointOfContact = new Vector3();
            normal = new Vector3();
            return false;
        }
    }
}
