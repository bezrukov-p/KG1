using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Filters.PointFilter
{
    public class LinearCorrectionFilter : Filter
    {
        int[] minimum;
        int[] maximum;

        protected override Color calculateNewPixelColor(Bitmap sourceImage, int x, int y)
        {
            var p = sourceImage.GetPixel(x, y);

            var r = (int)((p.R - minimum[0]) * ((255.0) / (maximum[0] - minimum[0])));
            var g = (int)((p.G - minimum[1]) * ((255.0) / (maximum[1] - minimum[1])));
            var b = (int)((p.B - minimum[2]) * ((255.0) / (maximum[2] - minimum[2])));

            return Color.FromArgb(Clamp(r),Clamp(g),Clamp(b));
        }

        public override Bitmap ProcessImage(Bitmap sourceImage, BackgroundWorker worker)
        {
            minimum = new int[3];
            maximum = new int[3];

            for(int i = 0; i < 3; ++i)
            {
                minimum[i] = 256;
                maximum[i] = -1;
            }

            for(int i = 0; i < sourceImage.Width; ++i)
            {
                for(int j = 0; j < sourceImage.Height; ++j)
                {
                    var p = sourceImage.GetPixel(i, j);

                    minimum[0] = Math.Min(p.R, minimum[0]);
                    maximum[0] = Math.Max(p.R, maximum[0]);

                    minimum[1] = Math.Min(p.G, minimum[1]);
                    maximum[1] = Math.Max(p.G, maximum[1]);

                    minimum[2] = Math.Min(p.B, minimum[2]);
                    maximum[2] = Math.Max(p.B, maximum[2]);
                }
            }

            return base.ProcessImage(sourceImage, worker);
        }
    }
}
