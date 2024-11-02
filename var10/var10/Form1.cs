using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography;
using System.Windows.Forms;
using System.Threading;

namespace var10
{
    //public class PointNew
    //{
    //    public int X;
    //    public int Y;
    //    public PointNew Next;

    //    public PointNew(int x, int y, PointNew next)
    //    {
    //        X = x;
    //        Y = y;
    //        Next = next;
    //    }
    //}

    public partial class Form1 : Form
    {
        private List<Point> points = new List<Point>();
        
        private List<Point> polygon1 = new List<Point>();
        private List<Point> polygon2 = new List<Point>();

        private List<Point> unionPolygon = new List<Point>();
        
        private bool isFirstPolygonDrawn = false;
        private bool isSecondPolygonDrawn = false;
        
        private Label coordinatesLabel;

        public Form1()
        {
            InitializeComponent();

            coordinatesLabel = new Label
            {
                Location = new System.Drawing.Point(5, 100),
                AutoSize = true,
                Font = new System.Drawing.Font("Arial", 12)
            };
            this.Controls.Add(coordinatesLabel);
        }

        private void MainForm_MouseMove(object sender, MouseEventArgs e)
        {
            // Обновление текста метки с координатами курсора
            coordinatesLabel.Text = $"{e.X}, {e.Y}";
        }

        private void drawingPanel_MouseClick(object sender, MouseEventArgs e)
        {
            points.Add(e.Location);
            drawingPanel.Invalidate();
        }

        private void btnDrawPolygon1_Click(object sender, EventArgs e)
        {
            if (points.Count >= 3)
            {
                polygon1 = ConvexHull(points);
                isFirstPolygonDrawn = true;
                points.Clear();
                drawingPanel.Invalidate();
            }
            else
            {
                MessageBox.Show("Add at least 3 points to create the first polygon.");
            }
        }

        private void btnDrawPolygon2_Click(object sender, EventArgs e)
        {
            if (points.Count >= 3)
            {
                polygon2 = ConvexHull(points);
                isSecondPolygonDrawn = true;
                points.Clear();
                drawingPanel.Invalidate();
            }
            else
            {
                MessageBox.Show("Add at least 3 points to create the second polygon.");
            }
        }

        private void btnUnionPolygons_Click(object sender, EventArgs e)
        {
            if (isFirstPolygonDrawn && isSecondPolygonDrawn)
            {
                unionPolygon = UnionConvexPolygons(polygon1, polygon2);
                drawingPanel.Invalidate();
            }
            else
            {
                MessageBox.Show("Draw both polygons before attempting to union them.");
            }
        }

        private List<Point> ConvexHull(List<Point> points)
        {
            //points = points.OrderBy(p => p.X).ThenBy(p => p.Y).ToList();
            Stack<Point> hull = new Stack<Point>();

            foreach (var point in points)
            {
                while (hull.Count >= 2 && CrossProduct(hull.ElementAt(1), hull.Peek(), point) <= 0)
                {
                    hull.Pop();
                }
                hull.Push(point);
            }

            int hullCount = hull.Count;
            for (int i = points.Count - 1; i >= 0; i--)
            {
                var point = points[i];
                while (hull.Count > hullCount && CrossProduct(hull.ElementAt(1), hull.Peek(), point) <= 0)
                {
                    hull.Pop();
                }
                hull.Push(point);
            }
            hull.Pop();
            return hull.Reverse().ToList();
        }

        private int CrossProduct(Point a, Point b, Point c)
        {
            return (b.X - a.X) * (c.Y - a.Y) - (b.Y - a.Y) * (c.X - a.X);
        }

        public static Point Intersection(Point p1, Point p2, Point p3, Point p4)
        {
            // Коэффициенты уравнений прямых: Ax + By = C
            float a1 = p2.Y - p1.Y;
            float b1 = p1.X - p2.X;
            float c1 = a1 * p1.X + b1 * p1.Y;

            float a2 = p4.Y - p3.Y;
            float b2 = p3.X - p4.X;
            float c2 = a2 * p3.X + b2 * p3.Y;

            // Определяем детерминант
            float determinant = a1 * b2 - a2 * b1;

            // Если детерминант равен нулю, отрезки параллельны
            if (determinant == 0)
            {
                return new Point(-1, -1);
            }

            // Найти точку пересечения
            float x = (b2 * c1 - b1 * c2) / determinant;
            float y = (a1 * c2 - a2 * c1) / determinant;
            Point intersection = new Point((int)x, (int)y);

            // Проверяем, находится ли точка пересечения на обоих отрезках
            if (IsPointOnLineSegment(intersection, p1, p2) && IsPointOnLineSegment(intersection, p3, p4))
            {
                return intersection;
            }

            //return intersection;
            return new Point(-1, -1); // Точка пересечения существует, но не принадлежит обоим отрезкам
        }

