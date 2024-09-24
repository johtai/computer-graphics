using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using FastBitmaps;

namespace Laba3
{
    public partial class Task3 : Form
    {
        Point[] points = new Point[3];
        Color[] colors = new Color[3];

        int point = 0;

        bool isPaint = true;
        bool isTracking = false;

        private Graphics g;
        
        Pen penLines = new Pen(Color.Black, 1f);
        Pen pen = new Pen(Color.Black, 3f);

        Point point1 = new Point(0, 0);
        Point point2 = new Point(0, 0);

        Bitmap bitmap;

        public Task3()
        {
            InitializeComponent();
            g = pictureBox1.CreateGraphics();
            g.Clear(Color.White);
            bitmap = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            pictureBox1.Image = bitmap;
        }

        private void button_point_Click(object sender, EventArgs e)
        {
            Button button = (sender as Button);
            point = Convert.ToInt32(button.Name.Substring(button.Name.Length - 1)) - 1;

            if (colorDialog1.ShowDialog() == DialogResult.Cancel)
                return;

            button.BackColor = colorDialog1.Color;
            colors[point] = colorDialog1.Color;
            isPaint = false;
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            var mouse = e as MouseEventArgs;
            if (isPaint)
            {
                isTracking = !isTracking;
                point1 = new Point(mouse.X, mouse.Y);
            }
            else
            {
                Pen penC = new Pen(colors[point], 3f);
                g.DrawRectangle(penC, mouse.X, mouse.Y, 3, 3);
                points[point] = new Point(mouse.X, mouse.Y);
            }
        }

        (Point[], Color[]) SortPoints(Point[] sortPoints, Color[] sortColors)
        {
            Point[] newPoints = sortPoints;
            Color[] newColors = sortColors;

            for (int i = 0; i < sortPoints.Length - 1; i++)
            {
                if (sortPoints[i + 1].Y < sortPoints[i].Y)
                {
                    Point temp = sortPoints[i];
                    Color tempC = sortColors[i];
                    newPoints[i] = sortPoints[i + 1];
                    newColors[i] = sortColors[i + 1];
                    newPoints[i + 1] = temp;
                    newColors[i + 1] = tempC;
                }
            }

            return (newPoints, newColors);
        }

        private void button_gradient_Click(object sender, EventArgs e)
        {
            Point[] newPoints;
            Color[] newColors;

            // Упорядочиваем точки по y (комментарий на этот случай закомментирован)
            (newPoints, newColors) = SortPoints(points, colors);

            // Определяем функции для вычисления x-координат на основе y
            Func<double, double> func1 = y => (y - newPoints[0].Y) / (newPoints[1].Y - newPoints[0].Y) * (newPoints[1].X - newPoints[0].X) + newPoints[0].X; // Левый край от v0 до v1
            Func<double, double> func2 = y => (y - newPoints[0].Y) / (newPoints[2].Y - newPoints[0].Y) * (newPoints[2].X - newPoints[0].X) + newPoints[0].X; // Левый край от v0 до v2
            Func<double, double> func3 = y => (y - newPoints[1].Y) / (newPoints[2].Y - newPoints[1].Y) * (newPoints[2].X - newPoints[1].X) + newPoints[1].X; // Правый край от v1 до v2

            using (var fastBitmap = new FastBitmap(bitmap))
            {
                int yMin = newPoints[0].Y;
                int yMax = newPoints[1].Y;

                Color color0 = newColors[0];
                Color color1 = newColors[1];
                Color color2 = newColors[2];

                int x1, x2; // x-координаты пересечений
                int linR1, linG1, linB1, linR2, linG2, linB2;

                // Растеризуем треугольник от v0 до v1
                for (int i = newPoints[0].Y; i <= newPoints[1].Y; i++)
                {
                    // Получаем x-координаты пересечений
                    x1 = (int)func1(i);
                    x2 = (int)func2(i);

                    // Пересчитываем минимальные и максимальные y для текущей строки
                    yMin = newPoints[0].Y;
                    yMax = newPoints[1].Y;

                    // Интерполируем цвета по y для левого края
                    linR1 = LinY(i, yMin, yMax, color0.R, color1.R);
                    linG1 = LinY(i, yMin, yMax, color0.G, color1.G);
                    linB1 = LinY(i, yMin, yMax, color0.B, color1.B);

                    // Интерполируем цвета по y для правого края
                    yMin = newPoints[0].Y;
                    yMax = newPoints[2].Y;
                    linR2 = LinY(i, yMin, yMax, color0.R, color2.R);
                    linG2 = LinY(i, yMin, yMax, color0.G, color2.G);
                    linB2 = LinY(i, yMin, yMax, color0.B, color2.B);

                    // Заполняем пиксели на границах
                    fastBitmap[x1, i] = Color.FromArgb(linR1, linG1, linB1);
                    fastBitmap[x2, i] = Color.FromArgb(linR2, linG2, linB2);

                    // Находим диапазон x между пересечениями
                    int minX = Math.Min(x1, x2);
                    int maxX = Math.Max(x1, x2);

                    // Заполняем цвета пикселей между x1 и x2
                    if (minX == x1)
                    {
                        for (int j = minX; j < maxX; j++)
                        {
                            // Интерполируем цвета для промежуточных пикселей
                            int linR3 = LinY(j, minX, maxX, linR1, linR2);
                            int linG3 = LinY(j, minX, maxX, linG1, linG2);
                            int linB3 = LinY(j, minX, maxX, linB1, linB2);
                            fastBitmap[j, i] = Color.FromArgb(linR3, linG3, linB3);
                        }
                    }
                    else
                    {
                        for (int j = minX; j < maxX; j++)
                        {
                            int linR3 = LinY(j, minX, maxX, linR2, linR1);
                            int linG3 = LinY(j, minX, maxX, linG2, linG1);
                            int linB3 = LinY(j, minX, maxX, linB2, linB1);
                            fastBitmap[j, i] = Color.FromArgb(linR3, linG3, linB3);
                        }
                    }
                }

                // Растеризация от v1 до v2
                for (int i = newPoints[1].Y; i <= newPoints[2].Y; i++)
                {
                    yMin = newPoints[0].Y;
                    yMax = newPoints[2].Y;
                    x1 = (int)func3(i); // Пересечение v1-v2
                    x2 = (int)func2(i); // Пересечение v0-v2

                    // Интерполируем цвета по y для левого края
                    linR1 = LinY(i, yMin, yMax, color0.R, color2.R);
                    linG1 = LinY(i, yMin, yMax, color0.G, color2.G);
                    linB1 = LinY(i, yMin, yMax, color0.B, color2.B);

                    // Интерполируем цвета по y для правого края
                    yMin = newPoints[1].Y;
                    yMax = newPoints[2].Y;
                    linR2 = LinY(i, yMin, yMax, color1.R, color2.R);
                    linG2 = LinY(i, yMin, yMax, color1.G, color2.G);
                    linB2 = LinY(i, yMin, yMax, color1.B, color2.B);

                    // Заполняем пиксели на границах
                    fastBitmap[x2, i] = Color.FromArgb(linR1, linG1, linB1);
                    fastBitmap[x1, i] = Color.FromArgb(linR2, linG2, linB2);

                    // Определяем диапазон x между пересечениями
                    int minX = Math.Min(x1, x2);
                    int maxX = Math.Max(x1, x2);

                    // Заполняем цвета пикселей между x1 и x2
                    if (minX == x1)
                    {
                        for (int j = minX; j < maxX; j++)
                        {
                            int linR3 = LinY(j, minX, maxX, linR2, linR1);
                            int linG3 = LinY(j, minX, maxX, linG2, linG1);
                            int linB3 = LinY(j, minX, maxX, linB2, linB1);
                            fastBitmap[j, i] = Color.FromArgb(linR3, linG3, linB3);
                        }
                    }
                    else
                    {
                        for (int j = minX; j < maxX; j++)
                        {
                            int linR3 = LinY(j, minX, maxX, linR1, linR2);
                            int linG3 = LinY(j, minX, maxX, linG1, linG2);
                            int linB3 = LinY(j, minX, maxX, linB1, linB2);
                            fastBitmap[j, i] = Color.FromArgb(linR3, linG3, linB3);
                        }
                    }
                }
            }

            pictureBox1.Image = bitmap;
        }

