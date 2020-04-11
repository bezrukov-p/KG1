using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Filters.PointFilter
{
    public class TargetColorFilter : Filter
    {

        double rk;
        double gk;
        double bk;


        public TargetColorFilter(Color source, Color target)
        {
            rk = ((double)target.R/ source.R);
            gk = ((double)target.G / source.G);
            bk = ((double)target.B / source.B); ;
        }

        protected override Color calculateNewPixelColor(Bitmap sourceImage, int x, int y)
        {
            var p = sourceImage.GetPixel(x, y);

            var r = Clamp((int)(p.R * rk));
            var g = Clamp((int)(p.G * gk));
            var b = Clamp((int)(p.B * bk));

            return Color.FromArgb(r, g, b);
        }
    }
}
