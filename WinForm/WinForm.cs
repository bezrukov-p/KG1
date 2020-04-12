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

using Filters;
using Filters.PointFilter;
using Filters.MatrixFilter;
using Filters.Morphology;
using Filters.Transform;

namespace WindowsFormsApp2
{
    public partial class WinForm : Form
    {
        Bitmap Image { get; set; } // картинка
        Bitmap SourceImage { get; set; }

        HistogramForm HisWinSource { get; set; }
        HistogramForm HisWinTarget { get; set; }

        OriginalWindow OrigWin { get; set; }

        EditForm MorphEditForm { get; set; }

        Stack<Bitmap> before;
        Stack<Bitmap> after;

        bool[,] MorphMask;

        public WinForm()
        {
            InitializeComponent();


            //Надо ограничивать, но будем считать, что у нас бесконечная память
            before = new Stack<Bitmap>();
            after = new Stack<Bitmap>();

            впередToolStripMenuItem.Enabled = false;
            назадToolStripMenuItem.Enabled = false;

            bool[,] mask = { { true, true, true }, { true, true, true }, { true, true, true } };
            MorphMask = mask;
        }

        #region MenuButtons

        #region Файл

        private void открытьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Filter = "Image files|*.png;*.jpg;*.bpm|All files(*.*)|*.*";
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                SourceImage = new Bitmap(dialog.FileName);
                pictureBox2.Image = SourceImage;
                Image = SourceImage;
            }

            before.Clear();
            after.Clear();

            впередToolStripMenuItem.Enabled = false;
            назадToolStripMenuItem.Enabled = false;
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

        #endregion

        #region Фильтр

