using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace Lab4
{
    public partial class Form1 : Form
    {
        enum Transormations { Shift, Rotation, RotationCenter, Zoom, ZoomCenter};
        Bitmap bmp;
        Graphics g;
        bool isReady = false; // флаг готовности полигона
        double[,] transformationMatrix; // матрица преобразования
        List<Point> list = new List<Point>(); // список точек для полигона
        Point myPoint; // Точка, относительно которой происходит поворот
        // Для хранения пересекающейся линии
        Point lineStart;
        Point lineEnd;
        bool isSecondLineReady = true;
        bool drPoint = true;

        public Form1()
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

        private double[,] dotRotate(double[,] dotmatr1, double[,] matr2)
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

        private void rotate_around_point(double angle, double radians, double cos, double sin, Point mypoint)
        {
            double[,] translate = new double[,]
            {
                    {1,0,0 },
                    {0,1,0 },
                    {-mypoint.X, -mypoint.Y, 1 }
            };
            double[,] angelfi = new double[,]
            {
                    {cos, sin, 0 },
                    {-sin, cos, 0 },
                    {0, 0, 1 }
            };
            double[,] translateBack = new double[,]
            {
                    {1, 0, 0 }, {0, 1, 0 },{mypoint.X, mypoint.Y, 1 }
            };

            double[,] resDP = new double[,]
            {

                {cos, sin, 0 },
                { -sin, cos, 0},
                {-myPoint.X * cos - myPoint.Y * sin + myPoint.X, -myPoint.X * sin + myPoint.Y * cos - myPoint.Y, 1 }
            };

           


            for (int i = 0; i < list.Count; i++)
            {

                double[,] pointMartr = { { list[i].X, list[i].Y, 1 } };

                //pointMartr = dotRotate(pointMartr, translate);
                //pointMartr = dotRotate(pointMartr, angelfi);
                //pointMartr = dotRotate(pointMartr, translateBack);



                list[i] = new Point((int)pointMartr[0, 0], (int)pointMartr[0, 1]);
                pointMartr = dotRotate(pointMartr, resDP);
            }
        }

        // Добавляет очередной элемент к полигону при нажатии мышкой
        private void pictureBox1_MouseDown(object sender, MouseEventArgs e)
        {
            if (!isReady)
            {
                list.Add(new Point(e.X, e.Y));
                if (list.Count() == 3)
                    chainButton.Enabled = true;

                if (list.Count() == 1)
                {
                    button2.Enabled = true;
                    ((Bitmap)pictureBox1.Image).SetPixel(e.X, e.Y, Color.Red);
                }
                else
                {
                    var pen = new Pen(Color.Red, 1);
                    g.DrawLine(pen, list[list.Count() - 2], list.Last());
                    pen.Dispose();
                }
                pictureBox1.Image = pictureBox1.Image;
            }
            else if (!isSecondLineReady)
            {
                if (!isSecondLineReady)
                {
                    if (lineStart.IsEmpty)
                    {
                        lineStart = new Point(e.X, e.Y);
                    }
                    else
                    {
                        lineEnd = new Point(e.X, e.Y);
                        g.DrawLine(new Pen(Color.Blue, 1), lineStart, lineEnd);
                        isSecondLineReady = true;

                        // Проверка на пересечение
                        foreach (var intersection in GetIntersections(lineStart, lineEnd))
                        {
                            g.FillEllipse(Brushes.Green, intersection.X-5 , intersection.Y-5, 10, 10);
                        }

                        lineStart = Point.Empty; 
                        lineEnd = Point.Empty; 
                    }
                }
            }
            else
            {
                if (!drPoint)
                {
                    myPoint = new Point(e.X, e.Y);
                    g.FillEllipse(Brushes.Yellow, myPoint.X - 5, myPoint.Y - 5, 10, 10); // покрасим точку в желтый
                    pictureBox1.Image = pictureBox1.Image;
                    drPoint = true;
                }
            }
            pictureBox1.Image = pictureBox1.Image;
        }



        private void RefreshElements()
        {
            // Происходит активация элементов, если в комбобоксе выбрано "Смещение"
            label2.Enabled = textBox1.Enabled =
                textBox2.Enabled = dxLabel.Enabled =
                dyLabel.Enabled = comboBox1.SelectedIndex == (int)Transormations.Shift;
            // Происходит активация элементов, если в комбобоксе выбрано "Поворот вокруг заданной точки" или "Поворот вокруг центра"
            label1.Enabled = textBox3.Enabled = degreesLabel.Enabled = 
                comboBox1.SelectedIndex == (int)Transormations.Rotation || comboBox1.SelectedIndex == (int)Transormations.RotationCenter;
            // Происходит активация элементов, если в комбобоксе выбрано "Масштабирование относительно точки" или "Масштабирование относительно центра"
            label3.Enabled = textBox4.Enabled =
                textBox5.Enabled = XLabel.Enabled = YLabel.Enabled =
                comboBox1.SelectedIndex == (int)Transormations.Zoom || comboBox1.SelectedIndex == (int)Transormations.ZoomCenter;
        }

        // Очистка сцены (удаление всех полигонов)
        private void Clear()
        {
            g.Clear(pictureBox1.BackColor);
            list.Clear();
            lineStart = Point.Empty;
            lineEnd = Point.Empty;
            isSecondLineReady = true;
            drPoint = true;

            comboBox1.SelectedIndex = 0;
            RefreshElements();
            button2.Enabled = chainButton.Enabled = false;
            pictureBox1.Image = pictureBox1.Image;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            isReady = false;
            Clear();
        }

        private void drawSecondLineButton_Click(object sender, EventArgs e)
        {
            isReady = true;
            drPoint = true;
            isSecondLineReady = false;
        }


        private void ChooseTransformation()
        {
            double X;
            double Y;
            double angle;
            double XScale;
            double YScale;

            switch (comboBox1.SelectedIndex)
            {
                case (int)Transormations.Shift:
                    double dX = System.Convert.ToDouble(textBox1.Text);
                    double dY = -System.Convert.ToDouble(textBox2.Text);
                    transformationMatrix = new double[,] { 
                        { 1.0, 0, 0 }, 
                        { 0, 1.0, 0 }, 
                        { dX, dY, 1.0 } };
                    break;
                case (int)Transormations.Rotation:
                    X = System.Convert.ToDouble(myPoint.X);
                    Y = -System.Convert.ToDouble(myPoint.Y);
                    angle = Math.PI * System.Convert.ToDouble(textBox3.Text) / 180.0;
                    transformationMatrix = new double[,] { 
                        { Math.Cos(angle), Math.Sin(angle), 0 }, 
                        { -Math.Sin(angle), Math.Cos(angle), 0 }, 
                        { -X*Math.Cos(angle) - Y*Math.Sin(angle) + X, 
                            -X*Math.Sin(angle)+Y*Math.Cos(angle)-Y, 1.0 } };
                    break;
                case (int)Transormations.RotationCenter:
                    myPoint = Centroid();
                    X = myPoint.X;
                    Y = myPoint.Y;
                    angle = Math.PI * System.Convert.ToDouble(textBox3.Text) / 180.0;
                    transformationMatrix = new double[,] {
                        { Math.Cos(angle), Math.Sin(angle), 0 },
                        { -Math.Sin(angle), Math.Cos(angle), 0 },
                        { -X*Math.Cos(angle) - Y*Math.Sin(angle) + X,
                            -X*Math.Sin(angle)+Y*Math.Cos(angle)-Y, 1.0 } };
                    break;
                case (int)Transormations.Zoom:
                    X = System.Convert.ToDouble(myPoint.X);
                    Y = -System.Convert.ToDouble(myPoint.Y);
                    XScale = System.Convert.ToDouble(textBox4.Text) / 100.0;
                    YScale = System.Convert.ToDouble(textBox5.Text) / 100.0;
                    transformationMatrix = new double[,] {
                        { XScale, 0, 0 },
                        { 0, YScale, 0 },
                        { (1 - XScale)* X, -(1 - YScale)* Y, 1.0 } };
                    break;
                case (int)Transormations.ZoomCenter:
                    myPoint = Centroid();
                    X = myPoint.X;
                    Y = myPoint.Y;
                    XScale = System.Convert.ToDouble(textBox4.Text) / 100.0;
                    YScale = System.Convert.ToDouble(textBox5.Text) / 100.0;
                    transformationMatrix = new double[,] {
                        { XScale, 0, 0 },
                        { 0, YScale, 0 },
                        { (1 - XScale)* X, -(1 - YScale)* Y, 1.0 } };
                    break;

                default:
                    break;
            }
        }

        // Для очищения экрана (холста)
        private void ClearScreen()
        {
            g.Clear(pictureBox1.BackColor);
            pictureBox1.Image = pictureBox1.Image;
        }

        // Перемножение матриц
        private double[,] MatrixMultiplication(double[,] m1, double[,] m2)
        {
            double[,] res = new double[m1.GetLength(0), m2.GetLength(1)];

            for (int i = 0; i < m1.GetLength(0); ++i)
                for (int j = 0; j < m2.GetLength(1); ++j)
                    for (int k = 0; k < m2.GetLength(0); k++)
                    {
                        res[i, j] += m1[i, k] * m2[k, j];
                    }

            return res;
        }

        // Возвращает геометрический центр полигона
        private Point Centroid ()
        {
            double S = 0;
            double X = 0;
            double Y = 0;
            int num;

            for (int i = 0; i < list.Count - 1; ++i)
            {
                num = -list[i].X * list[i + 1].Y + list[i + 1].X * list[i].Y;
                S += num;
                X += (list[i].X + list[i + 1].X) * num;
                Y += -(list[i].Y + list[i + 1].Y) * num;
            }
            S *= 0.5;
            X /= 6*S;
            Y /= 6*S;
            return new Point((int)X, (int)Y);
        }

        // Применяет преобразование с использованием матриц
        private void button2_Click(object sender, EventArgs e)
        {
            if (comboBox1.Text == "Поворот вокруг точки")
            {
                double angle = double.Parse(textBox3.Text);
                double radians = angle * Math.PI / 180;
                double cos = Math.Cos(radians);
                double sin = Math.Sin(radians);

                rotate_around_point(angle, radians, cos, sin, myPoint);

                pictureBox1.Invalidate();
            }
            else
            {
                ChooseTransformation();

                List<Point> newList = new List<Point>();
                foreach (Point p in list)
                {
                    double[,] point = new double[,] { { p.X * 1.0, p.Y * 1.0, 1.0 } };
                    double[,] res = MatrixMultiplication(point, transformationMatrix);
                    newList.Add(new Point(Convert.ToInt32(Math.Round(res[0, 0])), Convert.ToInt32(Math.Round(res[0, 1]))));
                }
                ClearScreen();

                Point prevPoint = newList.First();
                foreach (Point point in newList)
                {
                    if (point != prevPoint)
                    {
                        Pen pen = new Pen(Color.Red, 1);
                        g.DrawLine(pen, prevPoint, point);

                        prevPoint = point;
                        pen.Dispose();
                    }
                }

                list = newList;
                pictureBox1.Image = bmp;
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            RefreshElements();
        }

        // Замыкает полигон
        private void chainButton_Click(object sender, EventArgs e)
        {
            g.DrawLine(new Pen(Color.Red, 1), list.Last(), list.First());
            list.Add(list.First());
            pictureBox1.Image = pictureBox1.Image;
            isReady = true;
            chainButton.Enabled = false;
        }
        
        // Вычисление всех пересечений между линией и ребрами полигона
        private List<Point> GetIntersections(Point p1, Point p2)
        {
            List<Point> intersections = new List<Point>();

            for (int i = 0; i < list.Count; i++)
            {
                Point p3 = list[i];
                Point p4 = list[(i + 1) % list.Count]; // Используем модуль для замыкания полигона

                float denom = (p4.Y - p3.Y) * (p2.X - p1.X) - (p4.X - p3.X) * (p2.Y - p1.Y);
                if (denom == 0) continue; // Параллельные линии

                float ua = ((p4.X - p3.X) * (p1.Y - p3.Y) - (p4.Y - p3.Y) * (p1.X - p3.X)) / denom;
                float ub = ((p2.X - p1.X) * (p1.Y - p3.Y) - (p2.Y - p1.Y) * (p1.X - p3.X)) / denom;

                if (ua >= 0 && ua <= 1 && ub >= 0 && ub <= 1)
                {
                    float intersectionX = p1.X + ua * (p2.X - p1.X);
                    float intersectionY = p1.Y + ua * (p2.Y - p1.Y);
                    intersections.Add(new Point((int)intersectionX, (int)intersectionY));
                }
            }

            return intersections;
        }

        private bool IsPointInPolygon(Point[] polygon, Point point)
        {
            int n = polygon.Length;
            bool inside = false;

            // Индекс предыдущей точки
            int j = n - 1;

            for (int i = 0; i < n; i++)
            {
                // Проверка пересечения ребра
                if ((polygon[i].Y > point.Y) != (polygon[j].Y > point.Y) &&
                    (point.X < (polygon[j].X - polygon[i].X) * (point.Y - polygon[i].Y) / (polygon[j].Y - polygon[i].Y) + polygon[i].X))
                {
                    inside = !inside; // Если пересечение, меняем состояние
                }
                j = i; // Запоминаем текущую точку как предыдущую
            }

            return inside;
        }


        private void checkButton_Click(object sender, EventArgs e)
        {
            if (isReady && myPoint != Point.Empty)
            {
                // Проверка принадлежности к невыпуклому полигону
                if (IsPointInPolygon(list.ToArray(), myPoint))
                {
                    MessageBox.Show("Точка принадлежит полигону");
                }
                else
                {
                    MessageBox.Show("Точка не принадлежит полигону");
                }
                drPoint = true;

            }
            else
            {
                MessageBox.Show("Пожалуйста, сначала нарисуйте полигон и выберите точку.", "Ошибка");
            }
        }

        private void drawPoint_Click(object sender, EventArgs e)
        {
            isReady = true; 
            drPoint = false;
        }

        private void classifyButton_Click(object sender, EventArgs e)
        {
            if (isReady && isSecondLineReady)
            {
                // Классификация положения точки  относительно отрезка
                Point A = list[list.Count - 1]; 
                Point B = list[list.Count - 2];  
                Point P = myPoint; // Точка для классификации

                string position = ClassifyPosition(A, B, P);
                MessageBox.Show($"Положение точки относительно отрезка: {position}");
            }
            else
            {
                MessageBox.Show("Пожалуйста, убедитесь, что полигон завершен и линия готова.");
            }
        }

        private string ClassifyPosition(Point A, Point B, Point P)
        {
            double D = (B.X - A.X) * (P.Y - A.Y) - (B.Y - A.Y) * (P.X - A.X);

            if (D > 0)
            {
                return "Справа от отрезка";
            }
            else if (D < 0)
            {
                return "Слева от отрезка";
            }
            else
            {
                return "На отрезке";
            }
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }
    }
}