﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Windows.Forms;


namespace Lab8
{
    public partial class Afins3D : Form
    {
        private static int _spin = 0;
        fileaction f = new fileaction();
        private Polyhedron _polyhedron;
        double d = 5;
        Projection projectionFunction = new Projection();

        int _fi;
        double _l;
        double _m;
        double _n;

        private Matrix _reflection;
        //private Matrix _rotation;
        private double _translationX , _translationY, _translationZ;
        private double _scaleX, _scaleY, _scaleZ;
        private double _rotationX, _rotationY, _rotationZ;

        public Afins3D()
        {
            InitializeComponent();
            Initialize();
            projectionFunction.setProjection(Projection.enumprojection.perspective);
         
           //pictureBox1.MouseMove += Form_MouseMove;
        }
        //private void Form_MouseMove(object sender, MouseEventArgs e)
        //{
        //    MousepositionLabel.Text = $@"X:{e.X}, Y:{e.Y}";
        //}

       
        private void Initialize()
        {
            _translationX = 0; _translationY = 0; _translationZ = 0;
            _scaleX = 1; _scaleY = 1; _scaleZ = 1;
            _rotationX = 0; _rotationY = 0; _rotationZ = 0;

            _reflection =  new Matrix( new double[,]
            {
                {1, 0, 0, 0 },
                {0, 1, 0, 0 },
                {0, 0, 1, 0 },
                {0, 0, 0, 1 }
            });

          //  _rotation = new Matrix (new double[,]
          // {
          //      {1, 0, 0, 0 },
          //      {0, 1, 0, 0 },
          //      {0, 0, 1, 0 },
          //      {0, 0, 0, 1 }
          //});

          //  _lrotationIs = new Matrix(new double[,]
          // {
          //      {1, 0, 0, 0 },
          //      {0, 1, 0, 0 },
          //      {0, 0, 1, 0 },
          //      {0, 0, 0, 1 }
          //});
        }

        // Возвращает геометрический центр полигона
        // Возвращает геометрический центр всего многогранника

        void RadioButton_CheckedChanged(object sender, EventArgs e)
        {
           var radionButton = sender as RadioButton;

            if (radionButton != null && radionButton.Checked)
            {
                if (radionButton.Text == "Перспектива") projectionFunction.setProjection(Projection.enumprojection.perspective);
                if (radionButton.Text == "Изометрия") projectionFunction.setProjection(Projection.enumprojection.isometric);
            }

            pictureBox1.Invalidate();
        }

       

        private void pictureBox1_Paint(object sender, PaintEventArgs e)
        {
            if (_polyhedron == null)
                return;
            // Направление обзора
            var viewDirection = new Vertex(1, 0, -1);


            Matrix translationMatrix = TranslationMatrix(_translationX, _translationY, _translationZ);
            Matrix scalingMatrix = ScalingMatrix(_scaleX, _scaleY, _scaleZ);
            Matrix rotationMatrix = RotationMatrix(_rotationX, _rotationY, _rotationZ);
            Matrix lrotation = LRotation(_fi, _l, _m, _n);
            Vertex centroid = _polyhedron.Centroid(_polyhedron.LocalToWorld);

            Matrix toCenter = TranslationMatrix(-centroid.X, -centroid.Y, -centroid.Z);
            Matrix fromCenter = TranslationMatrix(centroid.X, centroid.Y, centroid.Z);

            Matrix worldMatrix; 
            if (!IsCentroid)
            {
                IsCentroid = true;
                worldMatrix = translationMatrix * scalingMatrix * rotationMatrix * lrotation *  _reflection;
            }
            else
            {
                worldMatrix = toCenter * translationMatrix * scalingMatrix * rotationMatrix * lrotation * _reflection * fromCenter;
            }
            _polyhedron.LocalToWorld *= worldMatrix;

            int clientWidth = e.ClipRectangle.Width;
            int clientHeight = e.ClipRectangle.Height;

            int offsetX = clientWidth / 2;
            int offsetY = clientHeight / 2;

            centroid = _polyhedron.Centroid(_polyhedron.LocalToWorld);
            e.Graphics.FillRectangle(Brushes.Red, (int) centroid.X + offsetX, (int)centroid.Y + offsetY,2, 2);

            var points2D = new List<Point>(10);

            Polyhedron visibleFaces = BackfaceCulling(_polyhedron, viewDirection);
            foreach (Face face in visibleFaces.Faces)
            {
                foreach (Vertex vertex in face.Vertices)
                {
                    Vertex worldVertex = Transformer.TransformToWorld(vertex, _polyhedron.LocalToWorld * projectionFunction.getProjection(), projectionFunction);
                    if (worldMatrix == null) throw new InvalidOperationException("Матрица преобразования некорректна.");
                    points2D.Add(new Point((int)worldVertex.X, (int)worldVertex.Y));
                }

                var centeredPoints = points2D.Select(p => new Point(p.X + offsetX, p.Y + offsetY)).ToArray();
                if(centeredPoints.Length > 0) 
                {                    
                    e.Graphics.DrawPolygon(Pens.Black, centeredPoints);
                }
                points2D.Clear();
            }
        }

