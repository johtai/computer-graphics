using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Numerics;

namespace Laba5
{
    public partial class Task3 : Form
    {
        Bitmap bmp;
        Graphics g;
        List<Point> list = new List<Point>();
        List<Point> listCenter = new List<Point>();
        List<Rectangle> listEllipse = new List<Rectangle>();
        int index;
        bool flag = false;
        public Task3()
        {
            InitializeComponent();
            bmp = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            pictureBox1.Image = bmp;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            g = Graphics.FromImage(pictureBox1.Image);
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Bezier();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            ClearPictureBox();
            list.Clear();
            listCenter.Clear();
            listEllipse.Clear();
        }

        private void pictureBox1_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                if (!flag)
                {
                    g.FillEllipse(Brushes.Red, e.X - 5, e.Y - 5, 10, 10);
                    list.Add(new Point(e.X, e.Y));
                    listEllipse.Add(new Rectangle(e.X - 5, e.Y - 5, 10, 10));
                    if (list.Count >= 4)
                        Redraw();
                    pictureBox1.Image = bmp;
                }
            }
            else if (e.Button == MouseButtons.Right)
            {
                for (int i = 0; i < listEllipse.Count; ++i)
                {
                    if (e.Y >= listEllipse[i].Top && e.Y <= (listEllipse[i].Top + listEllipse[i].Height)
                        && e.X >= listEllipse[i].Left && e.X <= (listEllipse[i].Left + listEllipse[i].Width))
                    {
                        list.RemoveAt(i);
                        listEllipse.RemoveAt(i);
                        break;
                    }
                }
                Redraw();
                pictureBox1.Image = bmp;
            }
        }

        private void pictureBox1_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                for (int i = 0; i < listEllipse.Count; ++i)
                {
                    if (e.Y >= listEllipse[i].Top && e.Y <= (listEllipse[i].Top + listEllipse[i].Height)
                        && e.X >= listEllipse[i].Left && e.X <= (listEllipse[i].Left + listEllipse[i].Width))
                    {
                        index = i;
                        flag = true;
                        break;
                    }
                }
            }
        }

        private void pictureBox1_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                if (flag)
                {
                    flag = false;
                    list[index] = new Point(e.X, e.Y);
                    listEllipse[index] = new Rectangle(e.X - 3, e.Y - 3, 6, 6);
                    Redraw();
                }
            }
        }

        private void ClearPictureBox()
        {
            g.Clear(pictureBox1.BackColor);
            pictureBox1.Image = bmp;
        }

        private void Bezier()
        {
            if (list.Count >= 4)
            {
                listCenter.Clear();
                for (int i = 2; i < list.Count; i += 2)
                {
                    var pen = new Pen(Color.Green, 3);
                    g.DrawLine(pen, list[i], (i + 1 < list.Count) ? list[i + 1] : list[i]);
                    listCenter.Add(new Point((list[i].X + ((i + 1 < list.Count) ? list[i + 1].X : list[i].X)) / 2,
                                             (list[i].Y + ((i + 1 < list.Count) ? list[i + 1].Y : list[i].Y)) / 2));
                    pen.Dispose();
                }

                Point p1 = list[0];
                Point p2, p3, p4;
                for (int i = 0; i < listCenter.Count; ++i)
                {
                    p2 = list[2 * i + 1];
                    p3 = (2 * i + 2 < list.Count) ? list[2 * i + 2] : list[2 * i + 1];
                    p4 = listCenter[i];

                    for (double t = 0; t < 1; t += 0.01)
                    {
                        if (p4 == listCenter.Last())
                            p4 = list.Last();
                        DrawLine(p1, p2, p3, p4, t);
                    }
                    p1 = p4;
                }
                pictureBox1.Image = bmp;
            }
            else
            {
                MessageBox.Show("Количество опорных точек должно быть не меньше 4");
            }
        }

        private void DrawLine(Point p1, Point p2, Point p3, Point p4, double t)
        {
            double[,] points = new double[,] { { p1.X, p2.X, p3.X, p4.X },
                                               { p1.Y, p2.Y, p3.Y, p4.Y } };
            double[,] tMatrix = new double[,] { { 1 },
                                                { t },
                                                { t * t },
                                                { t * t * t } };
            double[,] tMatrixNew = new double[,] { { 1 },
                                                   { t + 0.01 },
                                                   { (t + 0.01) * (t + 0.01) },
                                                   { (t + 0.01) * (t + 0.01) * (t + 0.01) } };
            double[,] matr = new double[,] { { 1, -3, 3, -1 },
                                             { 0, 3, -6, 3 },
                                             { 0, 0, 3, -3 },
                                             { 0, 0, 0, 1 } };
            matr = MatrixMultiplication(points, matr);
            double[,] res = MatrixMultiplication(matr, tMatrix);
            double[,] resNew = MatrixMultiplication(matr, tMatrixNew);

            var pen = new Pen(Color.Blue, 3);
            g.DrawLine(pen, new Point((int)res[0, 0], (int)res[1, 0]), new Point((int)resNew[0, 0], (int)resNew[1, 0]));
            pen.Dispose();
        }

        private double[,] MatrixMultiplication(double[,] m1, double[,] m2)
        {
            double[,] res = new double[m1.GetLength(0), m2.GetLength(1)];
            for (int i = 0; i < m1.GetLength(0); ++i)
            {
                for (int j = 0; j < m2.GetLength(1); ++j)
                {
                    for (int k = 0; k < m2.GetLength(0); k++)
                    {
                        res[i, j] += m1[i, k] * m2[k, j];
                    }
                }
            }
            return res;
        }

        private void Redraw()
        {
            ClearPictureBox();
            foreach (Point p in list)
            {
                g.FillEllipse(Brushes.Red, p.X - 5, p.Y - 5, 10, 10);
            }
            Bezier();
        }
    }
}
