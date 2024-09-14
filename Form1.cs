using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace ShadesOfGray
{
    public partial class Form1 : Form
    {
        int[] hist1 = new int[256];
        int[] hist2 = new int[256];

        bool isOppened = false;
        public Form1()
        {
            InitializeComponent();
            
        }




        private void openButton_Click(object sender, EventArgs e)
        {

            OpenFileDialog ofd = new OpenFileDialog();

            if (ofd.ShowDialog() == DialogResult.OK) 
            {

                try
                {
                    pictureBox1.Image = new Bitmap(ofd.FileName);
                }
                catch 
                {
                    MessageBox.Show("Невозможно открыть выбранный файл");
                }
                
                isOppened = true;
            }
        }

        private void convertButton_Click(object sender, EventArgs e)
        {

            if(isOppened == false) 
            {
                MessageBox.Show("Нечего преобразовывать!");
                return;
            }


            //NTSC RGB
            Bitmap input_b = new Bitmap(pictureBox1.Image);
            int Y;
            for (int x = 0; x < input_b.Width; ++x) 
            {

                for (int y = 0; y < input_b.Height; ++y) 
                {   
                    
                    Color pixelColor = input_b.GetPixel(x, y);
                    Y = (int)(0.299 * pixelColor.R + 0.587 * pixelColor.G + 0.114 * pixelColor.B);
                    Color newColor = Color.FromArgb(Y, Y, Y);
                    input_b.SetPixel(x, y, newColor);

                    hist1[Y] += 1;
                }

            }
            pictureBox2.Image = input_b;

            //sRGB.
            Bitmap inputSRGB = new Bitmap(pictureBox1.Image);
            for (int x = 0; x < inputSRGB.Width; x++)
            {

                for(int y = 0; y < inputSRGB.Height; y++) 
                {

                    Color pixelColor2 = inputSRGB.GetPixel(x, y);
                    Y = (int)(0.2126 * pixelColor2.R + 0.7152 * pixelColor2.G + 0.0722 * pixelColor2.B);
                    Color newColor2 = Color.FromArgb(Y, Y, Y);
                    inputSRGB.SetPixel(x, y, newColor2);

                    hist2[Y] += 1;
                }

            }
            pictureBox3.Image = inputSRGB;

            //Diffrend between NTSC RGB and sRGB.
            Bitmap inputDiff = new Bitmap(pictureBox1.Image);
            for (int x = 0; x < inputDiff.Width; x++) 
            {

                for (int y = 0; y < inputDiff.Height; y++) 
                {

                    Color pixelColorOne = input_b.GetPixel(x, y);
                    Color pixelColorTwo = inputSRGB.GetPixel(x, y);
                    int diff = Math.Abs(pixelColorOne.R - pixelColorTwo.R);
                    Color newColor = Color.FromArgb(diff, diff, diff);
                    inputDiff.SetPixel(x, y, newColor);
                }

            }

            pictureBox4.Image = inputDiff;

            //Рисуем Гистограмы для двух Цветовых схем 
            DrawHystogram(hist1, chart1, "Оттенки серого 1");
            DrawHystogram(hist2, chart2, "Оттенки серого 2");
            
        }



        private void DrawHystogram(int[] histogram, Chart chart, string seriesName) 
        {

            chart.Series[seriesName].Points.Clear();
            for (int i = 0; i < histogram.Length; i++) 
            {

                chart.Series[seriesName].Points.AddXY(i, histogram[i]);
            }
        }


    }
}