        public bool IsCentroid { get; private set; }


        //Матрица перемещения
        private Matrix TranslationMatrix(double tx, double ty, double tz)
        {
            return new Matrix(new[,]
            {
                { 1, 0, 0, 0 },
                { 0, 1, 0, 0 },
                { 0, 0, 1, 0 },
                { tx, ty, tz, 1 }
            });
        }

        //Матрица Масштабирования
        private Matrix ScalingMatrix(double sx, double sy, double sz)
        {

            return new Matrix( new[,]
            {
                {sx, 0, 0 ,0 },
                {0, sy, 0, 0 },
                {0, 0, sz, 0 },
                {0, 0, 0, 1 }
            });

        }

        private Matrix RotationMatrix(double angleX, double angleY, double angleZ)
        {
            //return MultiplyMarices(MultiplyMarices(zRotation(angleZ), yRotation(angleY)), xRotationMatrix(angleX));
            //return MultiplyMarices(MultiplyMarices(xRotationMatrix(angleX), yRotation(angleY)), zRotation(angleZ));
            return XRotationMatrix(angleX) * YRotation(angleY) * ZRotation(angleZ);
            //return MultiplyMarices(zRotation(angleZ), MultiplyMarices(yRotation(angleY), xRotationMatrix(angleX)));
        }

        private Matrix XRotationMatrix(double a)
        {

            double radians = (Math.PI / 180) * a;
            return new Matrix(new[,] {
            { 1,0,0,0 },
            {0, Math.Cos(radians), - Math.Sin(radians), 0 },
            { 0, Math.Sin(radians), Math.Cos(radians ), 0},
            {0, 0 ,0, 1 }
            });

        }

        private Matrix YRotation(double a)
        {
            double radians = (Math.PI / 180) * a;
            var cosx = Math.Cos(radians);
            var sinx = Math.Sin(radians);

            return new Matrix(new double[,] {
            {cosx, 0 , sinx, 0 },
            {0, 1, 0, 0 },
            {-sinx, 0, cosx, 0 },
            {0, 0, 0, 1 }
            });

        }

        private Matrix ZRotation(double a)
        {
            double radians = (Math.PI / 180) * a;
            var cosx = Math.Cos(radians);
            var sinx = Math.Sin(radians);

            return new Matrix( new double[,] {
            {cosx, -sinx, 0, 0  },
            {sinx, cosx, 0, 0  },
            {0, 0, 1, 0 },
            {0, 0, 0, 1 }
            });
        }

        private Matrix LRotation(int fi, double l, double m, double n)
        {
            double fiRad = (Math.PI / 180) * fi;
            double cosFi = Math.Cos(fiRad);
            double sinFI = Math.Sin(fiRad);

            return new Matrix( new double[,]
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
                    _polyhedron = Polyhedron.Tetrahedron();
                    pictureBox1.Invalidate();
                    break;
                case "Гексаэдр":
                    _polyhedron = Polyhedron.Hexahedron();
                    pictureBox1.Invalidate();
                    break;
                case "Октаэдр":
                    _polyhedron = Polyhedron.Octahedron();
                    pictureBox1.Invalidate();
                    break;

                case "Икосаэдр":
                    _polyhedron = Polyhedron.Icosahedron();
                    pictureBox1.Invalidate();
                    break;

                case "Додекаэдр":
                    _polyhedron = Polyhedron.Dodecahedron();
                    pictureBox1.Invalidate();
                    break;
            }

        }

        private void button2_Click(object sender, EventArgs e)
        {
            _translationX = Convert.ToInt32(dxBox.Text);
            _translationY = Convert.ToInt32(dyBox.Text);
            _translationZ = Convert.ToInt32(dzBox.Text);

            pictureBox1.Invalidate();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            var dxScale = Convert.ToDouble(textBox1.Text, new NumberFormatInfo() { NumberDecimalSeparator = "."});
            var dyScale = Convert.ToDouble(textBox2.Text, new NumberFormatInfo() { NumberDecimalSeparator = "." });
            var dzScale = Convert.ToDouble(textBox3.Text, new NumberFormatInfo() { NumberDecimalSeparator = "." });

            _scaleX = dxScale;
            _scaleY = dyScale;
            _scaleZ = dzScale;

            IsCentroid = false;

            pictureBox1.Invalidate();
        }

