using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace ShadesOfGray
{
    public partial class Form2 : Form
    {
        private Bitmap image;
        private Bitmap editedImage;

        public Form2()
        {
            InitializeComponent();

            button_red.Enabled = true;
            button_green.Enabled = true;
            button_blue.Enabled = true;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Image Files(*.BMP;*.JPG;*.GIF;*.PNG)|*.BMP;*.JPG;*.GIF;*.PNG|All files (*.*)|*.*";

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                image = new Bitmap(openFileDialog.FileName);
                pictureBox.Image = image;

                editedImage = new Bitmap(image.Width, image.Height);
            }
        }

        private void ExtractChannel(string color)
        {
            for (int x = 0; x < image.Width; x++)
            {
                for (int y = 0; y < image.Height; y++)
                {
                    Color pixelColor = image.GetPixel(x, y);
                    Color newColor = Color.White;

                    switch (color)
                    {
                        case "red":
                            editedImage.SetPixel(x, y, Color.FromArgb(pixelColor.R, 0, 0));
                            break;
                        case "green":
                            editedImage.SetPixel(x, y, Color.FromArgb(0, pixelColor.G, 0));
                            break;
                        case "blue":
                            editedImage.SetPixel(x, y, Color.FromArgb(0, 0, pixelColor.B));
                            break;
                    }
                }
            }
            
            GenerateColorHistogram(color);

            pictureBox.Image = editedImage;
            pictureBox.Invalidate();
        }

        private void GenerateColorHistogram(string color)
        {
            int[] histogram = new int[256];
            
            for (int x = 0; x < editedImage.Width; x++)
            {
                for (int y = 0; y < editedImage.Height; y++)
                {
                    Color pixel = editedImage.GetPixel(x, y);

                    int value = 0;

                    switch (color)
                    {
                        case "red":
                            value = pixel.R;
                            break;
                        case "green":
                            value = pixel.G;
                            break;
                        case "blue":
                            value = pixel.B;
                            break;
                    }

                    histogram[value]++;
                }
            }
            
            chart.Series[0].Points.Clear();
            for (int i = 0; i < histogram.Length - 1; i++)
            {
                chart.Series[0].Points.Add(histogram[i]);
            }

            switch (color)
            {
                case "red":
                    chart.Series[0].Color = Color.Red;
                    break;
                case "green":
                    chart.Series[0].Color = Color.Green;
                    break;
                case "blue":
                    chart.Series[0].Color = Color.Blue;
                    break;
            }
            chart.Series[0].Name = color;
        }

        private void Form2_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.UserClosing)
            {
                e.Cancel = true;
                Hide();
            }
        }

        private void button_red_Click(object sender, EventArgs e)
        {
            ExtractChannel("red");
        }

        private void button_green_Click(object sender, EventArgs e)
        {
            ExtractChannel("green");
        }

        private void button_blue_Click(object sender, EventArgs e)
        {
            ExtractChannel("blue");
        }
    }
}
