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
    public partial class Wait : Form
    {
        public static Wait wait;

        public Wait()
        {
            InitializeComponent();
            wait = this;
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        public void SetText(string text)
        {
            label1.Text = text;
            Update();
        }
    }
}
