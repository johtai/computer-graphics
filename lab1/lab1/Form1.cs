using System;
using System.Drawing;
using System.Windows.Forms;

namespace lab1
{
    public partial class MainForm : Form
    {
        private Func<double, double> selectedFunction;
        private ComboBox functionSelector;
        private Panel graphPanel;

        public MainForm()
        {
            InitializeComponent();
            selectedFunction = Math.Sin;
        }

        private void InitializeComponent()
        {
            this.functionSelector = new ComboBox();
            this.graphPanel = new Panel();

            this.Text = "Graph";
            this.Size = new Size(800, 600);

            // functionSelector
            this.functionSelector.Items.AddRange(new string[] { "sin(x)", "cos(x)", "x^2", "x^3" });
            this.functionSelector.SelectedIndex = 0;
            this.functionSelector.Dock = DockStyle.Top;
            this.functionSelector.SelectedIndexChanged += FunctionSelectorChanged;

            // graphPanel
            this.graphPanel.Dock = DockStyle.Fill;
            this.graphPanel.Paint += GraphPanel_Paint;
            this.graphPanel.Resize += GraphPanel_Resize;

            this.Controls.Add(this.graphPanel);
            this.Controls.Add(this.functionSelector);
        }

        private void FunctionSelectorChanged(object sender, EventArgs e)
        {
            switch (this.functionSelector.SelectedItem.ToString())
            {
                case "sin(x)":
                    selectedFunction = Math.Sin;
                    break;
                case "cos(x)":
                    selectedFunction = Math.Cos;
                    break;
                case "x^2":
                    selectedFunction = x => x * x;
                    break;
                case "x^3":
                    selectedFunction = x => x * x * x;
                    break;
            }
            graphPanel.Invalidate();
        }

        private void GraphPanel_Paint(object sender, PaintEventArgs e)
        {
            DrawGraph(e.Graphics, graphPanel.ClientSize.Width, graphPanel.ClientSize.Height, selectedFunction);
        }

        private void GraphPanel_Resize(object sender, EventArgs e)
        {
            graphPanel.Invalidate();
        }

        private void DrawGraph(Graphics g, int width, int height, Func<double, double> function)
        {
            g.Clear(Color.White);

            Pen axisPen = new Pen(Color.Black, 2);
            Pen graphPen = new Pen(Color.Blue, 2);

            double xMin = -10;
            double xMax = 10;

            // Масштабирование
            double yMin = double.MaxValue;
            double yMax = double.MinValue;

            for (double x = xMin; x <= xMax; x += 0.01)
            {
                double y = function(x);
                if (y < yMin) yMin = y;
                if (y > yMax) yMax = y;
            }

            double scaleX = width / (xMax - xMin);
            double scaleY = height / (yMax - yMin);

            // Оси
            g.DrawLine(axisPen, 0, height / 2, width, height / 2); 
            g.DrawLine(axisPen, width / 2, 0, width / 2, height);

            // График
            for (int px = 0; px < width; px++)
            {
                double x = xMin + px / scaleX;
                double y = function(x);

                // Преобразование координат в пиксели
                int py = (int)((yMax - y) * scaleY);

                if (px > 0)
                {
                    g.DrawLine(graphPen, px - 1, py, px, py);
                }
            }
        }
    }

}