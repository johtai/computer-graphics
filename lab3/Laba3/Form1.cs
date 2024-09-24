﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Laba3
{
    public partial class Form1 : Form
    {
        private Task1 task1 = new Task1();
        private Task2 task2 = new Task2();
        private Task3 task3 = new Task3();

        public Form1()
        {
            InitializeComponent();
            AddOwnedForm(task1);
            AddOwnedForm(task2);
            AddOwnedForm(task3);
        }

        private void button_Task1_Click(object sender, EventArgs e)
        {
            task1.Show();
        }

        private void button_Task2_Click(object sender, EventArgs e)
        {
            task2.Show();
        }

        private void button_Task3_Click(object sender, EventArgs e)
        {
            task3.Show();
        }

    }
}
