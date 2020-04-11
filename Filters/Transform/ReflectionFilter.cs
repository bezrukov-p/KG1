using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Filters.Transform
{
    public class ReflectionFilter : Filter
    {
        bool vertical;

        public ReflectionFilter(bool v)
        {
            vertical = v;
        }

        protected override Color calculateNewPixelColor(Bitmap sourceImage, int x, int y)
        {
            if (vertical)
                return sourceImage.GetPixel(x, sourceImage.Height - 1 - y);
            else
                return sourceImage.GetPixel(sourceImage.Width - 1 - x,  y);
        }
    }
}
