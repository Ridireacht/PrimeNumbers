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
