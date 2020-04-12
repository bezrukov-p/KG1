using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Filters.Morphology
{
    public class GradFilter : MorphologyFilter
    {
        public GradFilter()
        {
            bool[,] a = { { false, true, false }, { true, true, true }, { false, true, false } };
            mask = a;
        }

        public GradFilter(bool[,] _m)
        {
            mask = _m;
        }
        

        protected override Color calculateNewPixelColor(Bitmap sourceImage, int x, int y)
        {
            //На самом деле не надо
            return Color.White;
        }

        public override Bitmap ProcessImage(Bitmap sourceImage, BackgroundWorker worker)
        {
            Bitmap bmp1 = (new DilationFilter(mask)).ProcessImage(sourceImage, worker);
            Bitmap bmp2 = (new ErosionFilter(mask)).ProcessImage(sourceImage, worker);

            Bitmap res = new Bitmap(sourceImage.Width, sourceImage.Height);
            for(int i = 0; i < res.Width; ++i)
            {

                for(int j = 0; j < res.Height; ++j)
                {
                    var p1 = bmp1.GetPixel(i, j);
                    var p2 = bmp2.GetPixel(i, j);

                    var r = Clamp(p1.R - p2.R);
                    var g = Clamp(p1.G - p2.G);
                    var b = Clamp(p1.B - p2.B);

                    res.SetPixel(i, j,
                        Color.FromArgb(r, g, b));
                }
            }

            return res;
            
        }
    }
}
