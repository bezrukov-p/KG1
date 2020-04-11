using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Filters.Morphology
{
    public class ErosionFilter : MorphologyFilter
    {
        public ErosionFilter()
        {
            bool[,] a = { { true, true, true }, { true, true, true }, { true, true, true } };
            mask = a;
        }

        public ErosionFilter(bool[,] _m)
        {
            mask = _m;
        }

        protected override Color calculateNewPixelColor(Bitmap sourceImage, int x, int y)
        {
            int[] min = new int[3];

            min[0] = 257;
            min[1] = 257;
            min[2] = 257;

            var w = mask.GetLength(0);
            var h = mask.GetLength(1);

            for (int i = -w / 2; i <= w / 2; ++i)
            {
                for (int j = -h / 2; j <= h / 2; ++j)
                {
                    if (mask[i + w / 2, j + h / 2])
                    {
                        var p = sourceImage.GetPixel(x + i, y + j);

                        min[0] = Math.Min(min[0], p.R);
                        min[1] = Math.Min(min[0], p.G);
                        min[2] = Math.Min(min[0], p.B);
                    }
                }
            }

            return Color.FromArgb(min[0], min[1], min[2]);
        }

    }
}
