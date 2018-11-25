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

namespace RaytracerWF // Możliwe że intersect źle działa pod kątem
{
   
    public partial class Form1 : Form
    {
        public Camera camera;
        public Light light;

        List<Primitive> primitives;

        public static Form1 main;

        public void AddPrimitive(Primitive primitive)
        {
            //primitive.SetAmbient(lighting.ambient);
            primitives.Add(primitive);
        }

        public static float RadianTuDegree(float radian)
        {
            return (float)(radian * 180 / Math.PI);
        }

        public static float DegreeToRadian(float degree)
        {
            return (float)(degree * Math.PI / (float)180);
        }

        public Form1()
        {
            main = this;

            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            camera = new Camera();

            primitives = new List<Primitive>();

            FileReader reader = new FileReader();
            reader.Read(@"C:\Users\Emejpi\Downloads\RayTracer-master\RayTracer-master\example_inputs\scene5.test");

            Debug.WriteLine(light.color.ToString());

            foreach (Primitive primitive in primitives)
            {
                Debug.WriteLine("trans");
                reader.DrawMatrix(primitive.transform);
                Debug.WriteLine("rev");
                reader.DrawMatrix(primitive.reverseTransform);
                Debug.WriteLine("*");
                reader.DrawMatrix(primitive.reverseTransform * primitive.transform);
            }

            Debug.WriteLine("Read complited");

            Size = new System.Drawing.Size(camera.width + 5, camera.height + 40);
            pictureBox1.Size = new System.Drawing.Size(camera.width, camera.height - 1);

            Bitmap bmp = new Bitmap(camera.width, camera.height - 1);

            int pixelsCount = camera.height * camera.width;
            float percHitSomething = 0;
            float percInShadow = 0;
            bool noLighting = false;

            for (int y = 1; y < camera.height; y++)
            {
                for (int x = 0; x < camera.width; x++)
                {
                    Ray ray = camera.GetRay(x, y);

                    Color pixelColor = Color.Black;
                    //float x0;
                    float closestPointOfContact = 10000;
                    Vector3 pointOfContact;

                    bool hitSomething = false;
                    bool inShadow = false;

                    foreach (Primitive primitive in primitives)
                    {
                        Vector3 norm;

                        Vector3 pointBehind = ray.origin + ray.direction * 1000;
                        if (primitive.Intersect(ray, out pointOfContact, out norm))
                        {
                            //float oldPointOfContactToLightR = Vector3.Distance(ray.origin, pointBehind);
                            //float newPointOfContactToLightR = Vector3.Distance(pointOfContact, pointBehind);
                            //float oldToNewR = Vector3.Distance(ray.origin, pointOfContact);

                            //if (float.IsNaN(oldToNewR)
                            //|| oldPointOfContactToLightR < newPointOfContactToLightR
                            //|| oldPointOfContactToLightR < oldToNewR)
                            //{
                            //    continue;
                            //}

                                float curDistance = Vector3.Distance(camera.position, pointOfContact);
                            if (curDistance < closestPointOfContact)
                            {
                                hitSomething = true;

                                closestPointOfContact = curDistance;

                                //==LIGHT==//
                                bool visible = true;
                                if (!noLighting)
                                {
                                    Vector3 lightPose = light.GetPosition();

                                    Ray toLightRay = new Ray(pointOfContact, (light.GetDiraction(pointOfContact) * -1).Normalize()); //poszukaj filmikow, przeanalizuj laski
                                    foreach (Primitive lightPrim in primitives)
                                    {
                                        if (lightPrim == primitive)
                                            continue;

                                        Vector3 toLightPointOfContact;
                                        Vector3 toLightNormal;

                                        if (lightPrim.Intersect(toLightRay, out toLightPointOfContact, out toLightNormal))
                                        {
                                            float oldPointOfContactToLight = Vector3.Distance(pointOfContact, lightPose);
                                            float newPointOfContactToLight = Vector3.Distance(toLightPointOfContact, lightPose);
                                            float oldToNew = Vector3.Distance(pointOfContact, toLightPointOfContact);

                                            if (!float.IsNaN(oldToNew)
                                            && oldPointOfContactToLight > newPointOfContactToLight
                                            && oldPointOfContactToLight > oldToNew)
                                            {
                                                visible = false;
                                                inShadow = true;
                                                //pixelColor = Color.Black;
                                                break;
                                            }
                                        }
                                    }
                                }

                                MyColor myPixelColor = new MyColor(0, 0, 0);

                                if (visible)
                                {
                                    myPixelColor = noLighting ? primitive.material.diffuse : light.GetColorAtPoint(ray.origin, pointOfContact, primitive.material, norm);
                                }

                                    if (!noLighting)
                                    {
                                        //==Reflections==//
                                        Ray currentRay = ray;
                                        Vector3 currentNormal = norm;
                                        Vector3 currentPoinOfContact = pointOfContact;
                                        Primitive currentPrimitive = primitive;
                                        MyColor currentSpecular = currentPrimitive.material.specular;
                                    if (!visible)
                                        currentSpecular /= 2;
                                        for (int i = 0; i < 5; i++)
                                        {
                                            bool intersectionFound = false;

                                            Vector3 v = currentRay.direction;
                                            Vector3 n = currentNormal;
                                            Vector3 r = v - n * (2 * (Vector3.Dot(n, v)));

                                            currentRay = new Ray(currentPoinOfContact, r.Normalize());
                                            Vector3 pointBehindIntersected = currentRay.origin + currentRay.direction * 1000;
                                            foreach (Primitive primReflect in primitives)
                                            {
                                                if (currentPrimitive == primReflect)
                                                    continue;

                                                if (primReflect.Intersect(currentRay, out currentPoinOfContact, out currentNormal))
                                                {
                                                    //if (!float.IsNaN(Vector3.Distance(currentPoinOfContact, currentRay.origin))
                                                    //    && Vector3.Distance(currentRay.origin, pointBehindIntersected) > Vector3.Distance(currentPoinOfContact, pointBehindIntersected))
                                                    //{
                                                    float oldPointOfContactToLight = Vector3.Distance(currentRay.origin, pointBehindIntersected);
                                                    float newPointOfContactToLight = Vector3.Distance(currentPoinOfContact, pointBehindIntersected);
                                                    float oldToNew = Vector3.Distance(currentRay.origin, currentPoinOfContact);

                                                    if (!float.IsNaN(oldToNew)
                                                    && oldPointOfContactToLight > newPointOfContactToLight
                                                    && oldPointOfContactToLight > oldToNew)
                                                    {
                                                        MyColor newColor = light.GetColorAtPoint(currentRay.origin, currentPoinOfContact, primReflect.material, currentNormal);
                                                        myPixelColor = (myPixelColor + (newColor * currentSpecular));
                                                        currentSpecular = primReflect.material.specular * currentSpecular;
                                                        intersectionFound = true;
                                                        break;
                                                    }
                                                }
                                            }

                                            if (!intersectionFound)
                                                break;
                                        }
                                    }

                                    pixelColor = myPixelColor.ToColor();
                                
                            }
                        }
                    }
                    if (hitSomething)
                    {
                        percHitSomething += 1 / (float)pixelsCount;
                    }
                    if (inShadow)
                    {
                        percInShadow += 1 / (float)pixelsCount;
                    }

                    bmp.SetPixel(x, camera.height - y - 1, pixelColor);
                }
                Debug.WriteLine(((float)y / camera.height * 100) + "%");
            }

            Debug.WriteLine("perc hit " + percHitSomething * 100 + "%");
            Debug.WriteLine("perc in shadow " + percInShadow * 100 + "%");

            pictureBox1.Image = bmp;

            bmp.Save("D:\\red.png");
        }
    }
}
