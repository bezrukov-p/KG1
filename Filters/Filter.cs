using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.ComponentModel;

namespace Filters
{
    public abstract class Filter
    {
        protected abstract Color calculateNewPixelColor(Bitmap sourceImage, int x, int y); // вычисляет значение каждого пикселя

        public int Clamp(int value)
        {
            return Math.Min(Math.Max(value, 0), 255);
        }

        public int Clamp(int value, int min, int max)
        {
            return Math.Min(Math.Max(value, min), max);
        }

        public virtual Bitmap ProcessImage(Bitmap sourceImage, BackgroundWorker worker) //создаёт новую картинку с другими пикселями в зависимости от функции calculatenewpix
        {
            Bitmap resultImage = new Bitmap(sourceImage.Width, sourceImage.Height);

            for(int i = 0; i < sourceImage.Width;i++)
            {
                if (worker != null)
                {
                    worker.ReportProgress((int)((float)i / resultImage.Width * 100));
                    if (worker.CancellationPending)
                        return null;
                }


                for (int j = 0; j < sourceImage.Height;j++)
                {
                    resultImage.SetPixel(i, j, calculateNewPixelColor(sourceImage, i, j));
                }
            }

            return resultImage;
        }

        public static Bitmap ProcessImage(Bitmap sourceImage, BackgroundWorker worker, Filter[] filters) 
        {
            Bitmap img = sourceImage;
            foreach(var f in filters)
            {
                img = f.ProcessImage(img, worker);
            }

            return img;
        }
    }
}
