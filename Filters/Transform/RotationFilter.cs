using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Filters.Transform
{
    public class RotationFilter : Filter
    {
        double angle;

        public RotationFilter(double a)
        {
            angle = a;
        }

        protected override Color calculateNewPixelColor(Bitmap sourceImage, int x, int y)
        {
            var x0 = 0;
            var y0 = 0;

            var nx = (int)((x - x0) * Math.Cos(angle) - (y - y0) * Math.Sin(angle) + x0);
            var ny = (int)((x - x0) * Math.Sin(angle) + (y - y0) * Math.Cos(angle) + y0);

            if (nx < 0 || ny < 0 || nx >= sourceImage.Width || ny >= sourceImage.Height)
                return Color.Black;

            return sourceImage.GetPixel(nx,ny);
        }

        public override Bitmap ProcessImage(Bitmap sourceImage, BackgroundWorker worker)
        {
            //Данный фильтр предполагает, что вращать будем на 90 градусов
            Bitmap res = new Bitmap(sourceImage.Height, sourceImage.Width);
            for(int i = 0; i < res.Width; ++i)
            {
                for(int j = 0; j < res.Height; ++j)
                {
                    res.SetPixel(i, j, calculateNewPixelColor(sourceImage, (int)(res.Width * (-1 + Math.Sin(angle))/2) +  i, -(int)(res.Height * (1 + Math.Sin(angle))/2)+j));
                }
            }

            return res;
        }
    }
}
