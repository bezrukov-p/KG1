using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp2
{
    public partial class OriginalWindow : Form
    {
        public Bitmap Original
        {
            set => this.pictureBox1.Image = value;
        }

        public OriginalWindow()
        {
            InitializeComponent();
        }
    }
}
