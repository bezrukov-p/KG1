using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Filters.PointFilter
{
    public class GrayWorldFilter : Filter
    {
        int[] avg;
        double k;

        protected override Color calculateNewPixelColor(Bitmap sourceImage, int x, int y)
        {
            var p = sourceImage.GetPixel(x, y);

            var r = Clamp((int)(p.R * (k) / avg[0]));
            var g = Clamp((int)(p.G * (k) / avg[1]));
            var b = Clamp((int)(p.B * (k) / avg[2]));

            return Color.FromArgb(r, g, b);
        }

        public override Bitmap ProcessImage(Bitmap sourceImage, BackgroundWorker worker)
        {
            avg = new int[3];            
            
            avg[0] = 0;
            avg[1] = 0;
            avg[2] = 0;

            int n = 0;
            for (int i = 0; i < sourceImage.Width; ++i)
            {
                for (int j = 0; j < sourceImage.Height; ++j)
                {
                    var p = sourceImage.GetPixel(i, j);

                    avg[0] += p.R;
                    avg[1] += p.G;
                    avg[2] += p.B;

                    ++n; //Можно и не считать, но так не интересно
                }
            }

            avg[0] = (int)((double)avg[0] / n);
            avg[1] = (int)((double)avg[1] / n);
            avg[2] = (int)((double)avg[2] / n);

            k = ((avg[0] + avg[1] + avg[2]) / 3.0);

            return base.ProcessImage(sourceImage, worker);
        }
    }
}
