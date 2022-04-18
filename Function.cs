using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Linq;
using System.Diagnostics;
using System.Text.RegularExpressions;
using Microsoft.Data.Sqlite;


namespace PrimeNumbers
{
    class Function
    {
        private List<int> verificationPrimes = new();
        private readonly List<int> primes = new();
        Stopwatch timer = new();
        private bool isOutput = false, isCorrect = true;
        private string input;
        private int a, b;


        public Function()
        {
            Input();
            Calculate();

            if (isOutput)
                Output();

            Verify();

            if (isCorrect)
                Database();
        }

        public void Input()
        {
            Console.WriteLine("Your input has to be >= 2.\n");

            // inputting and checking first var
            while (true)
            {
                Console.Write("Input first value: ");
                input = Console.ReadLine();

                if (int.TryParse(input, out int x) && (x > 1))
                {
                    a = x;
                    break;
                }

                else
                    Console.WriteLine("\nIncorrect input! Try again.");
            }


            Console.WriteLine();


            // inputting and checking second var
            while (true)
            {
                Console.Write("Input second value: ");
                input = Console.ReadLine();

                if (int.TryParse(input, out int x) && (x > 1))
                {
                    b = x;
                    break;
                }

                else
                    Console.WriteLine("\nIncorrect input! Try again.");
            }


            // swap order of vars if it's incorrect (using tuples)
            if (a > b)
                (a, b) = (b, a);


            Console.WriteLine();


            // inputting if there will be output
            while (true)
            {
                Console.Write("Should there be an output of calculated primes? (y/n): ");
                input = Console.ReadLine();

                if (Regex.IsMatch(input, "^y$"))
                {
                    isOutput = true;
                    break;
                }

                else if (Regex.IsMatch(input, "^n$"))
                    break;

                else
                    Console.WriteLine("\nIncorrect answer! Try again.");
            }
        }

        public void Calculate()
        {
            timer = Stopwatch.StartNew();


            // that's where we choose to use mono-threading or multi-threading
            // algorithm - it depends on the complexity of our calculations
            if ((b - a) < 150000)
            {
                for (int i = a; i <= b; i++)
                    if (isPrime(i))
                        primes.Add(i);
            }

            else
            {
                var thing =  from n in (Enumerable.Range(a, b)).AsParallel().AsOrdered()
                             where isPrime(n)
                             select n;

                foreach (var i in thing)
                    primes.Add(i);
            }


            timer.Stop();
        }

        public void Output()
        {
            Console.WriteLine("\n\nPrime numbers:\n");
            foreach (int i in primes)
                Console.Write($"{i}  ");
        }

        public void Verify()
        {
            int rangeEnd = primes.Count;
            int lastCorrect = 0;
            int numsChecked = 0;


            Console.WriteLine($"\n\nCalculations took {timer.ElapsedMilliseconds}ms");
            timer = Stopwatch.StartNew();


            // getting file numbers
            try
            {
                input = System.IO.File.ReadAllText("../../../verification_primes.txt");
            }
            catch (System.IO.FileNotFoundException)
            {
                Console.WriteLine("File with verification primes doesn't exist in current folder.");
                return;
            }

            if (input == "")
            {
                Console.WriteLine("File with verification primes is empty.");
                return;
            }


            // parse primes into a new list
            input = input.Replace('\n', '\t');

            if (!Regex.IsMatch(input, "^[0-9]+(\\s+[0-9]+)+$"))
            {
                Console.WriteLine("Formatting of verifications primes' file doesn't match with the specified template, so it wasn't used.");
                return;
            }
            verificationPrimes = input.Split('\t').Select(n => Convert.ToInt32(n)).ToList();


            // if no primes calculated (their list is empty)
            if (!primes.Any())
                Console.WriteLine("\nThere is nothing to check as no primes were calculated.");

            else
            {
                // remove unnecessary elements to match this list with the actual 'calculated primes' list by starting position
                verificationPrimes.RemoveRange(0, verificationPrimes.IndexOf(primes[0]));

                // if there are more calculated primes other than verification ones
                if (b > verificationPrimes.Last())
                {
                    Console.WriteLine($"\nUnable to check all the numbers calculated as the last prime in verification list is {verificationPrimes.Last()}.\n");
                    rangeEnd = verificationPrimes.Count;
                }


                // compare the nums between two lists, one to one
                for (int i = 0; i < rangeEnd; i++)
                {
                    numsChecked += 1;

                    if (verificationPrimes[i] != primes[i])
                    {
                        isCorrect = false;
                        break;
                    }

                    lastCorrect = verificationPrimes[i];
                }


                timer.Stop();


                Console.WriteLine();
                // outcome of all checks
                if ((numsChecked == 1) && (!isCorrect))
                    Console.WriteLine("\nThe very first calculated prime num is already wrong, so all the sequence.");
                else
                    Console.WriteLine($"\nChecked nums: {numsChecked}\nLast correct one: {lastCorrect}\nVerification took: {timer.ElapsedMilliseconds}ms");

                if (isCorrect)
                    Console.WriteLine("\nAll calculations were done right.");
                else
                    Console.WriteLine("\nCalculations done wrong.");
            }
        }

        public void Database()
        {
            string txt_query = "CREATE TABLE IF NOT EXISTS Primes (prime INT); " +
                               "INSERT INTO Primes (prime) VALUES ";

            foreach (int i in primes)
                txt_query += $"({i}),";

            txt_query = Regex.Replace(txt_query, ",$", ";");


            using (var connection = new SqliteConnection("Data Source=../../../calculated_primes.db"))
            {
                connection.Open();

                var SQL_command = connection.CreateCommand();
                SQL_command.CommandText = txt_query;
                SQL_command.ExecuteNonQuery();

                connection.Close();
            }
        }

        public static bool isPrime(int num)
        {
            // all the numbers matching the statements below aren't prime,
            // so we skip them immediately
            if (((num % 2 == 0) && (num != 2)) || ((num % 3 == 0) && (num != 3)))
                return false;

            for (int i = 2; i <= Math.Sqrt(num); i++)
                if (num % i == 0)
                    return false;

            return true;
        }

    }
}