        private void инверсияToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Filter filter = new InvertFilter();
            backgroundWorker1.RunWorkerAsync(filter);
        }

        private void размытиеToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Filter filter = new BlurFilter();
            backgroundWorker1.RunWorkerAsync(filter);
        }

        private void фильтрГауссаToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Filter filter = new GaussianFilter();
            backgroundWorker1.RunWorkerAsync(filter);
        }

        private void вОттенкахСерогоToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Filter filter = new GrayScaleFilter();
            backgroundWorker1.RunWorkerAsync(filter);
        }

        private void сепияToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Filter filter = new SepiaFilter();
            backgroundWorker1.RunWorkerAsync(filter);
        }

        private void увеличениеЯркостиToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Filter filter = new BrightnessIncreaseFilter();
            backgroundWorker1.RunWorkerAsync(filter);
        }

        private void резкостьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Filter filter = new SharpFilter();
            backgroundWorker1.RunWorkerAsync(filter);
        }

        private void тиснениеToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            Filter filter = new EmbossingFilter();
            backgroundWorker1.RunWorkerAsync(filter);
        }

        private void фильтрСоболяToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Filter filter = new SobelFilter();
            backgroundWorker1.RunWorkerAsync(filter);
        }

        #endregion

        #region Окна

        private async void исходниоеИзображениеToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (SourceImage == null)
                return;

            if (HisWinSource != null)
            {
                HisWinSource.Close();
                HisWinSource = null;
            }
            else
            {
                HisWinSource = new HistogramForm();
                HisWinSource.FormClosed += (s, o) => { HisWinSource = null; };

                this.исходниоеИзображениеToolStripMenuItem.Enabled = false;
                await Task.Run(() =>
                {
                    HisWinSource.BuildFromImage(SourceImage);
                });
                this.исходниоеИзображениеToolStripMenuItem.Enabled = true;
                HisWinSource.Show();
            }
        }

        private async void послеПреобразованияToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (Image == null)
                return;

            if (HisWinTarget != null)
            {
                HisWinTarget.Close();
                HisWinTarget = null;
            }
            else
            {
                HisWinTarget = new HistogramForm();
                HisWinTarget.FormClosed += (s, o) => { HisWinSource = null; };

                this.послеПреобразованияToolStripMenuItem.Enabled = false;
                await Task.Run(() =>
                {
                    HisWinTarget.BuildFromImage(Image);
                });
                this.послеПреобразованияToolStripMenuItem.Enabled = true;
                HisWinTarget.Show();
            }
        }

        private void оригиналToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (SourceImage == null)
                return;

            if (OrigWin != null)
            {
                OrigWin.Close();
                OrigWin = null;
            }
            else
            {
                OrigWin = new OriginalWindow();
                OrigWin.Original = SourceImage;
                OrigWin.FormClosed += (f1, f2) => { OrigWin = null; };
                OrigWin.Show();
            }
        }

        #endregion

        #region Цветокор

        private void линейноеРастяжениеToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Filter filter = new LinearCorrectionFilter();
            backgroundWorker1.RunWorkerAsync(filter);
        }

        private void серыйМирToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Filter filter = new GrayWorldFilter();
            backgroundWorker1.RunWorkerAsync(filter);
        }

        private void опорныйЦветToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ColorDialog cd = new ColorDialog();
            cd.AllowFullOpen = true;
            cd.Color = Color.White;


            Color source;
            if (cd.ShowDialog() != DialogResult.OK)
                return;

            source = cd.Color;

            cd = new ColorDialog();
            cd.AllowFullOpen = true;
            cd.Color = Color.White;

            Color target;
            if (cd.ShowDialog() != DialogResult.OK)
                return;

            target = cd.Color;

            Filter filter = new TargetColorFilter(source, target);
            backgroundWorker1.RunWorkerAsync(filter);
        }

        private void идеальныйОтражательToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Filter filter = new IdealReflectorFilter();
            backgroundWorker1.RunWorkerAsync(filter);
        }

        #endregion

        #region Морфология

        private void расширениеToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Filter filter = new DilationFilter(MorphMask);
            backgroundWorker1.RunWorkerAsync(filter);
        }

        private void сужениеToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Filter filter = new ErosionFilter(MorphMask);
            backgroundWorker1.RunWorkerAsync(filter);
        }

        private void открытеToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Filter erosion = new DilationFilter(MorphMask);
            Filter dilation = new ErosionFilter(MorphMask);

            Filter[] arr = { erosion, dilation };

            backgroundWorker1.RunWorkerAsync(arr);
        }

        private void закрытиеToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Filter dilation = new DilationFilter(MorphMask);
            Filter erosion = new ErosionFilter();

            Filter[] arr = { dilation, erosion };

            backgroundWorker1.RunWorkerAsync(arr);
        }

        private void градиентToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Filter filter = new GradFilter(MorphMask);
            backgroundWorker1.RunWorkerAsync(filter);
        }

        private void медианныйToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Filter filter = new MedianFilter();
            backgroundWorker1.RunWorkerAsync(filter);
        }


        private void настройкаToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (MorphEditForm != null)
            {
                MorphEditForm.Close();
                MorphEditForm = null;
            }
            else
            {
                MorphEditForm = new EditForm();
                MorphEditForm.FormClosed += (s, o) => {

                    if(MorphEditForm.OK)
                    {
                        MorphMask = MorphEditForm.Mask;
                    }

                    MorphEditForm = null;
                };
                
                MorphEditForm.Render(3, MorphMask);
                MorphEditForm.Show();
            }
        }

        #endregion

        #region Правка

        private void обновитьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (SourceImage == null)
                return;

            before.Push(Image);
            after.Clear();

            Image = SourceImage;
            pictureBox2.Image = Image;

            назадToolStripMenuItem.Enabled = true;
            впередToolStripMenuItem.Enabled = false;
        }

        private void назадToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (Image == null)
                return;

            var current = before.Pop();
            after.Push(Image);
            Image = current;

            впередToolStripMenuItem.Enabled = true;
            if (before.Count == 0)
                назадToolStripMenuItem.Enabled = false;

            pictureBox2.Image = Image;
        }

        private void впередToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (Image == null)
                return;

            var current = after.Pop();
            before.Push(Image);
            Image = current;

            назадToolStripMenuItem.Enabled = true;
            if (after.Count == 0)
                впередToolStripMenuItem.Enabled = false;

            pictureBox2.Image = Image;
        }

        private void поворотВправоToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Filter filter = new RotationFilter(Math.PI / 2);
            backgroundWorker1.RunWorkerAsync(filter);
        }

        private void поворотВлевоToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Filter filter = new RotationFilter(-Math.PI / 2);
            backgroundWorker1.RunWorkerAsync(filter);
        }

        private void зеркальноПоВертикалиToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Filter filter = new ReflectionFilter(true);
            backgroundWorker1.RunWorkerAsync(filter);
        }

        private void зеркальноПоГоризонталиToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Filter filter = new ReflectionFilter(false);
            backgroundWorker1.RunWorkerAsync(filter);
        }

        #endregion

        #endregion

        #region Buttons

        private void button1_Click(object sender, EventArgs e)
        {
            backgroundWorker1.CancelAsync();
        }

        #endregion         

        #region BackgroundWorker

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            if (SourceImage == null)
                return;
            if (Image == null)
                Image = SourceImage;

            Bitmap newImage;
            if (e.Argument is Filter[])
            {
                newImage = Filter.ProcessImage(SourceImage, backgroundWorker1, (Filter[])e.Argument);
            }
            else
            {
                newImage = ((Filter)e.Argument).ProcessImage(Image, backgroundWorker1);
            }

            if (newImage != null)
            {
                before.Push(Image);
                after.Clear();
                Image = newImage;
            }
        }

        private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (!e.Cancelled)
            {
                pictureBox2.Image = Image;

                назадToolStripMenuItem.Enabled = true;
                впередToolStripMenuItem.Enabled = false;
            }
         
            progressBar1.Value = 0;
        }

        private void backgroundWorker1_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            progressBar1.Value = e.ProgressPercentage;
        }






        #endregion

    }
}
