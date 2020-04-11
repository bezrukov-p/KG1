using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Filters.PointFilter
{
    public class SepiaFilter : Filter
    {
        protected override Color calculateNewPixelColor(Bitmap sourceImage, int x, int y)
        {
            const int k = 20;
            Color sourceColor = sourceImage.GetPixel(x, y);
            double intensity = sourceColor.R * 0.299 + sourceColor.G * 0.587 + sourceColor.B * 0.114;
            Color resultColor = Color.FromArgb(Clamp(((int)intensity) + 2 * k),
                                                Clamp(((int)intensity) + k / 2),
                                                Clamp(((int)intensity) - k));
            return resultColor;
        }
    }
}
