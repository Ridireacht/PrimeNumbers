﻿using System.Diagnostics;
using System.Runtime.CompilerServices;
using PrimeNumbers.Properties;

[assembly: InternalsVisibleTo("PrimeNumbers.Tests")]


namespace PrimeNumbers
{
    internal class Calculation
    {
        // global vars and objs
        private readonly List<int> verificationPrimes = Resources.verification_primes.Split('\t').Select(n => Convert.ToInt32(n)).ToList();
        public List<int> primes = new();

        Stopwatch timer = new();

        private bool isCorrect;

        private int a, b;


        public void Set(int a, int b)
        {
            this.a = a;
            this.b = b;
        }

        public void GetPrimes()
        {
            timer = Stopwatch.StartNew();


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
