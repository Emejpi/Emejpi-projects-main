using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace RaytracerWF
{
    public class Camera
    {
        public int width;
        public int height;

        public Vector3 position;
        Vector3 lookDir;

        Vector3 center;

        float lookAngleX;
        float lookAngleY;

        Vector3 u, v, w;

        public void SetSize(int width, int height)
        {
            this.width = width;
            this.height = height + 1;
            center = new Vector3(width / 2, height / 2, 0);
        }

        public void SetTransform(Vector3 position, Vector3 LookAtPoint, Vector3 up, float lookAngleY)
        {
            this.position = position;
            lookDir = LookAtPoint - position;
            this.lookAngleY = Form1.DegreeToRadian(lookAngleY);
            Debug.WriteLine("Look angle y " + this.lookAngleY);
            lookAngleX = 2 * (float)Math.Atan((float)width / height * Math.Tan(this.lookAngleY / 2));
            Debug.WriteLine("Look angle x " + lookAngleX);

            w = (position - LookAtPoint).Normalize();
            u = Vector3.Cross(up, w).Normalize();
            v = Vector3.Cross(w, u);
        }



        Vector3 GetLookDirForAngle(float alpha, float beta)
        {
            return (u * alpha + v * beta - w).Normalize();
        }

        public Camera()
        {
            this.width = 500;
            this.height = 500;
            this.position = new Vector3();
            lookDir = new Vector3(0, 0, 1);
            center = new Vector3(width / 2, height / 2, 0);
        }

        public Camera(int width, int height, Vector3 position, Vector3 LookAtPoint)
        {
            this.width = width;
            this.height = height;
            this.position = position;
            lookDir = LookAtPoint - position;
            center = new Vector3(width / 2, height / 2, 0);
        }

        public Ray GetRay(int x, int y)
        {
            float alpha = (x - width / 2.0f) / (width / 2.0f) * (float)Math.Tan(lookAngleX / 2.0f);
            float beta = (y - height / 2.0f) / (height / 2.0f) * (float)Math.Tan(lookAngleY / 2.0f);

            Vector3 curLookDir = GetLookDirForAngle(alpha,beta);

            return new Ray(position, curLookDir);
        }
    }

    public class Ray
    {
        public Vector3 origin;
        public Vector3 direction;
        public Ray(Vector3 o, Vector3 d)
        {
            origin = o;
            direction = d;
        }
    }
}
