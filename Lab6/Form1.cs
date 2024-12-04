using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;
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
        bool centr_flag = true;
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

        // Возвращает геометрический центр полигона
        // Возвращает геометрический центр всего многогранника
        private point Centroid(Matrix3D world)
        {
            double centerX = 0; double centerY = 0; double centerZ = 0;
            foreach (point center in pnts.Select(face => face.GetCenter()))
            {
                var center1 = TransformToWorld(center, world);

                centerX += center1.X;
                centerY += center1.Y;
                centerZ += center1.Z;
            }

            centerX /= pnts.Count;
            centerY /= pnts.Count;
            centerZ /= pnts.Count;

            return new point(centerX, centerY, centerZ);
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
            public double[,] Matrix;

            public Matrix3D(double[,] _worldMatrix) 
            {
                Matrix = _worldMatrix;
            }


            public double this[int x, int y] => Matrix[x, y];

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

            return new Point((int)newX, (int) newY);
        }

        private point TransformToWorld(point p, double[,] worldMatrix)
        {
            double x = p.X * worldMatrix[0, 0] + p.Y * worldMatrix[1, 0] + p.Z * worldMatrix[2, 0] + worldMatrix[3, 0];
            double y = p.X * worldMatrix[0, 1] + p.Y * worldMatrix[1, 1] + p.Z * worldMatrix[2, 1] + worldMatrix[3, 1];
            double z = p.X * worldMatrix[0, 2] + p.Y * worldMatrix[1, 2] + p.Z * worldMatrix[2, 2] + worldMatrix[3, 2];

            return new point(x, y, z);
        }


        private point TransformToWorld(point p, Matrix3D worldMatrix)
        {

            Matrix3D matrix = new Matrix3D(new double[,] { { p.X, p.Y, p.Z, 1 } });


            var newMatrix = matrix * worldMatrix;
                
            double x = p.X * worldMatrix[0, 0] + p.Y * worldMatrix[1, 0] + p.Z * worldMatrix[2, 0] + worldMatrix[3, 0];
            double y = p.X * worldMatrix[0, 1] + p.Y * worldMatrix[1, 1] + p.Z * worldMatrix[2, 1] + worldMatrix[3, 1];
            double z = p.X * worldMatrix[0, 2] + p.Y * worldMatrix[1, 2] + p.Z * worldMatrix[2, 2] + worldMatrix[3, 2];

            return new point(newMatrix[0, 1], newMatrix[0,2], newMatrix[0,3]);
        }

        private void pictureBox1_Paint(object sender, PaintEventArgs e)
        {   
            
            if (pop != null)
            {
                Matrix3D translationMatr = translationMatrix(translationX, translationY, translationZ);
                Matrix3D scalingMatr = scalingMatrix(scaleX, scaleY, scaleZ, 0, 0, 0);
                Matrix3D rotationMatr = rotationMatrix(rotationX, rotationY, rotattionZ);
                Matrix3D LRotationMatr = LRotation( Convert.ToInt32( textBox4.Text) - 1, Convert.ToInt32( textBox5.Text), Convert.ToInt32 (textBox6.Text),Convert.ToInt32( textBox7.Text));
                var matrix = translationMatr * scalingMatr;
                point centr = Centroid(translationMatr /*rotationMatr*/ * scalingMatr);

                scalingMatr = scalingMatrix(scaleX, scaleY, scaleZ, 0, 0, 0);

                Matrix3D toCenter = translationMatrix(-centr.X, -centr.Y, -centr.Z);
                Matrix3D fromCenter = translationMatrix(centr.X, centr.Y, centr.Z);

             
                Matrix3D worldMatrix;
                if (centr_flag)
                {
                    centr_flag = false;
                    worldMatrix = scalingMatr * /*rotationMatr **/ translationMatr /** LRotationMatr * reflMatr*/;
                }
                else
                {
                    worldMatrix = toCenter * scalingMatr * /*rotationMatr * */translationMatr /** LRotationMatr * reflMatr */* fromCenter;
                }


                int clientWidth = e.ClipRectangle.Width;
                int clientHeight = e.ClipRectangle.Height;

                int offsetX = clientWidth / 2;
                int offsetY = clientHeight / 2;

                foreach (var face in pop.Faces) 
                {
                    List<Point> points2D = new List<Point>();
                    foreach (var vertex in face.Vertices) 
                    {
                        point worldPoint = TransformToWorld(vertex, worldMatrix.Matrix);
                        Point projectedPoint = ApplyProjection(worldPoint, projectFunc()); //передаём сюда матрицу перспективы(конверт в 2D)
                        points2D.Add(projectedPoint);
                    }
                    

                    var centeredPoints = points2D.Select(p => new Point(p.X + offsetX, p.Y + offsetY)).ToArray();

                    e.Graphics.DrawPolygon(Pens.Black, centeredPoints);

                }

                //SolidBrush sb = new SolidBrush(Color.FromArgb(255, 255, 0, 0));


                //point worldPoint1 = new point(centr.X, centr.Y, centr.Z);
                //Point projectedPoint2 = ApplyProjection(worldPoint1, projectFunc()); //передаём сюда матрицу перспективы(конверт в 2D)
                    
                //e.Graphics.FillEllipse(sb, projectedPoint2.X + offsetX, projectedPoint2.Y + offsetY, 5, 5);

                    
                
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

        private Matrix3D xRotationMatrix(double a) 
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

        private Matrix3D LRotation(int fi, double l, double m, double n)
        {
            double fiRad = (Math.PI / 180) * fi;
            double cosFi = Math.Cos(fiRad);
            double sinFI = Math.Sin(fiRad);

            return new Matrix3D( new double[,]
            {
                {Math.Pow(l, 2) + cosFi * (1 - Math.Pow(l, 2)), l * (1 - cosFi) * m + n * sinFI,               l * (1 - cosFi) * n - m * sinFI,              0},
                {l * (1 - cosFi) * m - n * sinFI,               Math.Pow(m, 2) + cosFi * (1 - Math.Pow(m, 2)), m * (1 - cosFi ) *  n  + l * sinFI,           0},
                {l * (1 - cosFi) * n + m * sinFI,               m * (1 - cosFi) * n - l * sinFI,               Math.Pow(n,2) + cosFi * (1 - Math.Pow(n, 2)), 0  },
                {0, 0, 0 ,1 }

            });
          
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

            pictureBox1.Invalidate();
            centr_flag = true;
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

        private void button7_Click(object sender, EventArgs e)
        {
            Matrix3D lRotMatr1 = LRotation(Convert.ToInt32(textBox4.Text), Convert.ToDouble(textBox5.Text), Convert.ToDouble(textBox6.Text), Convert.ToDouble(textBox7.Text));
            lRotMatr *= lRotMatr1;
            pictureBox1.Invalidate();
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


    }
}
