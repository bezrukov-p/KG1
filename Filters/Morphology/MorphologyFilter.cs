using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Filters.Morphology
{
    public abstract class MorphologyFilter : Filter
    {
        protected bool[,] mask;

        public override Bitmap ProcessImage(Bitmap sourceImage, BackgroundWorker worker)
        {
            var width = mask.GetLength(0);
            var height = mask.GetLength(1);


            Bitmap resultImage = new Bitmap(sourceImage.Width, sourceImage.Height);

            for (int i = width / 2; i < sourceImage.Width - width / 2; i++)
            {
                if (worker != null)
                {
                    worker.ReportProgress((int)((float)i / resultImage.Width * 100));
                    if (worker.CancellationPending)
                        return null;
                }


                for (int j = height / 2; j < sourceImage.Height - height / 2; j++)
                {
                    resultImage.SetPixel(i, j, calculateNewPixelColor(sourceImage, i, j));
                }
            }

            return resultImage;
        }
    }
}
