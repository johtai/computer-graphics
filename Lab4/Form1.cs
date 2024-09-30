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
        private bool isPolygonClosed = false;
        
        
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


    }
}
