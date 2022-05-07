using System;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics;
using System.Text.RegularExpressions;
using System.Runtime.CompilerServices;
using PrimeNumbers.Properties;

namespace PrimeNumbers
{
    class Function
    {
        // global vars and objs
        private List<int> primes = new List<int>();

        Stopwatch timer = new Stopwatch();

        public bool isOutput;
        public bool isDatabase;
        public bool isToClear;
        private bool isCorrect;

        public readonly string pathDB = "calculated_primes.db";
        public string output = "";
        public int a, b;


        public void Start()
        {
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
                output += $"\r\n\nDatabase operations took {timer.ElapsedMilliseconds}ms\n";

                if (isToClear)
                    ClearDatabase(pathDB);
            }
        }


        public void Output(List<int> numList)
        {
            output += "\r\n\nPrime numbers:\n";
            foreach (int i in numList)
                output += $"{i}  ";
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


        public void Verify(List<int> numList)
        {

        }


        public void CreateDatabase(string path)
        {
        
        }


        public void FillDatabase(string path, List<int> numList)
        {

        }


        public void ClearDatabase(string path)
        {

        }

        
        public void GetFromDatabase(string path, ref List<int> numList, int range_start, int range_end)
        {

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
