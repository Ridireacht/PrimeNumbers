using System.Runtime.CompilerServices;
using System.Collections.Generic;
using PrimeNumbers.Properties;
using System.Linq;
using System;

[assembly: InternalsVisibleTo("PrimeNumbers.Tests")]


namespace PrimeNumbers
{
    internal class Calculator
    {
        private readonly List<int> verificationPrimes = Resources.verification_primes.Split('\t').Select(n => Convert.ToInt32(n)).ToList();
        private String calculation_mode = "auto";
        private int range_start, range_end;


        public void SetEnds(int a, int b)
        {
            this.range_start = a;
            this.range_end = b;
        }


        // main method of the class - other methods invoked from here.
        // it also configures the mode of further calculations.
        public void GetPrimes(ref List<int> numList, int range_start, int range_end, string mode = "auto")
        {

            this.range_start = range_start;
            this.range_end = range_start;

            // from my observations, mono-threading mode goes faster than multi-threading one
            // as long as range of numbers to be checked is smaller than 150000 units
            if (mode == "auto")
                if ((range_end - range_start) < 150000)
                    calculation_mode = "mono";
                else
                    calculation_mode = "multi";

            else
                calculation_mode = mode;


            // number list comes empty by default. if not, then it was succesfully supplemented
            // with data from DB - we need to handle this situation separately
            if (numList.Any())
                CalculateDB(ref numList, range_start, range_end, mode);
            else
                CalculateNoDB(ref numList, range_start, range_end, mode);
        }


        // calculate primes with DB enabled
        public static void CalculateDB(ref List<int> numList, int range_start, int range_end, string mode)
        {
            int temp;

            // choosing between mono- and multi- threading depends on the complexity of calculations.
            // below is the mono-threading variant.
            if ( ((range_end - range_start) < 150000 || mode == "mono") && mode != "multi")
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

                    var thing = from n in (Enumerable.Range(temp, range_end - temp)).AsParallel().AsOrdered()
                                where IsPrime(n)
                                select n;

                    foreach (var i in thing)
                        numList.Add(i);
                }
            }
        }


        // calculate primes without DB
        public static void CalculateNoDB(ref List<int> numList, int range_start, int range_end, string mode)
        {
            // choosing between mono- and multi- threading depends on the complexity of calculations
            // below is the mono-threading variant.
            if ( ((range_end - range_start) < 150000 || mode == "mono") && mode != "multi")
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


        // compares calculated and verified primes
        public string VerifyCalculations(List<int> numList)
        {
            string output = "";
            int range_end = numList.Count;
            int lastCorrect = 0;
            int numsChecked = 0;


            // if no primes calculated (their list is empty)
            if (!numList.Any())
                return "\r\nThere is nothing to check as no primes were calculated.";


            else
            {
                // remove unnecessary elements to match this list with the actual 'calculated primes' list by starting position
                verificationPrimes.RemoveRange(0, verificationPrimes.IndexOf(numList[0]));

                // if there are more calculated primes other than verification ones
                if (numList.Last() > verificationPrimes.Last())
                {
                    output += $"\r\nUnable to check all the numbers calculated as the last prime in verification list is {verificationPrimes.Last()}.\r\n";
                    range_end = verificationPrimes.Count;
                }


                bool isCorrect = true;

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


                output += "\r\n";

                // outcome of all checks
                if ((numsChecked == 1) && (!isCorrect))
                    output += "\r\nThe very first calculated prime num is already wrong, so all the sequence.";
                else
                    output += $"\r\nChecked nums: {numsChecked}\r\nLast correct one: {lastCorrect}";

                if (isCorrect)
                    output += "\r\nAll calculations were done right.";
                else
                    output += "\nCalculations done wrong.";


                return output;
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