        private static bool IsPointOnLineSegment(Point point, Point lineStart, Point lineEnd)
        {
            return point.X >= Math.Min(lineStart.X, lineEnd.X) && point.X <= Math.Max(lineStart.X, lineEnd.X) &&
                   point.Y >= Math.Min(lineStart.Y, lineEnd.Y) && point.Y <= Math.Max(lineStart.Y, lineEnd.Y);
        }

        public static Point FindLeftestPoint(List<Point> curPolygon)
        {
            Point leftestPoint = new Point(1000000, -1000000);

            foreach (Point point in curPolygon)
            {
                if (point.X < leftestPoint.X)
                {
                    leftestPoint = point;
                }
                else if (point.X == leftestPoint.X && point.Y > leftestPoint.Y)
                {
                    leftestPoint = point;
                }
            }

            return leftestPoint;
        }

        private Point FindNextPoint(Point curPoint)
        {
            Point nextPoint = new Point(0, 0);

            for (int i = 0; i < polygon1.Count(); i++)
            {
                if (curPoint.X == polygon1[i].X && curPoint.Y == polygon1[i].Y)
                {
                    if (i != polygon1.Count() - 1)
                    {
                        nextPoint = polygon1[i + 1];
                    }
                    else
                    {
                        nextPoint = polygon1[0];
                    }

                }
            }

            for (int i = 0; i < polygon2.Count(); i++)
            {
                if (curPoint.X == polygon2[i].X && curPoint.Y == polygon2[i].Y)
                {
                    if (i != polygon2.Count() - 1)
                    {
                        nextPoint = polygon2[i + 1];
                    }
                    else
                    {
                        nextPoint = polygon2[0];
                    }

                }
            }

            return nextPoint;
        }

        public static bool FindSide(Point p1, Point p2, float x, float y)
        {
            float xa = p2.X - p1.X;
            float ya = p2.Y - p1.Y;
            x -= p1.X;
            y -= p1.Y;

            return y * xa - x * ya > 0;
        }

        private List<Point> UnionConvexPolygons(List<Point> polygon1, List<Point> polygon2)
        {
            // Находим самую левую точку каждого полигона
            Point left1 = FindLeftestPoint(polygon1);
            Point left2 = FindLeftestPoint(polygon2);

            // Определяем текущий и другой полигоны
            var (curPolygon, otherPolygon) = left1.X < left2.X ? (polygon1, polygon2) : (polygon2, polygon1);

            // Текущая и начальная точки
            Point curPoint = new Point(0, 0);
            Point nextPoint = new Point(0, 0);
            Point startPoint = FindLeftestPoint(curPolygon);

            // Список использованных точек
            List<Point> used = new List<Point>();
            List<Point> finalPoints = new List<Point>();

            bool start = false;
            int steps = 0;
            int i_cur = 0;
            
            while ((startPoint.X != curPoint.X) || ((startPoint.X != curPoint.X)))
            {
                if (steps > (polygon1.Count + polygon2.Count + 1))
                {
                    Console.WriteLine($"{steps}, {polygon1.Count + polygon2.Count + 1}");
                    Console.WriteLine($"break break break");
                    break;
                }
                
                steps++;

                // Обработка первой итерации цикла
                if (!start)
                {
                    curPoint = FindLeftestPoint(curPolygon);
                    nextPoint = FindNextPoint(curPoint);
                    start = true;
                }

                // Ребро от curPoint до nextPoint
                var line = new List<Point> { curPoint, nextPoint };

                // Добавляем первую точку ребра в финальный список
                if (!used.Contains(line[0]))
                {
                    finalPoints.Add(line[0]);
                    used.Add(line[0]);
                    Console.WriteLine($"Point №{i_cur}: {string.Join(", ", line[0])}");
                    i_cur++;

                    //canvas.DrawPoint(line[0], "red");
                    //if (finalPoints.Count > 1)
                    //{
                    //    canvas.DrawLine(finalPoints[^2], finalPoints[^1], "green");
                    //    Thread.Sleep(1000);
                    //}
                    //canvas.Update();
                }

                // Текущая точка в другом полигоне
                var otherPoint = FindLeftestPoint(otherPolygon);
                var otherNextPoint = FindNextPoint(otherPoint);
                int j = 0;

                List<List<Point>> intersections = new List<List<Point>>();

                while (j < otherPolygon.Count)
                {
                    // Текущее ребро в другом полигоне
                    // var otherLine = new List<Point> { otherPoint, FindNextPoint(otherPoint) };

                    // Точка пересечения рёбер
                    var pointOfIntersection = Intersection(line[0], line[1], otherPoint, FindNextPoint(otherPoint));

                    if (pointOfIntersection.X != -1)
                    {
                        intersections.Add(new List<Point> { otherPoint, FindNextPoint(otherPoint), pointOfIntersection });
                    }

                    otherPoint = otherNextPoint;
                    otherNextPoint = FindNextPoint(otherPoint);
                    j++;
                }

                if (intersections.Count == 0)
                {
                    curPoint = nextPoint;
                    nextPoint = FindNextPoint(curPoint);
                    continue;
                }

                // Находим ближайшую точку пересечения

                List<Point> greatIntersection = new List<Point>();
                int minX = int.MaxValue;

                foreach (var lineAndPoint in intersections)
                {
                    int modul = Math.Abs(line[0].X - lineAndPoint[2].X);
                    if (modul < minX)
                    {
                        minX = modul;
                        greatIntersection = lineAndPoint;
                    }
                }

                if (!used.Contains(greatIntersection[2]))
                {
                    finalPoints.Add(greatIntersection[2]);
                    used.Add(greatIntersection[2]);
                    Console.WriteLine($"Point №{i_cur}: {string.Join(", ", greatIntersection[2])}");
                    i_cur++;
                    //canvas.DrawPoint(greatIntersection[2], "red");
                    //canvas.DrawLine(finalPoints[^2], finalPoints[^1], "green");
                    //Thread.Sleep(1000);
                    //canvas.Update();
                }
                else
                {
                    continue;
                }

                foreach (var pointInLine in greatIntersection)
                {
                    if (!FindSide(line[0], line[1], pointInLine.X, pointInLine.Y))
                    {
                        if (!used.Contains(pointInLine))
                        {
                            finalPoints.Add(pointInLine);
                            used.Add(pointInLine);
                            Console.WriteLine($"Point №{i_cur}: {string.Join(", ", pointInLine)}");
                            i_cur++;
                            //canvas.DrawPoint(pointInLine, "red");
                            //canvas.DrawLine(finalPoints[^2], finalPoints[^1], "green");
                            //Thread.Sleep(1000);
                            //canvas.Update();
                        }
                        else
                        {
                            otherPoint = otherNextPoint;
                            otherNextPoint = FindNextPoint(otherPoint);
                            j++;
                            continue;
                        }

                        foreach (var point in otherPolygon)
                        {
                            if (point.X == pointInLine.X && point.Y == pointInLine.Y)
                            {
                                curPoint = point;
                            }
                        }
                        nextPoint = FindNextPoint(curPoint);
                        var tempPolygon = curPolygon;
                        
                        curPolygon = otherPolygon;
                        otherPolygon = tempPolygon;

                    }
                }
            }
            
            //canvas.DrawLine(finalPoints[^1], finalPoints[0], "green");
            //canvas.Update();
            Console.WriteLine("=====================================================\nEND 2\n=====================================================\n");
            return finalPoints;
        }

