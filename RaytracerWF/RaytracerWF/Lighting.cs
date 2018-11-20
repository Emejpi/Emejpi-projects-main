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
    public class Light
    {
        public Color color;

        public virtual Vector3 GetDiraction(Vector3 atPose)
        {
            return new Vector3();
        }

        public virtual Color GetLightColor()
        {
            return color;
        }

        public Color GetColorAtPoint(Vector3 point, Material material, Vector3 normal)
        {
            Vector3 direction = GetDiraction(point);

            return AddColors(material.ambient
                , MultiplyColors(GetLightColor(), MultiplyColor(material.diffuse,Max(0, Vector3.Dot(direction, normal)))));
        }

        public Color MultiplyColors(Color c1, Color c2)
        {
            return Color.FromArgb(c1.R * c2.R, c1.G * c2.G, c1.B * c2.B);
        }

        public Color MultiplyColor(Color c1, float value)
        {
            return Color.FromArgb((int)(c1.R * value), (int)(c1.G * value), (int)(c1.B * value));
        }

        public Color AddColors(Color c1, Color c2)
        {
            return Color.FromArgb(c1.R + c2.R, c1.G + c2.G, c1.B + c2.B);
        }

        float Max(float v1, float v2)
        {
            if (v1 > v2)
                return v1;
            return v2;
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
    }

    public class PointLight : Light
    {
        public PointLight()
        {
            constant = 1;
            linear = 0;
            quadratic = 0;

            position = new Vector3();
        }

        public Vector3 position;
        public float constant;
        public float linear;
        public float quadratic;

        public override Vector3 GetDiraction(Vector3 atPose)
        {
            return (position - atPose).Normalize();
        }
    }

    public class Material
    {
        public Color ambient;
        public Color diffuse;
        public Color specular;
        public Color emission;
        public float shininess;

        public Material()
        {
            ambient = Color.Black;
            diffuse = Color.Black;
            specular = Color.Black;
            emission = Color.Black;
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
