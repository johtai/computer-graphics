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
                    for (int k = 0; k < matr2.GetLength(1); ++k)
                    {
                        res[i, j] += dotmatr1[i, k] * matr2[k, j];
                    }

            return res;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (comboBox1.Text == "Поворот вокруг точки") 
            {
                double angleInRadians = double.Parse(gradBox1.Text) * Math.PI / 180;
                double[,] matr = { {Math.Cos(angleInRadians), Math.Sin(angleInRadians), 0},
                                    {-Math.Sin(angleInRadians), Math.Cos(angleInRadians), 0 },
                                    {-MyPoint.X * Math.Cos(angleInRadians) - MyPoint.Y * Math.Sin(angleInRadians) + MyPoint.X, -MyPoint.X * Math.Sin(angleInRadians) + MyPoint.Y * Math.Cos(angleInRadians) - MyPoint.Y, 1 }};




                for (int i = 0; i < points.Count(); i++)
                {
                    double[,] p = { { points[i].X }, { points[i].Y }, {1} };
                    double [,] abc = dotRotate(matr, p);

                    points[i] = new Point((int)abc[0, 0], (int)abc[1,0]);

                }
            }
            MessageBox.Show( "GradBox: "+ gradBox1.Text + " X:"+ MyPoint.X +" Y: " + MyPoint.Y);
            Invalidate();
            pictureBox1.Refresh();
        }
    }
}