        private void drawingPanel_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;

            if (polygon1.Count > 1)
                DrawPolygon(g, polygon1, Color.Blue);

            if (polygon2.Count > 1)
                DrawPolygon(g, polygon2, Color.Green);

            if (unionPolygon.Count > 1)
                DrawPolygon(g, unionPolygon, Color.Red);

            DrawPoints(g, points, Color.Black);
            //DrawIntersections(g, polygon1, polygon2);
        }

        private void DrawPolygon(Graphics g, List<Point> polygon, Color color)
        {
            if (polygon.Count > 1)
            {
                using (Pen pen = new Pen(color, 2))
                {
                    g.DrawPolygon(pen, polygon.ToArray());
                }
            }
        }

        private void DrawPoints(Graphics g, List<Point> points, Color color)
        {
            foreach (var point in points)
            {
                g.FillEllipse(new SolidBrush(color), point.X - 2, point.Y - 2, 4, 4);
            }
        }

        private void DrawIntersections(Graphics g, List<Point> polygon1, List<Point> polygon2)
        {
            Pen pen = new Pen(Color.Yellow, 2);

            for (int i = 0; i < polygon1.Count; i++)
            {
                for (int j = 0; j < polygon2.Count; j++)
                {
                    int ii = i + 1;
                    int jj = j + 1;

                    if (i == polygon1.Count - 1)
                    {
                        i = polygon1.Count - 1;
                        ii = 0;
                    }

                    if (j == polygon2.Count - 1)
                    {
                        j = polygon2.Count - 1;
                        jj = 0;
                    }

                    g.DrawLine(pen, polygon1[i], polygon1[ii]);
                    g.DrawLine(pen, polygon2[j], polygon2[jj]);
                    
                    //MessageBox.Show($"{polygon1[i].X}, {polygon1[i].Y}, {j}");
                    //MessageBox.Show($"{polygon1[i].X}, {polygon1[i].Y}, {polygon1[i + 1].X}, {polygon1[i + 1].Y},{polygon2[j].X}, {polygon2[j].Y},{polygon2[j + 1].X}, {polygon2[j + 1].Y},");
                    //MessageBox.Show($"{interPoint.X}, {interPoint.Y}, {i}, {j}");
                    
                    Point interPoint = Intersection(polygon1[i], polygon1[ii], polygon2[j], polygon2[jj]);

                    if (interPoint.X != -1)
                    {
                        g.FillEllipse(new SolidBrush(Color.Green), interPoint.X - 5, interPoint.Y - 5, 10, 10);
                    }
                    
                }
            }
        }
    }
}
