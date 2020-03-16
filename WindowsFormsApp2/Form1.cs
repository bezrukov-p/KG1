using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing.Imaging;

namespace WindowsFormsApp2
{
    public partial class Form1 : Form
    {
        Bitmap image; // картинка

        public Form1()
        {
            InitializeComponent();
        }

        private void фильтрыToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void открытьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Filter = "Image files|*.png;*.jpg;*.bpm|All files(*.*)|*.*";
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                image = new Bitmap(dialog.FileName);
                pictureBox1.Image = image;
                pictureBox1.Refresh();
            }
        }

        private void инверсияToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Filters filter = new InvertFilter();
            //Bitmap resultImage = filter.processImage(image, backgroundWorker1);
            //pictureBox2.Image = resultImage;
            //pictureBox2.Refresh();
            backgroundWorker1.RunWorkerAsync(filter);
        }

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            Bitmap newImage = ((Filters)e.Argument).processImage(image, backgroundWorker1);
            if (backgroundWorker1.CancellationPending != true)
                image = newImage;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            backgroundWorker1.CancelAsync();
        }

        private void размытиеToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Filters filter = new BlurFilter();
            //Bitmap resultImage = filter.processImage(image, backgroundWorker1);
            //pictureBox2.Image = resultImage;
            //pictureBox2.Refresh();
            backgroundWorker1.RunWorkerAsync(filter);
        }

        private void фильтрГауссаToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Filters filter = new GaussianFilter();
            //Bitmap resultImage = filter.processImage(image, backgroundWorker1);
            //pictureBox2.Image = resultImage;
            //pictureBox2.Refresh();
            backgroundWorker1.RunWorkerAsync(filter);
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void вОттенкахСерогоToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Filters filter = new GrayScaleFilter();
            //Bitmap resultImage = filter.processImage(image, backgroundWorker1);
            //pictureBox2.Image = resultImage;
            //pictureBox2.Refresh();
            backgroundWorker1.RunWorkerAsync(filter);
        }

        private void сепияToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Filters filter = new Sepia();
            //Bitmap resultImage = filter.processImage(image, backgroundWorker1);
            //pictureBox2.Image = resultImage;
            //pictureBox2.Refresh();
            backgroundWorker1.RunWorkerAsync(filter);
        }

        private void увеличениеЯркостиToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Filters filter = new BrightnessIncrease();
            //Bitmap resultImage = filter.processImage(image, backgroundWorker1);
            //pictureBox2.Image = resultImage;
            //pictureBox2.Refresh();
            backgroundWorker1.RunWorkerAsync(filter);
        }

        private void резкостьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Filters filter = new Sharp();
            //Bitmap resultImage = filter.processImage(image, backgroundWorker1);
            //pictureBox2.Image = resultImage;
            //pictureBox2.Refresh();
            backgroundWorker1.RunWorkerAsync(filter);
        }

        private void файлToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void сохранитьToolStripMenuItem_Click(object sender, EventArgs e)//
        {
            Bitmap bmpSave = (Bitmap)pictureBox2.Image;
            SaveFileDialog dialog = new SaveFileDialog();
            dialog.Filter = "Image files|*.png;*.jpg;*.bpm|All files(*.*)|*.*";

            if (dialog.ShowDialog() == DialogResult.OK)
            {
                bmpSave.Save(dialog.FileName, ImageFormat.Bmp);
            }
        }

        private void backgroundWorker1_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            progressBar1.Value = e.ProgressPercentage;
        }

        private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (!e.Cancelled)
            {
                pictureBox2.Image = image;///
                pictureBox2.Refresh();
            }
            progressBar1.Value = 0;
        }

        private void тиснениеToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void тиснениеToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            Filters filter = new Tysnenie();
            backgroundWorker1.RunWorkerAsync(filter);
        }
    }
}
