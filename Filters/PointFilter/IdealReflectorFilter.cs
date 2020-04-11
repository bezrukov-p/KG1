using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Filters.PointFilter
{
    public class IdealReflectorFilter : Filter
    {
        int[] maximum;

        protected override Color calculateNewPixelColor(Bitmap sourceImage, int x, int y)
        {
            var p = sourceImage.GetPixel(x, y);

            var r = (int)((p.R) * ((255.0) / (maximum[0])));
            var g = (int)((p.G) * ((255.0) / (maximum[1])));
            var b = (int)((p.B) * ((255.0) / (maximum[2])));

            return Color.FromArgb(Clamp(r), Clamp(g), Clamp(b));
        }

        public override Bitmap ProcessImage(Bitmap sourceImage, BackgroundWorker worker)
        {
            maximum = new int[3];

            for (int i = 0; i < 3; ++i)
            {
                maximum[i] = -1;
            }

            for (int i = 0; i < sourceImage.Width; ++i)
            {
                for (int j = 0; j < sourceImage.Height; ++j)
                {
                    var p = sourceImage.GetPixel(i, j);

                    maximum[0] = Math.Max(p.R, maximum[0]);
                    maximum[1] = Math.Max(p.G, maximum[1]);
                    maximum[2] = Math.Max(p.B, maximum[2]);
                }
            }

            return base.ProcessImage(sourceImage, worker);
        }
    }
}
