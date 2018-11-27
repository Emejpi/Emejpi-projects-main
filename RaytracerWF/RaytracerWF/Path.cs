using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RaytracerWF
{
    public partial class Path : Form
    {
        Wait wait;

        public Path()
        {
            InitializeComponent();
            wait = new Wait();
            wait.Show();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            //wait.Show();
            string path = textBox1.Text;
            Form1.path = path;
            Program.RunRaytracer();
            wait.Hide();
        }
    }
}
