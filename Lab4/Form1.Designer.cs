namespace Lab4
{
    partial class Form1
    {
        /// <summary>
        /// Обязательная переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором форм Windows

        /// <summary>
        /// Требуемый метод для поддержки конструктора — не изменяйте 
        /// содержимое этого метода с помощью редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.button1 = new System.Windows.Forms.Button();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.button2 = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.dxLabel = new System.Windows.Forms.Label();
            this.dyLabel = new System.Windows.Forms.Label();
            this.chainButton = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.degreesLabel = new System.Windows.Forms.Label();
            this.textBox3 = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.XLabel = new System.Windows.Forms.Label();
            this.YLabel = new System.Windows.Forms.Label();
            this.textBox4 = new System.Windows.Forms.TextBox();
            this.textBox5 = new System.Windows.Forms.TextBox();
            this.drawSecondLineButton = new System.Windows.Forms.Button();
            this.drawPoint = new System.Windows.Forms.Button();
            this.checkButton = new System.Windows.Forms.Button();
            this.classifyButton = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // pictureBox1
            // 
            this.pictureBox1.BackColor = System.Drawing.SystemColors.Control;
            this.pictureBox1.Location = new System.Drawing.Point(403, 15);
            this.pictureBox1.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(881, 599);
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
            this.pictureBox1.Click += new System.EventHandler(this.pictureBox1_Click);
            this.pictureBox1.MouseDown += new System.Windows.Forms.MouseEventHandler(this.pictureBox1_MouseDown);
            // 
            // button1
            // 
            this.button1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button1.Location = new System.Drawing.Point(13, 15);
            this.button1.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(167, 49);
            this.button1.TabIndex = 1;
            this.button1.Text = "Очистить";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // comboBox1
            // 
            this.comboBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Items.AddRange(new object[] {
            "Смещение",
            "Поворот вокруг заданной точки",
            "Поворот вокруг центра",
            "Масштабирование относительно точки",
            "Масштабирование относительно центра"});
            this.comboBox1.Location = new System.Drawing.Point(29, 121);
            this.comboBox1.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(315, 24);
            this.comboBox1.TabIndex = 6;
            this.comboBox1.SelectedIndexChanged += new System.EventHandler(this.comboBox1_SelectedIndexChanged);
            // 
            // textBox1
            // 
            this.textBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox1.Enabled = false;
            this.textBox1.Location = new System.Drawing.Point(89, 202);
            this.textBox1.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(67, 22);
            this.textBox1.TabIndex = 8;
            this.textBox1.Text = "0";
            // 
            // textBox2
            // 
            this.textBox2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox2.Enabled = false;
            this.textBox2.Location = new System.Drawing.Point(239, 202);
            this.textBox2.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.textBox2.Name = "textBox2";
            this.textBox2.Size = new System.Drawing.Size(67, 22);
            this.textBox2.TabIndex = 9;
            this.textBox2.Text = "0";
            // 
            // label2
            // 
            this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label2.AutoSize = true;
            this.label2.Enabled = false;
            this.label2.Location = new System.Drawing.Point(9, 174);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(134, 16);
            this.label2.TabIndex = 10;
            this.label2.Text = "Укажите смещение:";
            // 
            // button2
            // 
            this.button2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button2.Enabled = false;
            this.button2.Location = new System.Drawing.Point(206, 555);
            this.button2.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(158, 59);
            this.button2.TabIndex = 11;
            this.button2.Text = "Применить";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // label4
            // 
            this.label4.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(9, 85);
            this.label4.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(196, 16);
            this.label4.TabIndex = 14;
            this.label4.Text = "Аффинное преобразование:";
            // 
            // dxLabel
            // 
            this.dxLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.dxLabel.AutoSize = true;
            this.dxLabel.Enabled = false;
            this.dxLabel.Location = new System.Drawing.Point(53, 206);
            this.dxLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.dxLabel.Name = "dxLabel";
            this.dxLabel.Size = new System.Drawing.Size(24, 16);
            this.dxLabel.TabIndex = 15;
            this.dxLabel.Text = "dx:";
            // 
            // dyLabel
            // 
            this.dyLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.dyLabel.AutoSize = true;
            this.dyLabel.Enabled = false;
            this.dyLabel.Location = new System.Drawing.Point(203, 206);
            this.dyLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.dyLabel.Name = "dyLabel";
            this.dyLabel.Size = new System.Drawing.Size(25, 16);
            this.dyLabel.TabIndex = 16;
            this.dyLabel.Text = "dy:";
            // 
            // chainButton
            // 
            this.chainButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.chainButton.Enabled = false;
            this.chainButton.Location = new System.Drawing.Point(188, 15);
            this.chainButton.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.chainButton.Name = "chainButton";
            this.chainButton.Size = new System.Drawing.Size(176, 49);
            this.chainButton.TabIndex = 17;
            this.chainButton.Text = "Замкнуть полигон";
            this.chainButton.UseVisualStyleBackColor = true;
            this.chainButton.Click += new System.EventHandler(this.chainButton_Click);
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label1.AutoSize = true;
            this.label1.Enabled = false;
            this.label1.Location = new System.Drawing.Point(9, 242);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(165, 16);
            this.label1.TabIndex = 18;
            this.label1.Text = "Укажите угол поворота:";
            // 
            // degreesLabel
            // 
            this.degreesLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.degreesLabel.AutoSize = true;
            this.degreesLabel.Enabled = false;
            this.degreesLabel.Location = new System.Drawing.Point(87, 276);
            this.degreesLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.degreesLabel.Name = "degreesLabel";
            this.degreesLabel.Size = new System.Drawing.Size(65, 16);
            this.degreesLabel.TabIndex = 19;
            this.degreesLabel.Text = "Градусы:";
            // 
            // textBox3
            // 
            this.textBox3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox3.Enabled = false;
            this.textBox3.Location = new System.Drawing.Point(165, 272);
            this.textBox3.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.textBox3.Name = "textBox3";
            this.textBox3.Size = new System.Drawing.Size(67, 22);
            this.textBox3.TabIndex = 20;
            this.textBox3.Text = "0";
            // 
            // label3
            // 
            this.label3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label3.AutoSize = true;
            this.label3.Enabled = false;
            this.label3.Location = new System.Drawing.Point(9, 314);
            this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(278, 16);
            this.label3.TabIndex = 21;
            this.label3.Text = "Укажите степень масштабирования (в %):";
            // 
            // XLabel
            // 
            this.XLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.XLabel.AutoSize = true;
            this.XLabel.Enabled = false;
            this.XLabel.Location = new System.Drawing.Point(25, 351);
            this.XLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.XLabel.Name = "XLabel";
            this.XLabel.Size = new System.Drawing.Size(65, 16);
            this.XLabel.TabIndex = 22;
            this.XLabel.Text = "По оси X:";
            // 
            // YLabel
            // 
            this.YLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.YLabel.AutoSize = true;
            this.YLabel.Enabled = false;
            this.YLabel.Location = new System.Drawing.Point(203, 351);
            this.YLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.YLabel.Name = "YLabel";
            this.YLabel.Size = new System.Drawing.Size(66, 16);
            this.YLabel.TabIndex = 23;
            this.YLabel.Text = "По оси Y:";
            // 
            // textBox4
            // 
            this.textBox4.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox4.Enabled = false;
            this.textBox4.Location = new System.Drawing.Point(104, 347);
            this.textBox4.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.textBox4.Name = "textBox4";
            this.textBox4.Size = new System.Drawing.Size(67, 22);
            this.textBox4.TabIndex = 24;
            this.textBox4.Text = "100";
            // 
            // textBox5
            // 
            this.textBox5.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox5.Enabled = false;
            this.textBox5.Location = new System.Drawing.Point(277, 347);
            this.textBox5.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.textBox5.Name = "textBox5";
            this.textBox5.Size = new System.Drawing.Size(67, 22);
            this.textBox5.TabIndex = 25;
            this.textBox5.Text = "100";
            // 
            // drawSecondLineButton
            // 
            this.drawSecondLineButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.drawSecondLineButton.Location = new System.Drawing.Point(1324, 15);
            this.drawSecondLineButton.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.drawSecondLineButton.Name = "drawSecondLineButton";
            this.drawSecondLineButton.Size = new System.Drawing.Size(225, 49);
            this.drawSecondLineButton.TabIndex = 26;
            this.drawSecondLineButton.Text = "Добавить второе ребро";
            this.drawSecondLineButton.UseVisualStyleBackColor = true;
            this.drawSecondLineButton.Click += new System.EventHandler(this.drawSecondLineButton_Click);
            // 
            // drawPoint
            // 
            this.drawPoint.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.drawPoint.Location = new System.Drawing.Point(13, 555);
            this.drawPoint.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.drawPoint.Name = "drawPoint";
            this.drawPoint.Size = new System.Drawing.Size(158, 59);
            this.drawPoint.TabIndex = 28;
            this.drawPoint.Text = "Точка";
            this.drawPoint.UseVisualStyleBackColor = true;
            this.drawPoint.Click += new System.EventHandler(this.drawPoint_Click);
            // 
            // checkButton
            // 
            this.checkButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.checkButton.Location = new System.Drawing.Point(1324, 98);
            this.checkButton.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.checkButton.Name = "checkButton";
            this.checkButton.Size = new System.Drawing.Size(225, 47);
            this.checkButton.TabIndex = 29;
            this.checkButton.Text = "Проверка принадлежности точки полигону";
            this.checkButton.UseVisualStyleBackColor = true;
            this.checkButton.Click += new System.EventHandler(this.checkButton_Click);
            // 
            // classifyButton
            // 
            this.classifyButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.classifyButton.Location = new System.Drawing.Point(1324, 174);
            this.classifyButton.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.classifyButton.Name = "classifyButton";
            this.classifyButton.Size = new System.Drawing.Size(225, 48);
            this.classifyButton.TabIndex = 30;
            this.classifyButton.Text = "Классификация точки относительно ребра";
            this.classifyButton.UseVisualStyleBackColor = true;
            this.classifyButton.Click += new System.EventHandler(this.classifyButton_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.ClientSize = new System.Drawing.Size(1576, 629);
            this.Controls.Add(this.classifyButton);
            this.Controls.Add(this.checkButton);
            this.Controls.Add(this.drawPoint);
            this.Controls.Add(this.drawSecondLineButton);
            this.Controls.Add(this.textBox5);
            this.Controls.Add(this.textBox4);
            this.Controls.Add(this.YLabel);
            this.Controls.Add(this.XLabel);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.textBox3);
            this.Controls.Add(this.degreesLabel);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.chainButton);
            this.Controls.Add(this.dyLabel);
            this.Controls.Add(this.dxLabel);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.textBox2);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.comboBox1);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.pictureBox1);
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.Name = "Form1";
            this.Text = "Аффинные преобразования";
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.ComboBox comboBox1;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.TextBox textBox2;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label dxLabel;
        private System.Windows.Forms.Label dyLabel;
        private System.Windows.Forms.Button chainButton;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label degreesLabel;
        private System.Windows.Forms.TextBox textBox3;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label XLabel;
        private System.Windows.Forms.Label YLabel;
        private System.Windows.Forms.TextBox textBox4;
        private System.Windows.Forms.TextBox textBox5;
        private System.Windows.Forms.Button drawSecondLineButton;
        private System.Windows.Forms.Button drawPoint;
        private System.Windows.Forms.Button checkButton;
        private System.Windows.Forms.Button classifyButton;
    }
}

