using System;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics;
using System.Text.RegularExpressions;
using System.Runtime.CompilerServices;
using Microsoft.Data.Sqlite;
using PrimeNumbers.Properties;

[assembly: InternalsVisibleTo("PrimeNumbers.Tests")]


namespace PrimeNumbers
{
    internal class Function
    {
        // global vars and objs
        private readonly List<int> verificationPrimes = Resources.verification_primes.Split('\t').Select(n => Convert.ToInt32(n)).ToList();
        private List<int> primes = new();

        Stopwatch timer = new();

        private bool isOutput;
        private bool isDatabase;
        private bool isCorrect;

        private readonly string pathDB = "calculated_primes.db";
        private int a, b;


        public void Start()
        {
            Input();

            if (isDatabase)
                CreateDatabase(pathDB);

            GetPrimes();

            if (isOutput)
                Output(primes);



            Verify(primes);


            if (isCorrect && isDatabase && primes.Any())
            {
                timer = Stopwatch.StartNew();

                FillDatabase(pathDB, primes);

                timer.Stop();
                Console.WriteLine($"\n\nDatabase operations took {timer.ElapsedMilliseconds}ms\n");

                ClearDatabase(pathDB);
            }


            #if !DEBUG
                Console.ReadKey();
            #endif
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

        public void GetPrimes()
        {
            timer = Stopwatch.StartNew();


            if (isDatabase)
                GetFromDatabase(pathDB, ref primes, a, b);

            if (primes.Any())
                CalculateDB(a, b, ref primes);
            else
                CalculateNoDB(a, b, ref primes);


            timer.Stop();
        }

        public static void CalculateDB(int range_start, int range_end, ref List<int> numList)
        {
            int temp;

            // choosing between mono- and multi- threading depends on the complexity of calculations
            if ((range_end - range_start) < 150000)
            {
                temp = numList[0];

                if (range_start != temp)
                {
                    for (int i = range_start, j = 0; i < temp; i++)
                        if (IsPrime(i))
                        {
                            numList.Insert(j, i);
                            j++;
                        }
                }


                temp = numList.Last();

                if (range_end != temp)
                {
                    temp += 1;

                    for (int i = temp; i <= range_end; i++)
                        if (IsPrime(i))
                            numList.Add(i);
                }
            }

            // multi-threading calculations
            else
            {
                temp = numList[0];

                if (range_start != temp)
                {
                    var thing = from n in (Enumerable.Range(range_start, temp - range_start)).AsParallel().AsOrdered()
                                where IsPrime(n)
                                select n;

                    int j = 0;
                    foreach (var i in thing)
                    {
                        numList.Insert(j, i);
                        j++;
                    }
                }


                temp = numList.Last();

                if (range_end != temp)
                {
                    temp += 1;

                    var thing = from n in (Enumerable.Range(temp, range_end - temp - 1)).AsParallel().AsOrdered()
                                where IsPrime(n)
                                select n;

                    foreach (var i in thing)
                        numList.Add(i);
                }
            }
        }

        public static void CalculateNoDB(int range_start, int range_end, ref List<int> numList)
        {
            // choosing between mono- and multi- threading depends on the complexity of calculations
            if ((range_end - range_start) < 150000)
            {
                for (int i = range_start; i <= range_end; i++)
                    if (IsPrime(i))
                        numList.Add(i);
            }

            // multi-threading calculations
            else
            {
                var thing = from n in (Enumerable.Range(range_start, range_end - range_start)).AsParallel().AsOrdered()
                            where IsPrime(n)
                            select n;

                foreach (var i in thing)
                    numList.Add(i);
            }
        }

        public static void Output(List<int> numList)
        {
            Console.WriteLine("\n\nPrime numbers:\n");
            foreach (int i in numList)
                Console.Write($"{i}  ");
        }

        public void Verify(List<int> numList)
        {
            int range_end = numList.Count;
            int lastCorrect = 0;
            int numsChecked = 0;


            Console.WriteLine($"\n\nCalculations took {timer.ElapsedMilliseconds}ms");
            timer = Stopwatch.StartNew();


            // if no primes calculated (their list is empty)
            if (!numList.Any())
                Console.WriteLine("\nThere is nothing to check as no primes were calculated.");

            else
            {
                // remove unnecessary elements to match this list with the actual 'calculated primes' list by starting position
                verificationPrimes.RemoveRange(0, verificationPrimes.IndexOf(numList[0]));

                // if there are more calculated primes other than verification ones
                if (numList.Last() > verificationPrimes.Last())
                {
                    Console.WriteLine($"\nUnable to check all the numbers calculated as the last prime in verification list is {verificationPrimes.Last()}.\n");
                    range_end = verificationPrimes.Count;
                }


                isCorrect = true;

                // compare the nums between two lists, one to one
                for (int i = 0; i < range_end; i++)
                {
                    numsChecked += 1;

                    if (verificationPrimes[i] != numList[i])
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

        public static void CreateDatabase(string path)
        {
            using SqliteConnection connection = new("Data Source=" + path);
            connection.Open();

            var SQL_command = connection.CreateCommand();
            SQL_command.CommandText = "CREATE TABLE IF NOT EXISTS Primes (prime INT, UNIQUE(prime));";
            SQL_command.ExecuteNonQuery();

            connection.Close();
        }

        public static void FillDatabase(string path, List<int> numList)
        {
            // assembling an SQL command that will input all the new calculated primes into DB
            string txt_query = "INSERT OR IGNORE INTO Primes (prime) VALUES ";
            foreach (int i in numList)
                txt_query += $"({i}),";

            txt_query = Regex.Replace(txt_query, ",$", ";");


            using (SqliteConnection connection = new("Data Source=" + path))
            {
                connection.Open();

                var SQL_command = connection.CreateCommand();
                SQL_command.CommandText = txt_query;
                SQL_command.ExecuteNonQuery();

                connection.Close();
            }
        }

        public static void ClearDatabase(string path)
        {
            bool isToBeCleared = false;
            SetByInput(ref isToBeCleared, "we fully clear DB");

            if (isToBeCleared)
            {
                using SqliteConnection connection = new("Data Source=" + path);
                connection.Open();

                var SQL_command = connection.CreateCommand();
                SQL_command.CommandText = "DELETE FROM Primes;";
                SQL_command.ExecuteNonQuery();

                connection.Close();
            }
        }

        public static void GetFromDatabase(string path, ref List<int> numList, int range_start, int range_end)
        {
            // getting a bunch of primes within a range of 'a' and 'b'
            using SqliteConnection connection = new("Data Source=" + path);
            connection.Open();

            var SQL_command = connection.CreateCommand();
            SQL_command.CommandText = $"SELECT * FROM Primes WHERE prime >= {range_start} AND prime <= {range_end}";
            SQL_command.ExecuteNonQuery();

            SqliteDataReader SQL_reader = SQL_command.ExecuteReader();

            while (SQL_reader.Read())
                numList.Add(Convert.ToInt32(SQL_reader["prime"]));


            connection.Close();
        }

        public static void SetByInput(ref int num, string text)
        {
            string input;

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

        public static void SetByInput(ref bool flag, string text)
        {
            string input;

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
