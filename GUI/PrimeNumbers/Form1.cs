﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PrimeNumbers
{
    public partial class Form1 : Form
    {
        Function f = new Function();

        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            while (true)
            {
                if (int.TryParse(textBox2.Text, out int x) && (x > 1))
                {
                    f.a = x;
                    break;
                }

                else
                    textBox1.Text += "Incorrect input! Try again.\r\n\n";
            }

            while (true)
            {
                if (int.TryParse(textBox3.Text, out int x) && (x > 1))
                {
                    f.b = x;
                    break;
                }

                else
                    textBox1.Text += "Incorrect input! Try again.\r\n\n";
            }

            if (f.a > f.b)
                (f.a, f.b) = (f.b, f.a);

            if (checkBox1.Checked)
                f.isOutput = true;
            else
                f.isOutput = false;

            if (checkBox2.Checked)
                f.isDatabase = true;
            else
                f.isDatabase = false;

            if (checkBox3.Checked)
                f.isToClear = true;
            else
                f.isToClear = false;

            f.Start();
        }
    }
}
