using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp2
{
    public partial class EditForm : Form
    {
        Panel[,] buttons;
        bool[,] mask;

        public bool[,] Mask => mask;

        public bool OK { get; set; } = false; 

        public EditForm()
        {
            InitializeComponent();
        }

        public void Render(int n, bool[,] mask)
        {
            textBox1.Text = n + "";
            var m = n;

            this.mask = mask;
            DrawButtons(n);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            var n = Convert.ToInt32(textBox1.Text);
            var m = n;

            bool[,] _m = new bool[n, n];
            for(int i = 0; i < n; ++i)
            {
                for(int j = 0; j< m; ++j)
                {
                    _m[i,j] = true;
                }
            }

            mask = _m;
            DrawButtons(n);

        }

        void DrawButtons(int n)
        {
            var m = n;
            buttons = new Panel[n, m];
            panel1.Controls.Clear();


            var xlen = panel1.Width / n;
            var ylen = panel1.Height / m;

            for (int j = 0; j < m; ++j)
            {
                for (int i = 0; i < n; ++i)
                {
                    Panel button = new Panel();

                    if (mask[i, j])
                        button.BackColor = Color.Black;
                    else
                        button.BackColor = Color.White;

                    button.BorderStyle = BorderStyle.FixedSingle;
                    button.Size = new Size(xlen, ylen);
                    button.Location = new Point(xlen * i, ylen * j);
                    button.Tag = new Point(i, j);

                    button.Click += (s, e) =>
                    {
                        var target = s as Panel;
                        var point = target.Tag as Point?;

                        var i1 = point?.X ?? 0;
                        var j1 = point?.Y ?? 0;

                        mask[i1, j1] = !mask[i1, j1];

                        if (mask[i1, j1])
                        {
                            target.BackColor = Color.Black;
                        }
                        else
                        {
                            target.BackColor = Color.White;
                        }
                    };


                    panel1.Controls.Add(button);
                    buttons[i, j] = button;
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            OK = true;
            this.Close();
        }
    }
}
