using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices.ComTypes;
using System.Security.Cryptography;
using System.Security.Permissions;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace Affine_transformations_in_space
{
    public partial class Afins3D : Form
    {

        polyhedron pop;
        //double[,] worldMatrix;
        //polyhedron AxisPop;
        double d = 5;
        public Func<Matrix3D> projectFunc;
        public static List<polygon> pnts;
        public static List<point> AxesPoints;
        //public static int scale = 100;
        Matrix3D reflMatr;
        Matrix3D lRotMatr;
        public static int spin = 0;
        public static int switchRotationCase = 0;
        
        double translationX , translationY, translationZ;
        double rotationX, rotationY, rotattionZ;
        double scaleX, scaleY, scaleZ;
        double scaleXCenter, scaleYCenter, scaleZCenter;


        public Afins3D()
        {
            InitializeComponent();
            projectFunc = perspectiveMatrix;
            setDefaultWorldPosition();
            pictureBox1.MouseMove += Form_MouseMove;
        }


        private void Form_MouseMove(object sender, MouseEventArgs e) 
        {
            MousepositionLabel.Text = $"X:{e.X}, Y:{e.Y}";
        }

        private void setDefaultWorldPosition()
        {   
            
            translationX = 0; translationY = 0; translationZ = 0;
            rotationX = 0; rotationY = 0; rotattionZ = 0;
            scaleX = 1; scaleY = 1; scaleZ = 1;
            scaleXCenter = 0; scaleYCenter = 0; scaleZCenter = 0;

            reflMatr =  new Matrix3D( new double[,]
            {
                {1, 0, 0, 0 },
                {0, 1, 0, 0 },
                {0, 0, 1, 0 },
                {0, 0, 0, 1 }
            });

            lRotMatr = new Matrix3D (new double[,]
           {
                {1, 0, 0, 0 },
                {0, 1, 0, 0 },
                {0, 0, 1, 0 },
                {0, 0, 0, 1 }
          });
        }

        private double pointXOnMatrix(double point, double[,] matrix)
        {
            return point * matrix[0, 0] + point * matrix[1, 0] + point * matrix[2, 0] + matrix[3, 0];
        }

        private double pointYOnMatrix(double point, double[,] matrix)
        {
            return point * matrix[0, 1] + point * matrix[1, 1] + point * matrix[2, 1] + matrix[3, 1];
        }

        private double pointZOnMatrix(double point, double[,] matrix)
        {
            return point * matrix[0, 2] + point * matrix[1, 2] + point * matrix[2, 2] + matrix[3, 2];
        }
        private point chelnok(polygon face, Matrix3D world) 
        {
            double sumX = 0; double sumY = 0; double sumZ = 0;
            foreach (var vertex in face.Vertices)
            {
                sumX += pointXOnMatrix(vertex.X, world.Matrix);
                sumY += pointYOnMatrix(vertex.Y, world.Matrix);
                sumZ += pointYOnMatrix(vertex.Z, world.Matrix);

            }
            sumX /= face.Vertices.Count;
            sumY /= face.Vertices.Count;
            sumZ /= face.Vertices.Count;
            return new point(sumX, sumY, sumZ);
        }

        // Возвращает геометрический центр полигона
        // Возвращает геометрический центр всего многогранника
        private (double, double, double) Centroid(Matrix3D world)
        {
            double centerX = 0; double centerY = 0; double centerZ = 0;
            foreach (point center in pnts.Select(face => face.GetCenter()))
            {
                centerX += center.X;
                centerY += center.Y;
                centerZ += center.Z;
            }

            centerX /= pnts.Count;
            centerY /= pnts.Count;
            centerZ /= pnts.Count;

            return (centerX, centerY, centerZ);
        }

        void RadioButton_CheckedChanged(object sender, EventArgs e) 
        {
           var radionButton = sender as RadioButton;

            if (radionButton != null && radionButton.Checked) 
            {
                if (radionButton.Text == "Перспектива") projectFunc = perspectiveMatrix;
                if (radionButton.Text == "Изометрия") projectFunc = IsometricMatrix;
            }
            
            pictureBox1.Invalidate();         
        }

        public class Matrix3D 
        {
            public double[,] Matrix { get; set; }

            public Matrix3D(double[,] _worldMatrix) 
            {
                Matrix = _worldMatrix;
            }

            public Matrix3D() 
            {

            }

            public static Matrix3D operator* (Matrix3D mat1, Matrix3D mat2)
            {
                int rows = mat1.Matrix.GetLength(0);
                int cols = mat2.Matrix.GetLength(0);
                int commonDim = mat1.Matrix.GetLength(1);

                double[,] result = new double[rows, cols];

                for (int i = 0; i < rows; i++)
                {

                    for (int j = 0; j < cols; j++)
                    {
                        result[i, j] = 0;
                        for (int k = 0; k < commonDim; k++)
                        {
                            result[i, j] += mat1.Matrix[i, k] * mat2.Matrix[k, j];
                        }
                    }
                }
                return new Matrix3D(result);
            }


        }


        public class point 
        {
            public double X, Y, Z;

            public point(double x, double y, double z) 
            {
                X = x;
                Y = y;
                Z = z;
            }

        }
        public class polygon
        {
            public List<point> Vertices;

            public polygon(List<point> vertices) 
            {
                Vertices = vertices;
            }

            public point GetCenter()
            {
                double x = Vertices.Average(point => point.X);
                double y = Vertices.Average(point => point.Y);
                double z = Vertices.Average(point => point.Z);
                return new point(x, y, z);
            }
        }

        public class Polyrotation 
        {

            List<point> _generatingCurve;
            string _axisRotation;
            int _segments;
            double _angelRotation;

            Polyrotation(List<point> generatingCurve, string axisRotation, int segments) 
            {
                _generatingCurve = generatingCurve;
                _axisRotation = axisRotation;
                _segments = segments;
                _angelRotation = 360.0 / segments;
            }

            public polyhedron buildRotationFigure() 
            {
                List<point> vertices = new List<point>();
                List<polygon> faces = new List<polygon>();
                Matrix3D m = new Matrix3D();
                for (int i = 0; i < _segments; i++) 
                {
                    //Matrix3D rotationMatrix = new Matrix3D();
                    double angle = i * _angelRotation;
                    switch (_axisRotation.ToUpper()) 
                    {
                        case "X":
                            
                            Matrix3D rotationMatrix = xRotationMatrix(angle);
                            break;
                        case "Y":
                            break;
                        case "Z":
                            break;
                    };

                    foreach (var point in _generatingCurve) 
                    {
                        point rotatedPoint = TransformToWorld(point, rotationMatrix);
                        vertices.Add(rotatedPoint);
                    }

                }

                for (int segment = 0; segment < _segments; segment++)
                {
                    for (int i = 0; i < curvePointsCount - 1; i++)
                    {
                        // Индексы текущего сегмента
                        int v1 = segment * curvePointsCount + i;
                        int v2 = segment * curvePointsCount + i + 1;

                        // Индексы следующего сегмента (с учётом замыкания)
                        int nextSegment = (segment + 1) % _segments;
                        int v3 = nextSegment * curvePointsCount + i + 1;
                        int v4 = nextSegment * curvePointsCount + i;

                        // Создаём грань из 4-х точек
                        polygon face = new polygon(new List<point> { vertices[v1], vertices[v2], vertices[v3], vertices[v4] });
                        faces.Add(face);
                    }
                }



                return new polyhedron(vertices, faces);

            }

        }


        public class polyhedron
        {
            public List<point> Vertices;
            public List<polygon> Faces;

            public polyhedron(List<point> verticles , List<polygon> faces)
            {
                Vertices = verticles;
                Faces = faces;
            }

            public static polyhedron drawTetraedr()
            {       
                point v1 = new point(1 , 1 , 1 );
                point v2 = new point(1  , -1  , -1  );
                point v3 = new point(-1  , 1  , -1  );
                point v4 = new point(-1  , -1  , 1  ) ;

                polygon firstPol = new polygon(new List<point> { v1, v2, v3 });
                polygon secondPol = new polygon(new List<point> { v1, v2, v4 });
                polygon thirdPol = new polygon(new List<point> {v1, v3, v4 });
                polygon fourthPol = new polygon(new List<point> {v2, v3, v4 });

                return new polyhedron(new List<point> { v1, v2, v3, v4 }, new List<polygon> {firstPol, secondPol, thirdPol, fourthPol });             
            }

            public static polyhedron drawGexaedr() 
            {
                point v1 = new point(-1, -1, -1);
                point v2 = new point(1, -1, -1);
                point v3 = new point(-1, 1, -1);
                point v4 = new point(1, 1, -1);              
                point v5 = new point(-1, -1, 1);
                point v6 = new point(1, -1, 1);
                point v7 = new point(-1, 1, 1);
                point v8 = new point(1, 1, 1);

                polygon firstPol = new polygon(new List<point> { v1, v2, v4, v3 }); // Нижняя грань
                polygon secondPol = new polygon(new List<point> { v5, v6, v8, v7 }); // Верхняя грань
                polygon thirdPol = new polygon(new List<point> { v1, v3, v7, v5 }); // Левая грань
                polygon fourdPol = new polygon(new List<point> { v2, v4, v8, v6 }); // Правая грань
                polygon fivethPol = new polygon(new List<point> { v1, v2, v6, v5 }); // Передняя грань
                polygon sixthPol = new polygon(new List<point> { v3, v4, v8, v7 });  // Задняя грань

                return new polyhedron(new List<point> { v1, v2, v3, v4, v5, v6, v7, v8 }, new List<polygon> {firstPol, secondPol, thirdPol, fourdPol, fivethPol, sixthPol });
            }
            public static polyhedron DrawOctaedr()
            {
                point v1 = new point(1, 0, 1);
                point v2 = new point(1, 0, -1);
                point v3 = new point(-1, 0, -1);
                point v4 = new point(-1, 0, 1);
                point v5 = new point(0, 1, 0);
                point v6 = new point(0, -1, 0);

                polygon firstPol = new polygon(new List<point> { v1, v2, v5 });
                polygon secondPol = new polygon(new List<point> { v2, v3, v5 });
                polygon thirdPol = new polygon(new List<point> { v3, v4, v5 });
                polygon fourdPol = new polygon(new List<point> { v4, v1, v5 });
                polygon fivethPol = new polygon(new List<point> { v1, v2, v6 });
                polygon sixthPol = new polygon(new List<point> { v2, v3, v6 });
                polygon sevenPol = new polygon(new List<point> { v3, v4, v6 });
                polygon eightPol = new polygon(new List<point> { v4, v1, v6 });

                return new polyhedron(new List<point> { v1, v2, v3, v4, v5, v6 }, new List<polygon> { firstPol, secondPol, thirdPol, fourdPol, fivethPol, sixthPol, sevenPol, eightPol });
            }

            public static polyhedron drawIcosahedr()
            {
                double phi = (1 + Math.Sqrt(5)) / 2;  // Золотое сечение
                double scale = 1;  // Масштаб для вершин

                List<point> vertices = new List<point>
            {
            new point(-1 * scale,  phi * scale, 0),
            new point( 1 * scale,  phi * scale, 0 * scale),
            new point(-1 * scale, -phi * scale, 0),
            new point( 1 * scale, -phi * scale, 0),
            new point(0, -1 * scale,  phi * scale),
            new point(0,  1 * scale,  phi * scale),
            new point(0, -1 * scale, -phi * scale),
            new point(0,  1 * scale, -phi * scale),
            new point( phi * scale, 0, -1 * scale),
            new point( phi * scale, 0,  1 * scale),
            new point(-phi * scale, 0, -1 * scale),
            new point(-phi * scale, 0,  1 * scale)
            };

                // Определение 20 треугольных граней, каждая из которых указывает на три вершины
                List<polygon> faces = new List<polygon>
            {
            new polygon(new List<point> { vertices[0], vertices[11], vertices[5] }),
            new polygon(new List<point> { vertices[0], vertices[5], vertices[1] }),
            new polygon(new List<point> { vertices[0], vertices[1], vertices[7] }),
            new polygon(new List<point> { vertices[0], vertices[7], vertices[10] }),
            new polygon(new List<point> { vertices[0], vertices[10], vertices[11] }),

            new polygon(new List<point> { vertices[1], vertices[5], vertices[9] }),
            new polygon(new List<point> { vertices[5], vertices[11], vertices[4] }),
            new polygon(new List<point> { vertices[11], vertices[10], vertices[2] }),
            new polygon(new List<point> { vertices[10], vertices[7], vertices[6] }),
            new polygon(new List<point> { vertices[7], vertices[1], vertices[8] }),

            new polygon(new List<point> { vertices[3], vertices[9], vertices[4] }),
            new polygon(new List<point> { vertices[3], vertices[4], vertices[2] }),
            new polygon(new List<point> { vertices[3], vertices[2], vertices[6] }),
            new polygon(new List<point> { vertices[3], vertices[6], vertices[8] }),
            new polygon(new List<point> { vertices[3], vertices[8], vertices[9] }),

            new polygon(new List<point> { vertices[4], vertices[9], vertices[5] }),
            new polygon(new List<point> { vertices[2], vertices[4], vertices[11] }),
            new polygon(new List<point> { vertices[6], vertices[2], vertices[10] }),
            new polygon(new List<point> { vertices[8], vertices[6], vertices[7] }),
            new polygon(new List<point> { vertices[9], vertices[8], vertices[1] })
            };

                return new polyhedron(vertices, faces);
            }


            public static polyhedron drawDodecahedr()
            {
                double phi = (1 + Math.Sqrt(5)) / 2; // Золотое сечение
                double scale = 1; // Масштаб для вершин

                List<point> vertices = new List<point>
                {
                new point(-1 * scale, -1* scale , -1* scale), //0
                new point(-1* scale, -1* scale, 1* scale), //1
                new point(-1* scale, 1* scale, -1* scale), //2
                new point(-1* scale, 1* scale, 1* scale), //3
                new point( 1* scale, -1* scale, -1* scale), //4
                new point( 1* scale, -1* scale, 1* scale), //5
                new point( 1* scale, 1* scale, -1* scale), //6
                new point( 1* scale, 1* scale, 1* scale),//7

                new point(0, -1/phi * scale, -phi * scale), //8
                new point(0, -1/phi * scale, phi * scale),//9
                new point(0, 1/phi * scale, -phi * scale),//10
                new point(0, 1/phi * scale, phi * scale),//11

                new point(-1/phi * scale, -phi * scale, 0),//12
                new point(-1/phi * scale, phi * scale, 0),//13
                new point(1/phi * scale, -phi * scale, 0),//14
                new point(1/phi * scale, phi * scale, 0),//15

                new point(-phi * scale, 0, -1/phi * scale),//16
                new point( phi * scale, 0, -1/phi * scale),//17
                new point(-phi * scale, 0, 1/phi * scale),//18
                new point( phi * scale, 0, 1/phi * scale)//19
                };

                // Определение 12 пятиугольных граней
                List<polygon> faces = new List<polygon>
                {
                //new polygon(new List<point> { vertices[0], vertices[1], vertices[2], vertices[3] }),
                new polygon(new List<point> { vertices[0], vertices[8], vertices[4], vertices[14], vertices[12] }),
                new polygon(new List<point> { vertices[15], vertices[7], vertices[11], vertices[3], vertices[13] }),
                new polygon(new List<point> { vertices[0], vertices[16], vertices[2], vertices[10], vertices[8] }),
                //new polygon(new List<point> { vertices[0], vertices[19] }),
                new polygon(new List<point> { vertices[14], vertices[4], vertices[17], vertices[19], vertices[5] }),
                new polygon(new List<point> { vertices[8], vertices[4], vertices[17], vertices[6], vertices[10] }),
                new polygon(new List<point> { vertices[0], vertices[12], vertices[1], vertices[18], vertices[16] }),
                new polygon(new List<point> { vertices[1], vertices[12], vertices[14], vertices[5], vertices[9] }),
                new polygon(new List<point> { vertices[3], vertices[13], vertices[2], vertices[16], vertices[18] }),
                new polygon(new List<point> { vertices[3], vertices[18], vertices[1], vertices[9], vertices[11] }),
                new polygon(new List<point> { vertices[7], vertices[11], vertices[9], vertices[5], vertices[19] }),
                new polygon(new List<point> { vertices[13], vertices[2], vertices[10], vertices[6], vertices[15] }),
                //new polygon(new List<point> { vertices[19], vertices[18], vertices[17], vertices[16], vertices[16] })
                };

                return new polyhedron(vertices, faces);
            }

        }

        //перспективная проекция
        private Matrix3D perspectiveMatrix()
        {
           
            return new Matrix3D(new double[,]
            {
                {1, 0, 0, 0 },
                {0, 1, 0, 0 },
                {0, 0, 1, -1 / d },
                {0, 0, 1, 0 }
            });
        }

        private Matrix3D IsometricMatrix()
        {
            return new Matrix3D(new double[,]
            {
                {Math.Sqrt(3), 0, -Math.Sqrt(3), 0},
                {1, 2, 1, 0},
                {Math.Sqrt(2), -Math.Sqrt(2), Math.Sqrt(2), 0},
                {0,0,0,1}
            });
        }


        private Point ApplyProjection(point p, Matrix3D projectionMatrix)
        {
            double newX = p.X * projectionMatrix.Matrix[0, 0] + p.Y * projectionMatrix.Matrix[1, 0] + p.Z * projectionMatrix.Matrix[2, 0];
            double newY = p.X * projectionMatrix.Matrix[0, 1] + p.Y * projectionMatrix.Matrix[1, 1] + p.Z * projectionMatrix.Matrix[2, 1];

            int x2D = (int)(newX + pictureBox1.Width / 2);
           int y2D = (int)(newY + pictureBox1.Height / 2);

            return new Point((int)newX, (int) newY);
        }

        private point TransformToWorld(point p, double[,] worldMatrix)
        {
            double x = p.X * worldMatrix[0, 0] + p.Y * worldMatrix[1, 0] + p.Z * worldMatrix[2, 0] + worldMatrix[3, 0];
            double y = p.X * worldMatrix[0, 1] + p.Y * worldMatrix[1, 1] + p.Z * worldMatrix[2, 1] + worldMatrix[3, 1];
            double z = p.X * worldMatrix[0, 2] + p.Y * worldMatrix[1, 2] + p.Z * worldMatrix[2, 2] + worldMatrix[3, 2];

            return new point(x, y, z);
        }      

        private void pictureBox1_Paint(object sender, PaintEventArgs e)
        {   
            
            if (pop != null)
            {

                //worldMatrix = //getWorldMatrix(translationX, translationY, translationZ, rotationX, rotationY, rotattionZ, scaleX, scaleY, scaleZ);
                //return MultiplyMarices(lRotMatr, MultiplyMarices(reflMatr, MultiplyMarices(scalingMatr, MultiplyMarices(rotationMatr, translationMatr))));

                Matrix3D translationMatr = translationMatrix(translationX, translationY, translationZ);
                Matrix3D scalingMatr = scalingMatrix(scaleX, scaleY, scaleZ, scaleXCenter, scaleYCenter, scaleZCenter);
                Matrix3D rotationMatr = rotationMatrix(rotationX, rotationY, rotattionZ);
                Matrix3D LRotationMatr = LRotation( Convert.ToInt32( textBox4.Text), Convert.ToInt32( textBox5.Text), Convert.ToInt32 (textBox6.Text),Convert.ToInt32( textBox7.Text));
                var (xg1, yg2, zg3) = Centroid(translationMatr * rotationMatr * scalingMatr);
                Matrix3D fromCenter = translationMatrix(xg1, yg2, zg3);
                Matrix3D toCenter = translationMatrix(-xg1, -yg2, -zg3);

                scaleXCenter = 0;
                scaleYCenter = 0;
                scaleZCenter = 0;

                //Matrix3D worldMatrix = toCenter * translationMatr * rotationMatr * scalingMatr * fromCenter;
                Matrix3D worldMatrix = fromCenter * scalingMatr * rotationMatr * translationMatr * LRotationMatr * toCenter;
                //foreach (var vertex in pop.Vertices)
                //{
                //    var transformedPoint = TransformToWorld(vertex, worldMatrix);
                //    Console.WriteLine($"Transformed Point: X = {transformedPoint.X}, Y = {transformedPoint.Y}, Z = {transformedPoint.Z}");

                //}
                //Console.WriteLine("\n");


                foreach (var face in pop.Faces) 
                {
                    List<Point> points2D = new List<Point>();
                    foreach (var vertex in face.Vertices) 
                    {
                        point worldPoint = TransformToWorld(vertex, worldMatrix.Matrix);
                        Point projectedPoint = ApplyProjection(worldPoint, projectFunc()); //передаём сюда матрицу перспективы(конверт в 2D)
                        points2D.Add(projectedPoint);
                    }
                    

                    int clientWidth = e.ClipRectangle.Width;
                    int clientHeight = e.ClipRectangle.Height;

                    int offsetX = clientWidth / 2;
                    int offsetY = clientHeight / 2;

                    var centeredPoints = points2D.Select(p => new Point(p.X + offsetX, p.Y + offsetY)).ToArray();

                    e.Graphics.DrawPolygon(Pens.Black, centeredPoints);

                }



                SolidBrush sb = new SolidBrush(Color.FromArgb(255, 255, 0, 0));
                List<Point> points2D_1 = new List<Point>();
                foreach (var ver in pop.Vertices)
                {


                    point worldPoint = TransformToWorld(ver, worldMatrix.Matrix);
                    Point projectedPoint = ApplyProjection(worldPoint, projectFunc()); //передаём сюда матрицу перспективы(конверт в 2D)
                    points2D_1.Add(projectedPoint);

                    foreach (var vr in points2D_1)
                    {
                        e.Graphics.FillEllipse(sb, vr.X, vr.Y, 5, 5);

                    }
                }


            }

        }


        //Матрица перемещения
        private Matrix3D translationMatrix(double tx, double ty, double tz)
        {
            return new Matrix3D(new double[,]
            {
                { 1, 0, 0, 0 },
                { 0, 1, 0, 0 },
                { 0, 0, 1, 0 },
                { tx, ty, tz, 1 }
            });         
        }

        //Матрица Масштабирования
        private Matrix3D scalingMatrix(double sx, double sy, double sz, double cx, double cy, double cz)
        {

            return new Matrix3D( new double[,] 
            {
                {sx, 0, 0 ,0 },
                {0, sy, 0, 0 },
                {0, 0, sz, 0 },
                {0, 0, 0, 1 }
            });
          
        }

        private Matrix3D rotationMatrix(double angleX, double angleY, double angleZ)
        {
            //return MultiplyMarices(MultiplyMarices(zRotation(angleZ), yRotation(angleY)), xRotationMatrix(angleX));
            //return MultiplyMarices(MultiplyMarices(xRotationMatrix(angleX), yRotation(angleY)), zRotation(angleZ));
            return xRotationMatrix(angleX) * yRotation(angleY) * zRotation(angleZ);
            //return MultiplyMarices(zRotation(angleZ), MultiplyMarices(yRotation(angleY), xRotationMatrix(angleX)));
        }

        public Matrix3D xRotationMatrix(double a) 
        {

            double radians = (Math.PI / 180) * a;
            return new Matrix3D(new double[,] {
            { 1,0,0,0 },
            {0, Math.Cos(radians), - Math.Sin(radians), 0 },
            { 0, Math.Sin(radians), Math.Cos(radians ), 0},
            {0, 0 ,0, 1 }
            });
         
        }

        private Matrix3D yRotation(double a) 
        {
            double radians = (Math.PI / 180) * a;
            var cosx = Math.Cos(radians);
            var sinx = Math.Sin(radians);

            return new Matrix3D(new double[,] {
            {cosx, 0 , sinx, 0 },
            {0, 1, 0, 0 },
            {-sinx, 0, cosx, 0 },
            {0, 0, 0, 1 }
            });
          
        }

        private Matrix3D zRotation(double a) 
        {
            double radians = (Math.PI / 180) * a;
            var cosx = Math.Cos(radians);
            var sinx = Math.Sin(radians);

            return new Matrix3D( new double[,] {
            {cosx, -sinx, 0, 0  },
            {sinx, cosx, 0, 0  },
            {0, 0, 1, 0 },
            {0, 0, 0, 1 }
            });         
        }


        //private double[,] MultiplyMarices(double[,] mat1, double[,] mat2) 
        //{
        //    int rows = mat1.GetLength(0);
        //    int cols = mat2.GetLength(0);
        //    int commonDim = mat1.GetLength(1);

        //    double[,] result = new double[rows, cols];

        //    for (int i = 0; i < rows; i++) 
        //    {

        //        for (int j = 0; j < cols; j++) 
        //        {
        //            result[i, j] = 0;
        //            for (int k = 0; k < commonDim; k++) 
        //            {
        //                result[i, j] += mat1[i, k] * mat2[k, j];
        //            }
        //        }
        //    }
        //    return result;
        //}


        private Matrix3D LRotation(int fi, double l, double m, double n)
        {
            double fiRad = (Math.PI / 180) * fi;
            double cosFi = Math.Cos(fiRad);
            double sinFI = Math.Sin(fiRad);
            return new Matrix3D( new double[,]
            {
                {Math.Pow(l, 2) + cosFi * (1 - Math.Pow(l, 2)), l * (1 - cosFi) * m + n * sinFI, l * (1 - cosFi) * n - m * sinFI, 0},
                {l * (1 - cosFi) * m - n * sinFI, Math.Pow(m, 2) + cosFi * (1 - Math.Pow(m, 2)), m * (1 - cosFi ) *  n  + l * sinFI, 0},
                {l * (1 - cosFi) * n + m * sinFI, m * (1 - cosFi) * n - l * sinFI, Math.Pow(n,2) + cosFi * (1 - Math.Pow(n, 2)), 0  },
                {0, 0, 0 ,1 }

            });
          
        }

        private Matrix3D getWorldMatrix(double tx, double ty, double tz, double angleX, double angleY, double angleZ, double sx, double sy, double sz) 
        {
            //(scaleXCenter, scaleYCenter, scaleZCenter) = Centroid();
            //MessageBox.Show(scaleXCenter.ToString());
            //    double cx = scaleXCenter;
            //    double cy = scaleYCenter;
            //    double cz = scaleZCenter;

            //    double[,] translationToOrigin = translationMatrix(-cx, -cy, -cz);
            //    double[,] translationBack = translationMatrix(cx, cy, cz);


            //    double[,] translationMatr = translationMatrix(tx, ty, tz);
            //    double[,] rotationMatr = rotationMatrix(angleX, angleY, angleZ);
            //    double[,] scalingMatr = scalingMatrix(sx, sy, sz, 0, 0, 0);

            //    //Итоговая мировая матрица Translation * Rotation * Scaling * Reflection * Lrotation
            //    return MultiplyMarices(translationMatr, MultiplyMarices(translationBack, MultiplyMarices(rotationMatr, MultiplyMarices(scalingMatr, translationToOrigin))));
            Matrix3D translationMatr = translationMatrix(tx, ty, tz);
            Matrix3D scalingMatr = scalingMatrix(sx, sy, sz, scaleXCenter, scaleYCenter, scaleZCenter);
            Matrix3D rotationMatr = rotationMatrix(angleX, angleY, angleZ);
            scaleXCenter = 0;
            scaleYCenter = 0;
            scaleZCenter = 0;
            //return MultiplyMarices(translationMatr, MultiplyMarices(rotationMatr, scalingMatr));
            //return translationMatr;
            //return MultiplyMarices(translationMatr, MultiplyMarices(rotationMatr, scalingMatr));
            //return MultiplyMarices(scalingMatr, MultiplyMarices(rotationMatr, translationMatr));   //работает
            //return MultiplyMarices(lRotMatr, MultiplyMarices(reflMatr, MultiplyMarices(scalingMatr, MultiplyMarices(rotationMatr, translationMatr))));
            return translationMatr * scalingMatr * rotationMatr;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            
            switch (comboBox1.Text) 
            {
                case "Тетраэдр":
                    pop = polyhedron.drawTetraedr();
                    pnts = pop.Faces;
                    pictureBox1.Invalidate();                  
                    break;
                case "Гексаэдр":
                    pop = polyhedron.drawGexaedr();
                    pnts = pop.Faces;
                    pictureBox1.Invalidate();
                    break;
                case "Октаэдр":
                    pop = polyhedron.DrawOctaedr();
                    pnts = pop.Faces;
                    pictureBox1.Invalidate();
                    break;

                case "Икосаэдр":
                    pop = polyhedron.drawIcosahedr();
                    pnts = pop.Faces;
                    pictureBox1.Invalidate();
                    break;

                case "Додекаэдр":
                    pop = polyhedron.drawDodecahedr();
                    pnts = pop.Faces;
                    pictureBox1.Invalidate();
                    break;
            }
           
        }


        private void button2_Click(object sender, EventArgs e)
        {
            translationX += Convert.ToInt32(dxBox.Text);
            translationY += Convert.ToInt32(dyBox.Text);
            translationZ += Convert.ToInt32(dzBox.Text);

            pictureBox1.Invalidate();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            
            var dxScale = Convert.ToDouble(textBox1.Text, new NumberFormatInfo() { NumberDecimalSeparator = "."});
            var dyScale = Convert.ToDouble(textBox2.Text, new NumberFormatInfo() { NumberDecimalSeparator = "." });
            var dzScale = Convert.ToDouble(textBox3.Text, new NumberFormatInfo() { NumberDecimalSeparator = "." });

            scaleX *= dxScale;
            scaleY *= dyScale;
            scaleZ *= dzScale;
            //point p1 = new point(Convert.ToInt32(textBox1.Text), Convert.ToInt32(textBox2.Text), Convert.ToInt32(textBox3.Text));

            //scaleFigure(p1);
            pictureBox1.Invalidate();
        }



        private void button4_Click(object sender, EventArgs e)
        {
            switch(spin) 
            {
                case 0:
                    //xRotation(3);
                    break;
                case 1:
                    yRotation(3);
                    break;
                case 2:
                    zRotation(3);
                    break;
            }
        }


        public void radioButtonSwitch(object sender, EventArgs e)
        {                      
            var rbt = sender as RadioButton;

            if (rbt != null && rbt.Checked) 
            {
                switch (rbt.Text) 
                {

                    case "XAxis":
                       spin = 0;
                        break;
                    case "YAxis":
                        spin = 1;
                        break;
                    case "ZAxis":
                        spin = 2;
                        break;

                }

            }                          
           
        }
    

    private void button4_Click_1(object sender, EventArgs e)
    {                    

            if (radioButton6.Checked)
            {
               
                var reflXYMatr = new Matrix3D( new double[,]
                {
                        {1, 0, 0, 0 },
                        {0, 1, 0, 0 },
                        {0, 0, -1, 0 },
                        {0, 0, 0, 1 }
                });

                //reflMatr = MultiplyMarices(reflMatr, reflXYMatr);
                reflMatr = reflMatr * reflXYMatr;

                pictureBox1.Invalidate();


            }
            else if (radioButton7.Checked)
            {
               
                var reflXZMatr = new Matrix3D( new double[,]
                {
                        {1, 0, 0, 0 },
                        {0, -1, 0, 0 },
                        {0, 0, 1, 0 },
                        {0, 0, 0, 1 }
                });

                //reflMatr = MultiplyMarices(reflMatr, reflXZMatr);
                reflMatr = reflMatr * reflXZMatr;
                pictureBox1.Invalidate();
            }
            else if (radioButton8.Checked)
            {
               
                var reflYZMatr = new Matrix3D( new double[,]
                {
                        {-1, 0, 0, 0 },
                        {0, 1, 0, 0 },
                        {0, 0, 1, 0 },
                        {0, 0, 0, 1 }
                });

                //reflMatr = MultiplyMarices(reflMatr, reflYZMatr);
                reflMatr = reflMatr * reflYZMatr;

                pictureBox1.Invalidate();
            }
            
    }

        private void switchReflectionRadioButton(object sender, EventArgs e)
        {
            
        }

        private void button5_Click_1(object sender, EventArgs e)
        {
            try 
            {

                double rbb = Convert.ToDouble(rotationBox.Text, new NumberFormatInfo() { NumberDecimalSeparator = "." });

                switch (spin)
                {
                    case 0:
                        rotationX += rbb;
                        break;
                    case 1:
                        rotationY += rbb;
                        break;
                    case 2:
                        rotattionZ += rbb;
                        break;
                }


                pictureBox1.Invalidate();

            }
            catch (Exception ex) { MessageBox.Show("Введите корректные значения!"); }
            
        }

        private double[,] scaleFigureCentroid(double sx, double sy, double sz, double cx,
            double cy, double cz)
        {
            return new double[,]
            {
                {sx, 0, 0 ,0 },
                {0, sy, 0, 0 },
                {0, 0, sz, 0 },
                {cx, cy, cz, 1 }
            };


        }

        private void button6_Click_1(object sender, EventArgs e)
        {
            var dxScale = Convert.ToDouble(textBox1.Text, new NumberFormatInfo() { NumberDecimalSeparator = "." });
            var dyScale = Convert.ToDouble(textBox2.Text, new NumberFormatInfo() { NumberDecimalSeparator = "." });
            var dzScale = Convert.ToDouble(textBox3.Text, new NumberFormatInfo() { NumberDecimalSeparator = "." });

            scaleX *= dxScale;
            scaleY *= dyScale;
            scaleZ *= dzScale;

            //(scaleXCenter, scaleYCenter, scaleZCenter) = Centroid();

            pictureBox1.Invalidate();
        }

        private void button7_Click(object sender, EventArgs e)
        {
            Matrix3D lRotMatr1 = LRotation(Convert.ToInt32(textBox4.Text), Convert.ToDouble(textBox5.Text), Convert.ToDouble(textBox6.Text), Convert.ToDouble(textBox7.Text));
            //lRotMatr =  MultiplyMarices(lRotMatr, lRotMatr1);
            lRotMatr = lRotMatr * lRotMatr1;
            pictureBox1.Invalidate();
        }


        private polyhedron LoadFromOBJ(string filePath)
        {
            List<point> vertices = new List<point>();
            List<polygon> faces = new List<polygon>();

            foreach (var line in File.ReadLines(filePath))
            {
                if (line.StartsWith("v "))
                {
                    var parts = line.Split(' ', (char)StringSplitOptions.RemoveEmptyEntries);
                    double x = double.Parse(parts[1], CultureInfo.InvariantCulture);
                    double y = double.Parse(parts[2], CultureInfo.InvariantCulture);
                    double z = double.Parse(parts[3], CultureInfo.InvariantCulture);
                    vertices.Add(new point(x, y, z));
                }
                else if (line.StartsWith("f "))
                {
                    var parts = line.Split(' ', (char)StringSplitOptions.RemoveEmptyEntries);
                    List<point> faceVertices = parts.Skip(1)
                                                     .Select(index => vertices[int.Parse(index) - 1])
                                                     .ToList();
                    faces.Add(new polygon(faceVertices));
                }
            }

            return new polyhedron(vertices, faces);
        }

        private void SaveToOBJ(polyhedron polyhedron, string filePath)
        {
            using (StreamWriter writer = new StreamWriter(filePath))
            {
                foreach (var vertex in polyhedron.Vertices)
                {
                    writer.WriteLine($"v {vertex.X} {vertex.Y} {vertex.Z}");
                }

                foreach (var face in polyhedron.Faces)
                {
                    var indices = face.Vertices.Select(v => polyhedron.Vertices.IndexOf(v) + 1);
                    writer.WriteLine("f " + string.Join(" ", indices));
                }
            }
        }

        private void LoadFile_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {

                pop = LoadFromOBJ(openFileDialog.FileName);
                pnts = pop.Faces;
                pictureBox1.Invalidate();
            }
        }


    }




}
