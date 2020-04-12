using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Filters.MatrixFilter
{
    public class MedianFilter : MatrixFilter
    {

        public MedianFilter()
        {
            float[,] a = { {1,1,1 }, { 1,1,1 }, { 1,1,1 } };
            kernel = a;
        }

        protected override Color calculateNewPixelColor(Bitmap sourceImage, int x, int y)
        {
            List<Color> colors = new List<Color>();

            var w = kernel.GetLength(0);
            var h = kernel.GetLength(1);

            for (int i = -w / 2; i <= w / 2; ++i)
            {
                for (int j = -h / 2; j <= h / 2; ++j)
                {
                    var p = sourceImage.GetPixel(
                        Clamp(x + i,0, sourceImage.Width -1), 
                        Clamp(y + j,0,sourceImage.Height - 1));

                    colors.Add(p);
                }
            }

            var aList = colors.Select(c => (int)((c.R + c.G + c.B) / 3.0)).OrderBy(c => c).ToList();
            var col = aList[aList.Count / 2];

            return Color.FromArgb(col, col, col);
        }
    }
}
