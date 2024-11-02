using System.Windows.Forms;

namespace var10
{
    partial class Form1
    {
        private Button btnDrawPolygon1;
        private Button btnDrawPolygon2;
        private Button btnUnionPolygons;
        private Panel drawingPanel;

        private void InitializeComponent()
        {
            this.btnDrawPolygon1 = new Button();
            this.btnDrawPolygon2 = new Button();
            this.btnUnionPolygons = new Button();
            this.drawingPanel = new Panel();

            this.drawingPanel.MouseMove += new MouseEventHandler(MainForm_MouseMove);
            // 
            // btnDrawPolygon1
            // 
            this.btnDrawPolygon1.Location = new System.Drawing.Point(12, 12);
            this.btnDrawPolygon1.Size = new System.Drawing.Size(120, 23);
            this.btnDrawPolygon1.Text = "Draw Polygon 1";
            this.btnDrawPolygon1.Click += new System.EventHandler(this.btnDrawPolygon1_Click);

            // 
            // btnDrawPolygon2
            // 
            this.btnDrawPolygon2.Location = new System.Drawing.Point(12, 41);
            this.btnDrawPolygon2.Size = new System.Drawing.Size(120, 23);
            this.btnDrawPolygon2.Text = "Draw Polygon 2";
            this.btnDrawPolygon2.Click += new System.EventHandler(this.btnDrawPolygon2_Click);

            // 
            // btnUnionPolygons
            // 
            this.btnUnionPolygons.Location = new System.Drawing.Point(12, 70);
            this.btnUnionPolygons.Size = new System.Drawing.Size(120, 23);
            this.btnUnionPolygons.Text = "Union Polygons";
            this.btnUnionPolygons.Click += new System.EventHandler(this.btnUnionPolygons_Click);

            // 
            // drawingPanel
            // 
            this.drawingPanel.Location = new System.Drawing.Point(150, 12);
            this.drawingPanel.Size = new System.Drawing.Size(500, 500);
            this.drawingPanel.BorderStyle = BorderStyle.FixedSingle;
            this.drawingPanel.MouseClick += new MouseEventHandler(this.drawingPanel_MouseClick);
            this.drawingPanel.Paint += new PaintEventHandler(this.drawingPanel_Paint);

            // 
            // Form1
            // 
            this.ClientSize = new System.Drawing.Size(664, 531);
            this.Controls.Add(this.btnDrawPolygon1);
            this.Controls.Add(this.btnDrawPolygon2);
            this.Controls.Add(this.btnUnionPolygons);
            this.Controls.Add(this.drawingPanel);
            this.Text = "Convex Polygon Union";
        }
    }
}
