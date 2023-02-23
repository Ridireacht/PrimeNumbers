using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.Diagnostics;
using System.IO;

namespace PrimeNumbers
{
    public partial class Window : Form
    {

        // global vars
        int a = 0,
            b = 0;

        bool isOutput,
             isDatabase,
             isCorrect;

        bool isFirstPlaceholderOn = true,
             isSecondPlaceholderOn = true;


        // global objs
        readonly Calculator c = new();

        Stopwatch timer = new();
        List<int> primes = new();



        public Window()
        {
            InitializeComponent();
        }



        // 'Calculate!' button
        private void button1_Click(object sender, EventArgs e)
        {
            // nullify status of multiple-times-used vars
            textBox1.Text = "";
            primes.RemoveRange(0, primes.Count);


            // if both range textboxes are correctfully filled, proceed;
            // otherwise return error text
            if (IO.TryRangeEnd(textBox2.Text) != null)
            {
                if (IO.TryRangeEnd(textBox3.Text) != null)
                {
                    // in case everything is alright
                    a = Int32.Parse(textBox2.Text);
                    b = Int32.Parse(textBox3.Text);


                    // swap order of ends if incorrect
                    if (a > b)
                        (a, b) = (b, a);


                    // set flags and options
                    if (checkBox1.Checked)
                        isOutput = true;
                    else
                        isOutput = false;

                    if (checkBox2.Checked)
                        isDatabase = true;
                    else
                        isDatabase = false;

                    if (isDatabase)
                    {
                        DB.CreateDatabase();
                        DB.GetFromDatabase(ref primes, a, b);
                    }


                    // calculation
                    timer = Stopwatch.StartNew();

                    if (radioButton1.Checked)
                        c.GetPrimes(ref primes, a, b, "mono");
                    else if (radioButton2.Checked)
                        c.GetPrimes(ref primes, a, b, "multi");
                    else
                        c.GetPrimes(ref primes, a, b);

                    timer.Stop();


                    // output (if needed)
                    if (isOutput)
                        textBox1.Text = IO.Output(primes) + "\r\n\r\n";
                    textBox1.Text += $"Calculations took {timer.ElapsedMilliseconds}ms";


                    // verification
                    timer = Stopwatch.StartNew();
                    textBox1.Text += c.VerifyCalculations(primes);

                        // if response ends with "... correcT."
                        if (textBox1.Text[^2] == 't')
                            isCorrect = true;
                        else
                            isCorrect = false;

                    timer.Stop();
                    Console.WriteLine($"\r\nVerification took {timer.ElapsedMilliseconds}ms");


                    // manage DB
                    if (isCorrect && isDatabase && primes.Any())
                    {
                        timer = Stopwatch.StartNew();

                        DB.FillDatabase(primes);

                        timer.Stop();
                        textBox1.Text += $"\r\n\r\nDatabase operations took {timer.ElapsedMilliseconds}ms\r\n";
                    }

                }

                else
                    textBox1.Text = "Second range end is incorrect! Make sure the format is right and range itself is >= 2.";
            }

            else
                textBox1.Text = "First range end is incorrect! Make sure the format is right and range itself is >= 2.";
        }


        // placeholder handling (first range end)
        private void textBox2_Enter(object sender, EventArgs e)
        {
            if (isFirstPlaceholderOn)
            {
                textBox2.Text = "";
                textBox2.ForeColor = System.Drawing.Color.Black;
                isFirstPlaceholderOn = false;
            }
        }

        private void textBox2_Leave(object sender, EventArgs e)
        {
            if (!isFirstPlaceholderOn && textBox2.Text == "")
            {
                textBox2.Text = "2";
                textBox2.ForeColor = System.Drawing.Color.LightGray;
                isFirstPlaceholderOn = true;
            }
        }


        // placeholder handling (second range end)
        private void textBox3_Enter(object sender, EventArgs e)
        {
            if (isSecondPlaceholderOn)
            {
                textBox3.Text = "";
                textBox3.ForeColor = System.Drawing.Color.Black;
                isSecondPlaceholderOn = false;
            }

        }

        private void textBox3_Leave(object sender, EventArgs e)
        {
            if (!isSecondPlaceholderOn && textBox3.Text == "")
            {
                textBox3.Text = "100000";
                textBox3.ForeColor = System.Drawing.Color.LightGray;
                isSecondPlaceholderOn = true;
            }
        }


        // 'Clear DB' button
        private void button2_Click(object sender, EventArgs e)
        {
            if (isDatabase == true)
                DB.ClearDatabase();
            else
                textBox1.Text = "There is no database to be cleared!";
        }


        // 'Save to file' button
        private void button3_Click(object sender, EventArgs e)
        {
            if (!primes.Any())
                textBox1.Text = "There is nothing to write in the file, as there are no calculated primes yet!";
            else
            {
                // default name
                saveFileDialog1.FileName = "primes";
                
                // file extensions
                saveFileDialog1.DefaultExt = "txt";
                saveFileDialog1.Filter = "Text files (*.txt)|*.txt|All files (*.*)|*.*";

                // initial dir = desktop
                saveFileDialog1.InitialDirectory =
                    Environment.GetFolderPath(Environment.SpecialFolder.Desktop);

                // run dialog
                saveFileDialog1.ShowDialog();
            }
        }


        // saving a file
        private void saveFileDialog1_FileOk(object sender, System.ComponentModel.CancelEventArgs e)
        {
            string name = saveFileDialog1.FileName;
            File.WriteAllText(name, String.Join("\t", primes));
        }

    }
}
