using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Filters.MatrixFilter
{
    public class EmbossingFilter : MatrixFilter
    {
        public EmbossingFilter()
        {
            kernel = new float[3, 3] { { 0, 1, 0 }, { 1, 0, -1 }, { 0, -1, 0 } };
        }

        protected override Color calculateNewPixelColor(Bitmap bmp, int x, int y)
        {
            int radiusX = kernel.GetLength(0) / 2;
            int radiusY = kernel.GetLength(1) / 2;

            float resultR = 0;
            float resultG = 0;
            float resultB = 0;
            for (int l = -radiusY; l <= radiusY; l++)
                for (int k = -radiusX; k <= radiusX; k++)
                {
                    int idX = Clamp(x + k, 0, bmp.Width - 1);
                    int idY = Clamp(y + l, 0, bmp.Height - 1);
                    Color neighborColor = bmp.GetPixel(idX, idY);
                    resultR += neighborColor.R * kernel[k + radiusX, l + radiusY];
                    resultG += neighborColor.G * kernel[k + radiusX, l + radiusY];
                    resultB += neighborColor.B * kernel[k + radiusX, l + radiusY];
                }

            //[0;510]
            resultR += 255;
            resultG += 255;
            resultB += 255;

            //[0;255]
            resultR /= 2;
            resultG /= 2;
            resultB /= 2;

            return Color.FromArgb(
                Clamp((int)resultR),
                Clamp((int)resultG),
                Clamp((int)resultB)
                );

        }
    }
}
