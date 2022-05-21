using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;


namespace PrimeNumbers
{
    public partial class Window : Form
    {

        // global vars
        int a = 0,
            b = 0;

        bool isOutput,
             isDatabase,
             isCorrect,
             isToBeCleared;

        // global objs
        DB db = new();
        Calculator c = new();

        Stopwatch timer = new();
        List<int> primes = new();



        public Window()
        {
            InitializeComponent();
        }


        private void button1_Click(object sender, EventArgs e)
        {
            if (IO.TryRangeEnd(textBox2.Text) != null)
            {
                if (IO.TryRangeEnd(textBox3.Text) != null)
                {
                    a = Int32.Parse(textBox2.Text);
                    b = Int32.Parse(textBox3.Text);

                    if (a > b)
                        (a, b) = (b, a);

                    if (checkBox1.Checked)
                        isOutput = true;
                    else
                        isOutput = false;

                    if (checkBox2.Checked)
                        isDatabase = true;
                    else
                        isDatabase = false;
                }

                else
                    textBox1.Text = "First range end is incorrect! Make use the format is right and range itself is >= 2.";
            }

            else
                textBox1.Text = "Second range end is incorrect! Make use the format is right and range itself is >= 2.";
        }

    }
}
