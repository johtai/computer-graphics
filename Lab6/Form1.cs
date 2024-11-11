using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Affine_transformations_in_space
{
    public partial class Afins3D : Form
    {

        polyhedron pop;
        public Func<point, Point> projectFunc;

        public static List<polygon> pnts;

        public static int scale = 100;
        public static int spin = 0;
        
        public static double focalLength = 500; //Глубина
        public Afins3D()
        {
            InitializeComponent();
            projectFunc = Isometric2DPoint;


        }


        //public void drawTetrahedron()
        //{
        //    pop = polyhedron.drawTetraedr();
        //    pictureBox1.Invalidate();
        //    pnts = pop.Faces;


        //}

        private point chelnok(polygon face) 
        {
            double sumX = 0; double sumY = 0; double sumZ = 0;
            
            foreach (var vertex in face.Vertices)
            {
                sumX += vertex.X;
                sumY += vertex.Y;
                sumZ += vertex.Z;

            }
            sumX /= face.Vertices.Count;
            sumY /= face.Vertices.Count;
            sumZ /= face.Vertices.Count;
            return new point(sumX, sumY, sumZ);
        }
        // Возвращает геометрический центр полигона
        // Возвращает геометрический центр всего многогранника
        private point Centroid()
        {
           
            int totalVertices = 0;
            double centerX = 0; double centerY = 0; double centerZ = 0;
            foreach (polygon face in pnts)
            {
                
                
                centerX += chelnok(face).X;
                centerY += chelnok(face).Y;
                centerZ += chelnok(face).Z;

            }

             centerX /= pnts.Count; 
             centerY /= pnts.Count;
             centerZ /= pnts.Count;

            MessageBox.Show("Центр по X: " + centerX.ToString());
            MessageBox.Show("Центр по Y: " + centerY.ToString());
            MessageBox.Show("Центр по Z: " + centerZ.ToString());

            return new point(centerX, centerY, centerZ);
        }


        void RadioButton_CheckedChanged(object sender, EventArgs e) 
        {
           var radionButton = sender as RadioButton;

            if (radionButton != null && radionButton.Checked) 
            {
                if (radionButton.Text == "Перспектива") projectFunc = PointTo2D;
                if (radionButton.Text == "Изометрия") projectFunc = Isometric2DPoint;
            }
            
            pictureBox1.Invalidate();         
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
                point v1 = new point(1 * scale, 1 * scale, 1 * scale);
                point v2 = new point(1 * scale, -1 * scale, -1 * scale);
                point v3 = new point(-1 * scale, 1 * scale, -1 * scale);
                point v4 = new point(-1 * scale, -1 * scale, 1 * scale) ;

                polygon firstPol = new polygon(new List<point> { v1, v2, v3 });
                polygon secondPol = new polygon(new List<point> { v1, v2, v4 });
                polygon thirdPol = new polygon(new List<point> {v1, v3, v4 });
                polygon fourthPol = new polygon(new List<point> {v2, v3, v4 });

                return new polyhedron(new List<point> { v1, v2, v3, v4 }, new List<polygon> {firstPol, secondPol, thirdPol, fourthPol });             
            }

            public static polyhedron drawGexaedr() 
            {
                point v1 = new point(-1 * scale, -1 * scale, -1 * scale);
                point v2 = new point(1 * scale, -1 * scale, -1 * scale);
                point v3 = new point(-1 * scale, 1 * scale, -1 * scale);
                point v4 = new point(1 * scale, 1 * scale, -1 * scale);              
                point v5 = new point(-1 * scale, -1 * scale,  1 * scale);
                point v6 = new point(1 * scale, -1 * scale, 1 * scale);
                point v7 = new point(-1 * scale, 1 * scale, 1 * scale);
                point v8 = new point(1 * scale, 1 * scale, 1 * scale);

                polygon firstPol = new polygon(new List<point> { v1, v2, v4, v3 }); // Нижняя грань
                polygon secondPol = new polygon(new List<point> { v5, v6, v8, v7 }); // Верхняя грань
                polygon thirdPol = new polygon(new List<point> { v1, v3, v7, v5 }); // Левая грань
                polygon fourdPol = new polygon(new List<point> { v2, v4, v8, v6 }); // Правая грань
                polygon fivethPol = new polygon(new List<point> { v1, v2, v6, v5 }); // Передняя грань
                polygon sixthPol = new polygon(new List<point> { v3, v4, v8, v7 });  // Задняя грань

                return new polyhedron(new List<point> { v1, v2, v3, v4, v5, v6, v7, v8 }, new List<polygon> {firstPol, secondPol, thirdPol, fourdPol, fivethPol, sixthPol });
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
                double phi = (1 + Math.Sqrt(5)) / 2;  // Золотое сечение
                double scale = 1;  // Масштаб для вершин

                List<point> vertices = new List<point>
        {
            new point(-1 * scale, -1* scale , -1* scale),
            new point(-1* scale, -1* scale,  1* scale),
            new point(-1* scale,  1* scale, -1* scale),
            new point(-1* scale,  1* scale,  1* scale),
            new point( 1* scale, -1* scale, -1* scale),
            new point( 1* scale, -1* scale,  1* scale),
            new point( 1* scale,  1* scale, -1* scale),
            new point( 1* scale,  1* scale,  1* scale),

            new point(0, -1/phi * scale, -phi * scale),
            new point(0, -1/phi * scale,  phi * scale),
            new point(0,  1/phi * scale, -phi * scale),
            new point(0,  1/phi * scale,  phi * scale),

            new point(-1/phi * scale, -phi * scale, 0),
            new point(-1/phi * scale,  phi * scale, 0),
            new point(1/phi * scale, -phi * scale, 0),
            new point(1/phi * scale,  phi * scale, 0),

            new point(-phi * scale, 0, -1/phi * scale),
            new point( phi * scale, 0, -1/phi * scale),
            new point(-phi * scale, 0,  1/phi * scale),
            new point( phi * scale, 0,  1/phi * scale)
        };

                // Определение 12 пятиугольных граней
                List<polygon> faces = new List<polygon>
        {
            new polygon(new List<point> { vertices[0], vertices[8], vertices[4], vertices[14], vertices[12] }),
            new polygon(new List<point> { vertices[0], vertices[12], vertices[2], vertices[13], vertices[10] }),
            new polygon(new List<point> { vertices[0], vertices[10], vertices[6], vertices[16], vertices[8] }),
            new polygon(new List<point> { vertices[1], vertices[9], vertices[5], vertices[14], vertices[8] }),
            new polygon(new List<point> { vertices[1], vertices[8], vertices[16], vertices[11], vertices[3] }),

            new polygon(new List<point> { vertices[1], vertices[3], vertices[12], vertices[4], vertices[9] }),
            new polygon(new List<point> { vertices[2], vertices[10], vertices[17], vertices[18], vertices[13] }),
            new polygon(new List<point> { vertices[2], vertices[13], vertices[3], vertices[11], vertices[19] }),
            new polygon(new List<point> { vertices[3], vertices[19], vertices[7], vertices[15], vertices[9] }),
            new polygon(new List<point> { vertices[4], vertices[9], vertices[15], vertices[5], vertices[14] }),

            new polygon(new List<point> { vertices[5], vertices[15], vertices[7], vertices[6], vertices[17] }),
            new polygon(new List<point> { vertices[6], vertices[17], vertices[10], vertices[18], vertices[16] })
        };

                return new polyhedron(vertices, faces);
            }


        }


       


        //перспективная проекция
        private Point PointTo2D(point p) 
        {
            int x2D = (int)(focalLength * p.X / (p.Z + focalLength));
            int y2D = (int)(focalLength * p.Y / (p.Z + focalLength));

            x2D += pictureBox1.Width / 2;
            y2D += pictureBox1.Height / 2;

            return new Point(x2D, y2D);
        }

        //Изометрическая проекция
        private Point Isometric2DPoint(point p ) 
        {
            double angleCos = Math.Cos(Math.PI / 6);
            double angleSin = Math.Sin(Math.PI / 6);

            int x2D = (int)((p.X - p.Z) * angleCos);
            int y2D = (int)((p.Y - (p.X + p.Z) * angleSin) * -1);

            x2D += pictureBox1.Width / 2;
            y2D += pictureBox1.Height / 2;

            return new Point(x2D,y2D);
        }

        private void pictureBox1_Paint(object sender, PaintEventArgs e)
        {
            if(pop != null) 
            {
               
                for (int i = 0; i < pop.Faces.Count(); i++)
                {
                    // Vertices.Select(v => Isometric2DPoint(v)) // Пожарный гидрант(на всякий)

                    Point[] points2D = pop.Faces[i].Vertices.Select(projectFunc).ToArray();
                    e.Graphics.DrawPolygon(Pens.Black, points2D);                   
                }
            }            
            

        }

        private void scaleFigure(point p) 
        {

            double[,] matr = new double[,] {
                {p.X, 0, 0 ,0 },
                {0, p.Y, 0, 0 },
                {0, 0, p.Z, 0 },
                {0, 0, 0, 1 }
            };

            multMatr(matr);
        }

        private void multMatr(double[,] transformationMatrix) 
        {
           
           
            for (int i = 0; i < pop.Faces.Count; i++) 
            {
                for (int j = 0; j < pop.Faces[i].Vertices.Count; j++) 
                {
                    var po = pop.Faces[i].Vertices[j];
                    double newX =  po.X * transformationMatrix[0, 0] + po.Y * transformationMatrix[1, 0] + po.Z * transformationMatrix[2, 0] + transformationMatrix[3, 0];
                    double newY =  po.X * transformationMatrix[0, 1] + po.Y * transformationMatrix[1, 1] + po.Z * transformationMatrix[2, 1] + transformationMatrix[3, 1];
                    double newZ =  po.X * transformationMatrix[0, 2] + po.Y * transformationMatrix[1, 2] + po.Z * transformationMatrix[2, 2] + transformationMatrix[3, 2];

                    
                    pop.Faces[i].Vertices[j] = new point(newX, newY, newZ);
                }


            }
            pictureBox1.Invalidate();

        }


        private void translation(int dx, int dy, int dz)
        {
            double[,] translationMatrix = new double[,]
            {
                {1, 0, 0, 0 },
                {0, 1, 0, 0 },
                {0, 0 ,1, 0 },
                {dx, dy, dz, 1 }
            };



            multMatr(translationMatrix);
        }


        private void xRotation(double a) 
        {

            double radians = (Math.PI / 180) * a;
            double[,] matr = new double[,] {
            { 1,0,0,0 },
            {0,Math.Cos(radians),Math.Sin(radians),0 },
            { 0,-Math.Sin(radians),Math.Cos(radians ),0},
            {0, 0 ,0, 1 }
            };

            multMatr(matr);
        }

        private void yRotation(double a) 
        {
            double radians = (Math.PI / 180) * a;
            var cosx = Math.Cos(radians);
            var sinx = Math.Sin(radians);

            double[,] matr = new double[,] {
            {cosx, 0 , -sinx, 0 },
            {0, 1, 0, 0 },
            {sinx, 0, cosx, 0 },
            {0, 0, 0, 1 }
            };

            multMatr(matr);

        }

        private void zRotation(double a) 
        {
            double radians = (Math.PI / 180) * a;
            var cosx = Math.Cos(radians);
            var sinx = Math.Sin(radians);

            double[,] matr = new double[,] {
            {cosx, sinx, 0, 0  },
            {-sinx, cosx, 0, 0  },
            {0, 0, 1, 0 },
            {0, 0, 0, 1 }
            };

            multMatr(matr);
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
            try {
                translation(Convert.ToInt32(dxBox.Text), Convert.ToInt32(dyBox.Text), Convert.ToInt32(dzBox.Text));
            } catch (Exception ex){
                MessageBox.Show("Впишите корректные значения смещения!");
            }
                    
        }

        private void button3_Click(object sender, EventArgs e)
        {
            
            var dxScale = Convert.ToDouble(textBox1.Text, new NumberFormatInfo() { NumberDecimalSeparator = "."});
            var dyScale = Convert.ToDouble(textBox2.Text, new NumberFormatInfo() { NumberDecimalSeparator = "." });
            var dzScale = Convert.ToDouble(textBox3.Text, new NumberFormatInfo() { NumberDecimalSeparator = "." });
            point p1 = new point(dxScale, dyScale, dzScale);
            scaleFigure(p1);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            switch(spin) 
            {
                case 0:
                    xRotation(3);
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

            var rb = sender as RadioButton;

            if (rb.Checked) 
            {
                switch (rb.Text) 
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
            double[,] translationMatrix = new double[,]
            {
                {1, 0, 0, 0 },
                {0, 1, 0, 0 },
                {0, 0, 1, 0 },
                {0, 0, 0, 1 }
            };

            if (radioButton6.Checked)
                {
                MessageBox.Show("XY");
                translationMatrix = new double[,]
                    {
                        {1, 0, 0, 0 },
                        {0, 1, 0, 0 },
                        {0, 0, -1, 0 },
                        {0, 0, 0, 1 }
                    };
                }
            else if (radioButton7.Checked)
            {
                MessageBox.Show("XZ");
                translationMatrix = new double[,]
                {
                        {1, 0, 0, 0 },
                        {0, -1, 0, 0 },
                        {0, 0, 1, 0 },
                        {0, 0, 0, 1 }
                };
            }
            else if (radioButton8.Checked)
            {
                MessageBox.Show("YZ");
                translationMatrix = new double[,]
                {
                        {-1, 0, 0, 0 },
                        {0, 1, 0, 0 },
                        {0, 0, 1, 0 },
                        {0, 0, 0, 1 }
                };
            }
            multMatr(translationMatrix);
        }

        private void button5_Click_1(object sender, EventArgs e)
        {
            try 
            {
                
                double rbb = Convert.ToDouble(rotationBox.Text, new NumberFormatInfo() { NumberDecimalSeparator = "." });

                switch (spin)
                {
                    case 0:
                        xRotation(rbb);
                        break;
                    case 1:
                        yRotation(rbb);
                        break;
                    case 2:
                        zRotation(rbb);
                        break;
                }


            }
            catch (Exception ex) { MessageBox.Show("Введите корректные значения!"); }
            
        }


        private void button6_Click(object sender, EventArgs e)
        {
            scaleFigure(Centroid());
        }
    }
}
