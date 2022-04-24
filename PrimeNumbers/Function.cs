﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics;
using System.Text.RegularExpressions;
using Microsoft.Data.Sqlite;
using System.Runtime.CompilerServices;
using PrimeNumbers.Properties;

[assembly: InternalsVisibleTo("PrimeNumbers.Tests")]


namespace PrimeNumbers
{
    internal class Function
    {
        // global vars and objs
        private List<int> verificationPrimes = new();
        private readonly List<int> primes = new();

        Stopwatch timer = new();

        private bool isOutput;
        private bool isDatabase;
        private bool isCorrect;
        private bool isToBeCleared;

        private string input;
        private int a, b;


        public void Start()
        {
            Input();

            if (isDatabase)
                CreateDatabase();

            Calculate();

            if (isOutput)
                Output();

            Verify();

            if (isCorrect && isDatabase && primes.Any())
                FillDatabase();
        }

        public void Input()
        {
            Console.WriteLine("Both of your range ends have to be >= 2.\n");

            // getting range ends
            SetByInput(ref a, "Input first value: ");
            SetByInput(ref b, "Input second value: ");

            // swap their if it's incorrect (using tuples)
            if (a > b)
                (a, b) = (b, a);
            
            // getting conditions
            SetByInput(ref isOutput, "there be an output of calculated primes");
            SetByInput(ref isDatabase, "the program use DB");
        }

        public void Calculate()
        {
            timer = Stopwatch.StartNew();


            // trying to get any primes from our DB
            if (isDatabase)
                GetFromDatabase();


            // calculations w/ database
            if (primes.Any())
            {
                int temp;

                // choosing between mono- and multi- threading depends on the complexity of calculations
                if ((b - a) < 150000)
                {
                    temp = primes[0];

                    if (a != temp)
                    {
                        for (int i = a, j = 0; i < temp; i++)
                            if (IsPrime(i))
                            {
                                primes.Insert(j, i);
                                j++;
                            }
                    }


                    temp = primes.Last();

                    if (b != temp)
                    {
                        temp += 1;

                        for (int i = temp; i <= b; i++)
                            if (IsPrime(i))
                                primes.Add(i);
                    }
                }

                // multi-threading calculations
                else
                {
                    temp = primes[0];

                    if (a != temp)
                    {
                        var thing = from n in (Enumerable.Range(a, temp - a)).AsParallel().AsOrdered()
                                    where IsPrime(n)
                                    select n;

                        int j = 0;
                        foreach (var i in thing)
                        {
                            primes.Insert(j, i);
                            j++;
                        }
                    }


                    temp = primes.Last();

                    if (b != temp)
                    {
                        temp += 1;

                        var thing = from n in (Enumerable.Range(temp, b - temp - 1)).AsParallel().AsOrdered()
                                    where IsPrime(n)
                                    select n;

                        foreach (var i in thing)
                            primes.Add(i);
                    }
                }
            }


            // calculations w/o database
            else
            {
                // choosing between mono- and multi- threading depends on the complexity of calculations
                if ((b - a) < 150000)
                {
                    for (int i = a; i <= b; i++)
                        if (IsPrime(i))
                            primes.Add(i);
                }

                // multi-threading calculations
                else
                {
                    var thing = from n in (Enumerable.Range(a, b - a)).AsParallel().AsOrdered()
                                where IsPrime(n)
                                select n;

                    foreach (var i in thing)
                        primes.Add(i);
                }
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


            verificationPrimes = Resources.verification_primes.Split('\t').Select(n => Convert.ToInt32(n)).ToList();


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


                isCorrect = true;

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

        public static void CreateDatabase()
        {
            using SqliteConnection connection = new("Data Source=calculated_primes.db");
            connection.Open();

            var SQL_command = connection.CreateCommand();
            SQL_command.CommandText = "CREATE TABLE IF NOT EXISTS Primes (prime INT, UNIQUE(prime));";
            SQL_command.ExecuteNonQuery();

            connection.Close();
        }

        public void FillDatabase()
        {
            timer = Stopwatch.StartNew();


            // assembling an SQL command that will input all the new calculated primes into DB
            string txt_query = "INSERT OR IGNORE INTO Primes (prime) VALUES ";
            foreach (int i in primes)
                txt_query += $"({i}),";

            txt_query = Regex.Replace(txt_query, ",$", ";");


            using (SqliteConnection connection = new("Data Source=calculated_primes.db"))
            {
                connection.Open();

                var SQL_command = connection.CreateCommand();
                SQL_command.CommandText = txt_query;
                SQL_command.ExecuteNonQuery();

                timer.Stop();
                Console.WriteLine($"\n\nDatabase operations took {timer.ElapsedMilliseconds}ms\n");


                SetByInput(ref isToBeCleared, "we fully clear DB");

                if (isToBeCleared)
                {
                    SQL_command.CommandText = "DELETE FROM Primes;";
                    SQL_command.ExecuteNonQuery();
                }

                connection.Close();
            }
        }

        public void GetFromDatabase()
        {
            // getting a bunch of primes within a range of 'a' and 'b'
            using SqliteConnection connection = new("Data Source=calculated_primes.db");
            connection.Open();

            var SQL_command = connection.CreateCommand();
            SQL_command.CommandText = $"SELECT * FROM Primes WHERE prime >= {a} AND prime <= {b}";
            SQL_command.ExecuteNonQuery();

            SqliteDataReader SQL_reader = SQL_command.ExecuteReader();

            while (SQL_reader.Read())
                primes.Add(Convert.ToInt32(SQL_reader["prime"]));


            connection.Close();
        }

        public void SetByInput(ref int num, string text)
        {
            while (true)
            {
                Console.Write(text);
                input = Console.ReadLine();

                if (int.TryParse(input, out int x) && (x > 1))
                {
                    num = x;
                    break;
                }

                else
                    Console.WriteLine("\nIncorrect input! Try again.");
            }

            Console.WriteLine();
        }

        public void SetByInput(ref bool flag, string text)
        {
            while (true)
            {
                Console.Write("Should " + text + "? (y/n): ");
                input = Console.ReadLine();

                if (Regex.IsMatch(input, "^y$"))
                {
                    flag = true;
                    break;
                }

                else if (Regex.IsMatch(input, "^n$"))
                {
                    flag = false;
                    break;
                }

                else
                    Console.WriteLine("\nIncorrect answer! Try again.");
            }

            Console.WriteLine();
        }

        public static bool IsPrime(int num)
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
