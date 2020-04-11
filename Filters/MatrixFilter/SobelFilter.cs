using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Filters.MatrixFilter
{
    public class SobelFilter : MatrixFilter
    {
        int[,] kernelX;
        int[,] kernelY;

        public SobelFilter()
        {
            kernelX = new int[3, 3] { { -1, 0, 1 }, { -2, 0, 2 }, { -1, 0, 1 } };
            kernelY = new int[3, 3] { { -1, -2, -1 }, { 0, 0, 0 }, { 1, 2, 1 } };
        }

        protected override Color calculateNewPixelColor(Bitmap sourceImage, int x, int y)
        {
            int radiusX = kernelX.GetLength(0) / 2;
            int radiusY = kernelY.GetLength(0) / 2;

            int rx = 0;
            int gx = 0;
            int bx = 0;

            int ry = 0;
            int gy = 0;
            int by = 0;

            for (int l = -radiusY; l <= radiusY; l++)
            {
                for (int k = -radiusX; k <= radiusX; k++)
                {
                    int idX = Clamp(x + k, 0, sourceImage.Width - 1);
                    int idY = Clamp(y + l, 0, sourceImage.Height - 1);
                    Color neighborColor = sourceImage.GetPixel(idX, idY);

                    rx += neighborColor.R * kernelX[k + radiusX, l + radiusY];
                    gx += neighborColor.G * kernelX[k + radiusX, l + radiusY];
                    bx += neighborColor.B * kernelX[k + radiusX, l + radiusY];

                    ry += neighborColor.R * kernelX[k + radiusX, l + radiusY];
                    gy += neighborColor.G * kernelX[k + radiusX, l + radiusY];
                    by += neighborColor.B * kernelX[k + radiusX, l + radiusY];
                }


            }

            int r = Clamp((int)Math.Sqrt(rx * rx + ry * ry));
            int g = Clamp((int)Math.Sqrt(gx * gx + gy * gy));
            int b = Clamp((int)Math.Sqrt(bx * bx + by * by));

            return Color.FromArgb(r, g, b);
        }
    }
}
