using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ShadesOfGray
{
    public partial class Form0 : Form
    {
        public Form0()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var myForm = new Form1();
            myForm.Show();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Ещё не реализованно!");
        }

        private void button3_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Ещё не реализованно!");
        }
    }
}
