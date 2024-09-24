namespace Laba3
{
    partial class Task3
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
            this.label13 = new System.Windows.Forms.Label();
            this.label14 = new System.Windows.Forms.Label();
            this.label15 = new System.Windows.Forms.Label();
            this.button_point1 = new System.Windows.Forms.Button();
            this.button_point2 = new System.Windows.Forms.Button();
            this.button_point3 = new System.Windows.Forms.Button();
            this.colorDialog1 = new System.Windows.Forms.ColorDialog();
            this.button_gradient = new System.Windows.Forms.Button();
            this.button_clean = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // pictureBox1
            // 
            this.pictureBox1.BackColor = System.Drawing.Color.White;
            this.pictureBox1.Location = new System.Drawing.Point(11, 82);
            this.pictureBox1.Margin = new System.Windows.Forms.Padding(2);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(865, 523);
            this.pictureBox1.TabIndex = 1;
            this.pictureBox1.TabStop = false;
            this.pictureBox1.Click += new System.EventHandler(this.pictureBox1_Click);
            this.pictureBox1.MouseEnter += new System.EventHandler(this.pictureBox1_MouseEnter);
            this.pictureBox1.MouseLeave += new System.EventHandler(this.pictureBox1_MouseLeave);
            this.pictureBox1.MouseMove += new System.Windows.Forms.MouseEventHandler(this.pictureBox1_MouseMove);
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.label13.Location = new System.Drawing.Point(11, 11);
            this.label13.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(64, 17);
            this.label13.TabIndex = 29;
            this.label13.Text = "Точка 1:";
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.label14.Location = new System.Drawing.Point(73, 11);
            this.label14.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(64, 17);
            this.label14.TabIndex = 30;
            this.label14.Text = "Точка 2:";
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.label15.Location = new System.Drawing.Point(136, 11);
            this.label15.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(64, 17);
            this.label15.TabIndex = 31;
            this.label15.Text = "Точка 3:";
            // 
            // button_point1
            // 
            this.button_point1.BackColor = System.Drawing.SystemColors.ControlText;
            this.button_point1.Location = new System.Drawing.Point(23, 30);
            this.button_point1.Margin = new System.Windows.Forms.Padding(2);
            this.button_point1.Name = "button_point1";
            this.button_point1.Size = new System.Drawing.Size(32, 35);
            this.button_point1.TabIndex = 32;
            this.button_point1.UseVisualStyleBackColor = false;
            this.button_point1.Click += new System.EventHandler(this.button_point_Click);
            // 
            // button_point2
            // 
            this.button_point2.BackColor = System.Drawing.SystemColors.ControlText;
            this.button_point2.Location = new System.Drawing.Point(84, 30);
            this.button_point2.Margin = new System.Windows.Forms.Padding(2);
            this.button_point2.Name = "button_point2";
            this.button_point2.Size = new System.Drawing.Size(32, 35);
            this.button_point2.TabIndex = 33;
            this.button_point2.UseVisualStyleBackColor = false;
            this.button_point2.Click += new System.EventHandler(this.button_point_Click);
            // 
            // button_point3
            // 
            this.button_point3.BackColor = System.Drawing.SystemColors.ControlText;
            this.button_point3.Location = new System.Drawing.Point(146, 30);
            this.button_point3.Margin = new System.Windows.Forms.Padding(2);
            this.button_point3.Name = "button_point3";
            this.button_point3.Size = new System.Drawing.Size(32, 35);
            this.button_point3.TabIndex = 34;
            this.button_point3.UseVisualStyleBackColor = false;
            this.button_point3.Click += new System.EventHandler(this.button_point_Click);
            // 
            // button_gradient
            // 
            this.button_gradient.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F);
            this.button_gradient.Location = new System.Drawing.Point(720, 8);
            this.button_gradient.Margin = new System.Windows.Forms.Padding(2);
            this.button_gradient.Name = "button_gradient";
            this.button_gradient.Size = new System.Drawing.Size(154, 67);
            this.button_gradient.TabIndex = 35;
            this.button_gradient.Text = "Градиент";
            this.button_gradient.UseVisualStyleBackColor = true;
            this.button_gradient.Click += new System.EventHandler(this.button_gradient_Click);
            // 
            // button_clean
            // 
            this.button_clean.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F);
            this.button_clean.Location = new System.Drawing.Point(207, 30);
            this.button_clean.Margin = new System.Windows.Forms.Padding(2);
            this.button_clean.Name = "button_clean";
            this.button_clean.Size = new System.Drawing.Size(90, 29);
            this.button_clean.TabIndex = 41;
            this.button_clean.Text = "Очистить все";
            this.button_clean.UseVisualStyleBackColor = true;
            this.button_clean.Click += new System.EventHandler(this.button_clean_Click);
            // 
            // Task3
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.ClientSize = new System.Drawing.Size(885, 614);
            this.Controls.Add(this.button_clean);
            this.Controls.Add(this.button_gradient);
            this.Controls.Add(this.button_point3);
            this.Controls.Add(this.button_point2);
            this.Controls.Add(this.button_point1);
            this.Controls.Add(this.label15);
            this.Controls.Add(this.label14);
            this.Controls.Add(this.label13);
            this.Controls.Add(this.pictureBox1);
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "Task3";
            this.Text = "Задание №2";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Task2_FormClosing);
            this.Load += new System.EventHandler(this.Task2_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.Button button_point1;
        private System.Windows.Forms.Button button_point2;
        private System.Windows.Forms.Button button_point3;
        private System.Windows.Forms.ColorDialog colorDialog1;
        private System.Windows.Forms.Button button_gradient;
        private System.Windows.Forms.Button button_clean;
    }
}

