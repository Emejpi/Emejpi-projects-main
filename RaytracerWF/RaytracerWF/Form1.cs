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
        public Lighting lighting;

        List<Primitive> primitives;

        public static Form1 main;

        public void AddPrimitive(Primitive primitive)
        {
            primitive.SetAmbient(lighting.ambient);
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
            //Transform transform = new Transform();
            //transform.CalcTransformMatrix();

            //for (int x = 0; x < transform.matrix.rows; x++)
            //{
            //    Debug.WriteLine("row" + x);
            //    for (int y = 0; y < transform.matrix.columns; y++)
            //    {
            //        Debug.WriteLine("column" + y);
            //        Debug.WriteLine("value -" + transform.matrix.grid[x,y]);
            //    }
            //}

                    camera = new Camera();
            lighting = new Lighting();

            primitives = new List<Primitive>();

            FileReader reader = new FileReader();
            reader.Read(@"C:\Users\Emejpi\Downloads\RayTracer-master\RayTracer-master\example_inputs\scene4-ambient.test");

            foreach (Primitive primitive in primitives)
                reader.DrawMatrix(primitive.transform);

            Debug.WriteLine("Read complited");

            Size = new System.Drawing.Size(camera.width + 5, camera.height + 40);
            pictureBox1.Size = new System.Drawing.Size(camera.width, camera.height - 1);

            Bitmap bmp = new Bitmap(camera.width, camera.height - 1);

            for (int y = 1; y < camera.height; y++)
            {
                for (int x = 0; x < camera.width; x++)
                {
                    Ray ray = camera.GetRay(x, y);

                    Color pixelColor = Color.Black;
                    //float x0;
                    float closestPointOfContact = 10000;
                    Vector3 pointOfContact;

                    foreach (Primitive primitive in primitives)
                    {
                        //Debug.WriteLine("before");
                        //reader.DrawMatrix(primitive.transform);

                        Ray transformedRay = primitive.transform.Reverse() * ray;
                        //Debug.WriteLine("after");
                        //reader.DrawMatrix(primitive.transform.Reverse());

                        if (primitive.Intersect(transformedRay, out pointOfContact))
                        {
                            pointOfContact = primitive.transform * pointOfContact;

                            float curDistance = Vector3.Distance(camera.position, pointOfContact);
                            if (curDistance < closestPointOfContact)
                            {
                                closestPointOfContact = curDistance;
                                pixelColor = primitive.ambient;
                            }
                        }
                    }
                    bmp.SetPixel(x, camera.height - y - 1, pixelColor);
                }
                Debug.WriteLine(((float)y / camera.height * 100) + "%");
            }

            pictureBox1.Image = bmp;

            bmp.Save("D:\\red.png");
        }
    }
}
