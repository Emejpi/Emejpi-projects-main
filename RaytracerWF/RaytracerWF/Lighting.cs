using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;


namespace RaytracerWF
{
    public class MyColor
    {
        public int r;
        public int g;
        public int b;

        public static MyColor Black = new MyColor(0, 0, 0);

        public MyColor(Color color)
        {
            r = color.R;
            g = color.G;
            b = color.B;
        }

        public MyColor(int R, int G, int B)
        {
            r = R;
            g = G;
            b = B;
        }

        public Vector3 ToFloat()
        {
            return new Vector3((float)r / 255, (float)g / 255, (float)b / 255);
        }

        public Color ToColor()
        {
            int r = Math.Min(255, this.r);
            int g = Math.Min(255, this.g);
            int b = Math.Min(255, this.b);
            return Color.FromArgb(r, g, b);
        }

        public string ToString()
        {
            return "R - " + r + " : G - " + g + " : B - " + b;
        }

        public static MyColor operator *(MyColor c1, MyColor c2)
        {
            Vector3 v1 = c1.ToFloat();
            Vector3 v2 = c2.ToFloat();

            Vector3 v = v1 * v2;

            return v.ToMyColor();
        }



        public static MyColor operator *(MyColor c1, float value)
        {
            Vector3 v1 = c1.ToFloat();
            Vector3 v = v1 * value;

            return v.ToMyColor();
        }

        public static MyColor operator /(MyColor c1, float value)
        {
            Vector3 v1 = c1.ToFloat();
            Vector3 v = v1 / value;

            return v.ToMyColor();
        }

        public static MyColor operator +(MyColor c1, MyColor c2)
        {
            return new MyColor(c1.r + c2.r, c1.g + c2.g, c1.b + c2.b);
        }
    }

    public class Lighting
    {
        public List<Light> lights;

        public Lighting()
        {
            lights = new List<Light>();
        }

        public MyColor GetColorAtPoint(Vector3 cameraPose, Vector3 point, Material material, Vector3 normal, bool forceVisible = false)
        {
            MyColor ambient = material.ambient;
            MyColor emission = material.emission;

            MyColor outColor = ambient + emission;
            foreach (Light light in lights)
            {
                int visible = 0;
                if (light.visible || forceVisible)
                    visible = 1;

                Vector3 lightDirection = light.GetDiraction(point);
                Vector3 toCamera = (cameraPose - point).Normalize();
                Vector3 halfVector = (lightDirection + toCamera).Normalize();
                MyColor lightColor = light.GetLightColor(point);

                MyColor diffuse = material.diffuse * (Max(0, Vector3.Dot(lightDirection, normal)));
                MyColor specular = material.specular * (float)Math.Pow(Max(0, Vector3.Dot(halfVector, normal)), material.shininess);
                if (forceVisible)
                    specular /= 2;

                outColor += lightColor* visible *(diffuse + specular);
            }
            return outColor;
        }

        float Max(float v1, float v2)
        {
            if (v1 > v2)
                return v1;
            return v2;
        }
    }

    public class Light
    {
        public MyColor color;
        public bool visible;

        public virtual Vector3 GetDiraction(Vector3 atPose)
        {
            return new Vector3();
        }

        public virtual MyColor GetLightColor(Vector3 point)
        {
            return color;
        }

        public virtual Vector3 GetPosition()
        {
            return new Vector3();
        }

    }

    public class DirectionalLight : Light
    {
        public DirectionalLight()
        {
            direction = new Vector3();
        }

        public Vector3 direction;

        public override Vector3 GetDiraction(Vector3 atPose)
        {
            return direction;
        }

        public override Vector3 GetPosition()
        {
            return (direction * -1000);
        }
    }

    public class PointLight : Light
    {
        public PointLight()
        {
            if(constant == 0)
                constant = 1;

            position = new Vector3();
        }

        public Vector3 position;
        public static float constant;
        public static float linear;
        public static float quadratic;

        public override Vector3 GetPosition()
        {
            return position;
        }

        public override Vector3 GetDiraction(Vector3 atPose)
        {
            return (position - atPose).Normalize();
        }

        public override MyColor GetLightColor(Vector3 point)
        {
            float distance = (position - point).Magnitude();
            return color; //* (1 / (constant + linear * distance + distance * distance * quadratic));
        }
    }

    public class Material
    {
        public MyColor ambient;
        public MyColor diffuse;
        public MyColor specular;
        public MyColor emission;
        public float shininess;

        public Material()
        {
            ambient = MyColor.Black;
            diffuse = MyColor.Black;
            specular = MyColor.Black;
            emission = MyColor.Black;
            shininess = 0;
        }

        public Material(Material material)
        {
            ambient = material.ambient;
            diffuse = material.diffuse;
            specular = material.specular;
            emission = material.emission;
            shininess = material.shininess;
        }
    }
}
