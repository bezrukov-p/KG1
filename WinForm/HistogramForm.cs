using Filters;
using Filters.PointFilter;
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
    public partial class HistogramForm : Form
    {

        private Bitmap imageFromArray(int[] arr, Color c, out int max)
        {
            max = arr.Max();
            var k = 100.0 / (max);

            Bitmap img = new Bitmap(256, 100);

            for(int i = 0; i < 256; ++i)
            {
                for(int j = 0; j < Convert.ToInt32(arr[i] * k); ++j)
                {
                    img.SetPixel(i,img.Height - 1 - j,c);
                }
            }

            return img;
        }

        public int[] SetRHistgram
        {
            set
            {
                pictureBox2.Image = imageFromArray(value, Color.Red, out int max);
                labelR.Text = max + "";
            }
        }

        public int[] SetGHistgram
        {
            set
            {
                pictureBox3.Image = imageFromArray(value, Color.Green, out int max);
                labelG.Text = max + "";
            }
        }

        public int[] SetBHistgram
        {
            set
            {
                pictureBox4.Image = imageFromArray(value, Color.Blue, out int max);
                labelB.Text = max + "";
            }
        }

        public int[] SetLHistgram
        {
            set
            {
                pictureBox1.Image = imageFromArray(value, Color.Black, out int max);
                labelL.Text = max + "";
            }
        }

        public HistogramForm()
        {
            InitializeComponent();
        }

        public void BuildFromImage(Bitmap source)
        {
            var r = new int[256];
            var g = new int[256];
            var b = new int[256];

            var gray = new int[256];

            for (int i = 0; i < source.Width; ++i)
            {
                for (int j = 0; j < source.Height; ++j)
                {
                    var p = source.GetPixel(i, j);

                    r[p.R] += 1;
                    g[p.G] += 1;
                    b[p.B] += 1;
                }
            }

            Filter grayMaker = new GrayScaleFilter();

            var img = grayMaker.ProcessImage(source, null);

            for (int i = 0; i < img.Width; ++i)
            {
                for (int j = 0; j < img.Height; ++j)
                {
                    var p = img.GetPixel(i, j);
                    gray[p.R] += 1;
                }
            }


            SetRHistgram = r;
            SetGHistgram = g;
            SetBHistgram = b;
            SetLHistgram = gray;
        }
    }
}