        private int LinX(int x, int a, int b, int c1, int c2)
        {
            if (c2 > c1)
            {
                return c1 + (x - a) * (c2 - c1) / (b - a + 1);
            }
            else
            {
                return c1 - (x - a) * (c1 - c2) / (b - a + 1);
            }
        }
        private int LinY(int y, int a, int b, int c1, int c2)
        {
            if (c2 > c1)
            {
                if (b - a == 0)
                {
                    b++;
                }
                return c1 + (y - a) * (c2 - c1) / (b - a);
            }
            else
            {
                if (b - a == 0)
                {
                    b++;
                }
                return c1 - (y - a) * (c1 - c2) / (b - a);
            }
        }

        private float LinX2(float x, float a, float b)
        {
            return (x - a) / (b - a) * pictureBox1.Width;
        }

        private float LinY2(float y, float min, float max)
        {
            return (y - max) / (min - max) * pictureBox1.Height;
        }

        private void button_pen_color_Click(object sender, EventArgs e)
        {
            Button button = (sender as Button);
            if (colorDialog1.ShowDialog() == DialogResult.Cancel)
                return;
            pen.Color = colorDialog1.Color;
            button.BackColor = colorDialog1.Color;
        }

        private void pictureBox1_MouseEnter(object sender, EventArgs e)
        {
            pictureBox1.Cursor = Cursors.Hand;
        }

        private void pictureBox1_MouseLeave(object sender, EventArgs e)
        {
            pictureBox1.Cursor = Cursors.Default;
            isTracking = false;
        }

        private void pictureBox1_MouseMove(object sender, MouseEventArgs e)
        {
            point2 = point1;
            point1 = new Point(e.X, e.Y);
            if (isTracking)
            {
                g.DrawLine(pen, point1.X, point1.Y, point2.X, point2.Y);
            }
        }

        private void trackBar1_ValueChanged(object sender, EventArgs e)
        {
            pen.Width = (sender as TrackBar).Value;
        }

        private void button_choose_pen_Click(object sender, EventArgs e)
        {
            isPaint = true;
        }

        private void button_choose_cleaner_Click(object sender, EventArgs e)
        {
            isPaint = true;
            pen.Color = Color.White;
        }

        private void button_clean_Click(object sender, EventArgs e)
        {
            Graphics g = Graphics.FromImage(bitmap);
            g.Clear(Color.White);
            pictureBox1.Refresh();
            pictureBox1.Invalidate();
        }

        private void Task2_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.UserClosing)
            {
                e.Cancel = true;
                Hide();
            }
        }

        private void Task2_Load(object sender, EventArgs e)
        {

        }
    }
}
