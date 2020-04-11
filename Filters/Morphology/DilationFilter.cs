using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Filters.Morphology
{
    public class DilationFilter :  MorphologyFilter
    {
        public DilationFilter()
        {
            bool[,] a = { { true, true, true }, { true, true, true }, { true, true, true } };
            mask = a;
        }

        public DilationFilter(bool[,] _m)
        {
            mask = _m;
        }

        protected override Color calculateNewPixelColor(Bitmap sourceImage, int x, int y)
        {
            int[] max = new int[3];

            max[0] = -1;
            max[1] = -1;
            max[2] = -1;

            var w = mask.GetLength(0);
            var h = mask.GetLength(1);

            for (int i = -w/2; i <= w/2; ++i)
            {
                for(int j = -h/2; j <= h/2; ++j )
                {
                    if(mask[i + w/2, j + h/2])
                    {
                        var p = sourceImage.GetPixel(x + i, y + j);

                        max[0] = Math.Max(max[0], p.R);
                        max[1] = Math.Max(max[0], p.G);
                        max[2] = Math.Max(max[0], p.B);
                    }
                }
            }

            return Color.FromArgb(max[0], max[1], max[2]);
        }


    }
}