        private void button4_Click(object sender, EventArgs e)
            {
                switch(_spin)
                {
                    case 0:
                        //xRotation(3);
                        break;
                    case 1:
                        YRotation(3);
                        break;
                    case 2:
                        ZRotation(3);
                        break;
                }
            }

        private void ButtonLoad_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.Cancel) 
            {
                return;
            }
            string filename = openFileDialog1.FileName;
            //string filetext = System.IO.File.ReadAllText(filename);
            _polyhedron = f.LoadFromOBJ(filename);
            pictureBox1.Invalidate();
            
            //MessageBox.Show("asd");
        }

        private void Save_Click(object sender, EventArgs e)
        {
            // Пример фигуры вращения
            var profile = new List<Vertex> {
                new Vertex(0, 0, 0),
                new Vertex(0, 1, 0),
                new Vertex(0, 2, 1),
            };

            var revolutionFigure = FigureBuilder.BuildRevolutionFigure(profile, "z", 500);
            var fileAction = new fileaction();
            fileAction.SaveToOBJ(revolutionFigure, "revolution.obj");

            // Пример графика функции
            Func<double, double, double> func = (x, y) => Math.Sin(Math.Sqrt(x * x + y * y));
            var surface = FigureBuilder.BuildSurface(func, -5, 5, -5, 5, 20);
            fileAction.SaveToOBJ(surface, "surface.obj");
        }

        public void radioButtonSwitch(object sender, EventArgs e)
        {
            var rbt = sender as RadioButton;

            if (rbt != null && rbt.Checked)
            {
                switch (rbt.Text)
                {

                    case "XAxis":
                       _spin = 0;
                        break;
                    case "YAxis":
                        _spin = 1;
                        break;
                    case "ZAxis":
                        _spin = 2;
                        break;

                }

            }

        }


        private void button4_Click_1(object sender, EventArgs e)
        {

                if (radioButton6.Checked)
                {

                    var reflXYMatr = new Matrix( new double[,]
                    {
                            {1, 0, 0, 0 },
                            {0, 1, 0, 0 },
                            {0, 0, -1, 0 },
                            {0, 0, 0, 1 }
                    });

                    //reflMatr = MultiplyMarices(reflMatr, reflXYMatr);
                    MessageBox.Show("Hy");
                    _reflection = _reflection * reflXYMatr;

                    pictureBox1.Invalidate();


                }
                else if (radioButton7.Checked)
                {

                    var reflXZMatr = new Matrix( new double[,]
                    {
                            {1, 0, 0, 0 },
                            {0, -1, 0, 0 },
                            {0, 0, 1, 0 },
                            {0, 0, 0, 1 }
                    });

                    //reflMatr = MultiplyMarices(reflMatr, reflXZMatr);
                    _reflection = _reflection * reflXZMatr;
                    pictureBox1.Invalidate();
                }
                else if (radioButton8.Checked)
                {

                    var reflYZMatr = new Matrix( new double[,]
                    {
                            {-1, 0, 0, 0 },
                            {0, 1, 0, 0 },
                            {0, 0, 1, 0 },
                            {0, 0, 0, 1 }
                    });

                    //reflMatr = MultiplyMarices(reflMatr, reflYZMatr);
                    _reflection = _reflection * reflYZMatr;

                    pictureBox1.Invalidate();
                }

        }

        private void button7_Click(object sender, EventArgs e)
        {
                _fi = Convert.ToInt32(textBox4.Text);
                _l = Convert.ToInt32(textBox5.Text);
                _m = Convert.ToInt32(textBox6.Text);
                _n = Convert.ToInt32(textBox7.Text);
            //    Matrix lRotMatr1 = LRotation(Convert.ToInt32(textBox4.Text), Convert.ToDouble(textBox5.Text), Convert.ToDouble(textBox6.Text), Convert.ToDouble(textBox7.Text));
            //_lrotationIs = lRotMatr1;
                //_rotation *= lRotMatr1;
            pictureBox1.Invalidate();
        }

        private void switchReflectionRadioButton(object sender, EventArgs e)
        {

        }

        private void button5_Click_1(object sender, EventArgs e)
        {
            try
            {
                    switch (_spin) 
                    {

                        case 0:
                            _rotationX = Convert.ToInt32(rotationBox.Text);
                            _rotationY = 0;
                            _rotationZ = 0;
                            break;
                        case 1:
                            _rotationY = Convert.ToInt32(rotationBox.Text);
                            _rotationX = 0;
                            _rotationZ = 0;
                            break;
                        case 2:
                            _rotationZ = Convert.ToInt32(rotationBox.Text);
                            _rotationX = 0;
                            _rotationY = 0;
                            break;


                    }
                pictureBox1.Invalidate();
            }
            catch (Exception)
            {
                MessageBox.Show("Введите корректные значения!");
            }

        }
        private void button6_Click_1(object sender, EventArgs e)
        {
            var dxScale = Convert.ToDouble(textBox1.Text, new NumberFormatInfo() { NumberDecimalSeparator = "." });
            var dyScale = Convert.ToDouble(textBox2.Text, new NumberFormatInfo() { NumberDecimalSeparator = "." });
            var dzScale = Convert.ToDouble(textBox3.Text, new NumberFormatInfo() { NumberDecimalSeparator = "." });

            _scaleX = dxScale;
            _scaleY = dyScale;
            _scaleZ = dzScale;

            //(scaleXCenter, scaleYCenter, scaleZCenter) = Centroid();

            pictureBox1.Invalidate();
        }


        public static Polyhedron BackfaceCulling(Polyhedron polyhedron, Vertex viewDirection)
        {
            //Если нормалей нет, возвращаем исходный polyhedron
            if (polyhedron.Faces[0].Normales.Count == 0) return polyhedron;

            var visibleFaces = new List<Face>();
            var visibleVertices = new List<Vertex>();
            foreach (var face in polyhedron.Faces)
            {
                var normal = face.Normales[0];

                double dotProduct = normal.X * viewDirection.X + normal.Y * viewDirection.Y + normal.Z * viewDirection.Z;

                //Если грань "лицевая", добавляем её в список видимых
                if (dotProduct < 0) 
                {
                    visibleFaces.Add(face);
                    foreach (var vertex in face.Vertices) 
                    {
                        visibleVertices.Add(vertex);
                    }
                }
                  

            }
            return new Polyhedron(visibleVertices.ToList(), visibleFaces);
        }

    }

    public class Transformer
    {   
        
        public static Vertex TransformToWorld(Vertex vertex, Matrix matrix, Projection projection)
        {
            var point = new Matrix(new[,] { { vertex.X, vertex.Y, vertex.Z, 1 } });
            point *= matrix;

            var result = new Vertex(point[0, 0], point[0, 1], point[0, 2]);

            if (projection.getEnumProjection() == Projection.enumprojection.perspective)
            {
                return result / point[0, 3];
            }

            return result;
            //return new Vertex(point[0, 0], point[0, 1], point[0, 2]);
        }
    }


    public class Projection
    {
        //private Func<Matrix> _projectionFunction { get; set; }
        private Matrix _projMatrix;
        public enum enumprojection
        {
            none,
            perspective,
            isometric
        }
        enumprojection en = enumprojection.perspective;
        public void setProjection(enumprojection proj)
        {

            switch (proj)
            {
                case enumprojection.perspective:
                    _projMatrix = PerspectiveMatrix();
                    break;
                case enumprojection.isometric:
                    _projMatrix = IsometricMatrix();
                    break;
            }
        }
        public enumprojection getEnumProjection()
        {
            return en;
        }

        public Matrix getProjection() { return _projMatrix; }

        //Перспективная проекция
        private Matrix PerspectiveMatrix()
        {
            return new Matrix(new[,]
            {
                { 1, 0, 0, 0 },
                { 0, 1, 0, 0 },
                { 0, 0, 1, -1/-300d },
                { 0, 0, 0 ,1 }
            });
        }

        private Matrix IsometricMatrix()
        {
            //return new Matrix(new[,]
            //{
            //    { Math.Sqrt(3), 0, -Math.Sqrt(3), 0 },
            //    { 1, 2, 1, 0 },
            //    { Math.Sqrt(2), -Math.Sqrt(2), Math.Sqrt(2), 0 },
            //    { 0, 0, 0, 1 }
            //});

            return new Matrix(new[,]
{
                {Math.Sqrt(3), 1, Math.Sqrt(2), 0 },
                {0, 2, -Math.Sqrt(2), 0 },
                {-Math.Sqrt(3), 1, Math.Sqrt(2), 0 },
                {0, 0 ,0, 1 }
            });


        }

    }

}
