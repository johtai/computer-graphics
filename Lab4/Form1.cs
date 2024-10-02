using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Lab4
{
    public partial class Form1 : Form
    {
        private List<Point> points = new List<Point>();
        private Point MyPoint;
        private bool isPolygonClosed = false;
        private bool refresh = false;


        public Form1()
        {
            InitializeComponent();
            pictureBox1.MouseClick += PictureBox1_MouseClick;
            pictureBox1.Paint += PictureBox1_DrawPolygon;
        }



        private void PictureBox1_MouseClick(object sender, MouseEventArgs e)
        {
            if (isPolygonClosed)
            {
                MyPoint = e.Location;
                return;
            }


            points.Add(e.Location);
            pictureBox1.Invalidate();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            points.Clear();
            isPolygonClosed = false;
            pictureBox1.Invalidate();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (points.Count > 2)
            {
                isPolygonClosed = true;
                pictureBox1.Invalidate();
            }

        }

        private void PictureBox1_DrawPolygon(object sender, PaintEventArgs e)
        {
            if (points.Count > 1)
            {
                e.Graphics.DrawLines(Pens.Black, points.ToArray());

                if (isPolygonClosed)
                {
                    e.Graphics.DrawLine(Pens.Black, points[points.Count - 1], points[0]);
                }
            }
        }


        private double [,] dotRotate(double[,] dotmatr1, double[,] matr2)
        {
            double[,] res = new double[dotmatr1.GetLength(0), matr2.GetLength(1)];
          
            for (int i = 0; i < dotmatr1.GetLength(0); ++i)
                for (int j = 0; j < matr2.GetLength(1); ++j)
                    for (int k = 0; k < matr2.GetLength(0); ++k)
                    {
                        res[i, j] += dotmatr1[i, k] * matr2[k, j];
                    }

            return res;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (comboBox1.Text == "Поворот вокруг точки") 
            {
                double angle = double.Parse(gradBox1.Text);
                double radians = angle * Math.PI / 180;
                double cos = Math.Cos(radians);
                double sin = Math.Sin(radians);



                double[,] translate = new double[,]
                {
                    {1,0,0 },
                    {0,1,0 },
                    {-MyPoint.X, -MyPoint.Y, 1 }
                };
                double[,] angelfi = new double[,]
                {
                    {cos, sin, 0 },
                    {-sin, cos, 0 },
                    {0, 0, 1 }
                };
                double[,] translateBack = new double[,]
                {
                    {1, 0, 0 }, {0, 1, 0 },{MyPoint.X, MyPoint.Y, 1 }
                };


                for (int i = 0; i < points.Count; i++) 
                {

                    double[,] pointMartr = { { points[i].X, points[i].Y, 1 } };

                    pointMartr = dotRotate(pointMartr, translate);
                    pointMartr = dotRotate(pointMartr, angelfi);
                    pointMartr = dotRotate(pointMartr, translateBack);

                    points[i] = new Point((int)pointMartr[0,0], (int)pointMartr[0,1]);
                }


            }
            //MessageBox.Show( "GradBox: "+ gradBox1.Text + " X:"+ MyPoint.X +" Y: " + MyPoint.Y);
            pictureBox1.Invalidate();
            //pictureBox1.Refresh();
        }
    }
}
