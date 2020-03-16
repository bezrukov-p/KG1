using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.ComponentModel;

namespace WindowsFormsApp2
{
    abstract class Filters
    {
        protected abstract Color calculateNewPixelColor(Bitmap sourceImage, int x, int y); // вычисляет значение каждого пикселя

        public int Clamp(int value, int min, int max)
        {
            if (value < min)
                return min;
            if (value > max)
                return max;
            return value;
        }

        public Bitmap processImage(Bitmap sourceImage, BackgroundWorker worker) //создаёт новую картинку с другими пикселями в зависимости от функции calculatenewpix
        {
            Bitmap resultImage = new Bitmap(sourceImage.Width, sourceImage.Height);

            for(int i = 0; i < sourceImage.Width;i++)
            {
                worker.ReportProgress((int)((float)i / resultImage.Width * 100));
                if (worker.CancellationPending)
                    return null;
                if (i == 630)
                {

                }

                for (int j = 0; j < sourceImage.Height;j++)
                {
                    resultImage.SetPixel(i, j, calculateNewPixelColor(sourceImage, i, j));
                }
            }

            return resultImage;
        }
    }

    class MatrixFilter : Filters
    {
        protected float[,] kernel = null;
        protected MatrixFilter() { }
        public MatrixFilter(float[,] kernel)
        {
            this.kernel = kernel;
        }

        protected override Color calculateNewPixelColor(Bitmap sourceImage, int x, int y)
        {
            int radiusX = kernel.GetLength(0) / 2;
            int radiusY = kernel.GetLength(1) / 2;

            float resultR = 0;
            float resultG = 0;
            float resultB = 0;
            for(int l = -radiusY; l <= radiusY; l++)
                for(int k = -radiusX; k <= radiusX; k++)
                {
                    int idX = Clamp(x + k, 0, sourceImage.Width - 1);
                    int idY = Clamp(y + l, 0, sourceImage.Height - 1);
                    Color neighborColor = sourceImage.GetPixel(idX, idY);
                    resultR += neighborColor.R * kernel[k + radiusX, l + radiusY];
                    resultG += neighborColor.G * kernel[k + radiusX, l + radiusY];
                    resultB += neighborColor.B * kernel[k + radiusX, l + radiusY];
                }
            return Color.FromArgb(
                Clamp((int)resultR, 0, 255),
                Clamp((int)resultG, 0, 255),
                Clamp((int)resultB, 0, 255)
                );
        }
    }

    class BlurFilter : MatrixFilter
    {
        public BlurFilter()
        {
            int sizeX = 3;
            int sizeY = 3;
            kernel = new float[sizeX, sizeY];
            for(int i = 0; i < sizeX; i++)
                for(int j = 0; j < sizeY; j++)
                    kernel[i, j] = 1.0f / (float)(sizeX * sizeY);
        }
    }

    class Tysnenie : MatrixFilter
    {
        public Tysnenie()
        {
            kernel = new float[3, 3] { { 0, 1, 0}, { 1, 0, -1}, { 0, -1, 0} };
        }
    }

    class GaussianFilter : MatrixFilter
    {
        public GaussianFilter()
        {
            createGaussianKernel(3, 2);
        }

        public void createGaussianKernel(int radius, float sigma)
        {
            //размер ядра
            int size = 2 * radius + 1; //размер ядра
            kernel = new float[size, size]; // ядро
            float norm = 0; // коэфф номировки ядра
            //расчитываем ядро линейного фильтра
            for (int i = -radius; i <= radius; i++)
                for(int j = -radius; j <= radius; j++)
                {
                    kernel[i + radius, j + radius] = (float)(Math.Exp(-(i * i + j * j) / (2 * sigma * sigma)));
                    norm += kernel[i + radius, j + radius];
                }
            //нормируем ядро
            for (int i = 0; i < size; i++)
                for (int j = 0; j < size; j++)
                    kernel[i, j] /= norm;

        }
    }

    class SobelFilter : MatrixFilter
    {

    }

    class Sharp : MatrixFilter
    {
        public Sharp()
        {
            kernel = new float[3, 3] { { 0, -1, 0 }, { -1, 5, -1 }, { 0, -1, 0 } };
        }
    }

    class Tisnenye : MatrixFilter /////////////////////
    {
        public Tisnenye()
        {
            kernel = new float[3, 3] { { 0, 1, 0 }, { 1, 0, -1 }, { 0, -1, 0} };
        }
    }

    class InvertFilter : Filters
    {
        protected override Color calculateNewPixelColor(Bitmap sourceImage, int x, int y)
        {
            Color sourceColor = sourceImage.GetPixel(x, y);
            Color resultColor = Color.FromArgb(255 - sourceColor.R,
                                                255 - sourceColor.G,
                                                255 - sourceColor.B);
            return resultColor;
        }
    }

    class GrayScaleFilter : Filters
    {
        protected override Color calculateNewPixelColor(Bitmap sourceImage, int x, int y)
        {
            Color sourceColor = sourceImage.GetPixel(x, y);
            double intensity = sourceColor.R * 0.299 + sourceColor.G * 0.587 + sourceColor.B * 0.114;
            Color resultColor = Color.FromArgb(Clamp(((int)intensity), 0, 255),
                                                Clamp(((int)intensity), 0, 255),
                                                Clamp(((int)intensity), 0, 255));
            return resultColor;
        }
    }

    class Sepia : Filters
    {
        protected override Color calculateNewPixelColor(Bitmap sourceImage, int x, int y)
        {
            const int k = 20;
            Color sourceColor = sourceImage.GetPixel(x, y);
            double intensity = sourceColor.R * 0.299 + sourceColor.G * 0.587 + sourceColor.B * 0.114;
            Color resultColor = Color.FromArgb(Clamp(((int)intensity) + 2 * k, 0, 255),
                                                Clamp(((int)intensity) + k / 2, 0, 255),
                                                Clamp(((int)intensity) - k, 0, 255));
            return resultColor;
        }
    }

    class BrightnessIncrease : Filters
    {
        protected override Color calculateNewPixelColor(Bitmap sourceImage, int x, int y)
        {
            const int k = 50;
            Color sourceColor = sourceImage.GetPixel(x, y);
            Color resultColor = Color.FromArgb(Clamp(sourceColor.R + k, 0, 255),
                                                Clamp(sourceColor.G + k, 0, 255),
                                                Clamp(sourceColor.B + k, 0, 255));
            return resultColor;
        }
    }
}
