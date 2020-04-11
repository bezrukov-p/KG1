using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Filters.PointFilter
{
    public class BrightnessIncreaseFilter : Filter
    {
        protected override Color calculateNewPixelColor(Bitmap sourceImage, int x, int y)
        {
            const int k = 50;
            Color sourceColor = sourceImage.GetPixel(x, y);
            Color resultColor = Color.FromArgb(Clamp(sourceColor.R + k),
                                                Clamp(sourceColor.G + k),
                                                Clamp(sourceColor.B + k));
            return resultColor;
        }
    }
}
