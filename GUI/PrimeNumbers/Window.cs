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
            if (checkBox1.Checked)
                isOutput = true;
            else
                isOutput = false;

            if (checkBox2.Checked)
                isDatabase = true;
            else
                isDatabase = false;
        }

    }
}
