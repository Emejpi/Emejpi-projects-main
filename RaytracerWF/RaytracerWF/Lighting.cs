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
    public class Lighting
    {
        public Color ambient;

        public Lighting()
        {
            ambient = Color.Red;
        }

        public Color GetColor()
        {
            return ambient;
        }
    }
}
