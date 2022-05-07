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

        private readonly string pathDB = "calculated_primes.db";
        private int a, b;


        public void Start()
        {
            if (isDatabase)
                CreateDatabase(pathDB);

            GetPrimes();

            if (isOutput)
                Output();

            Verify(primes);

            if (isCorrect && isDatabase && primes.Any())
            {
                timer = Stopwatch.StartNew();

                FillDatabase(pathDB, primes);

                timer.Stop();
                Console.WriteLine($"\n\nDatabase operations took {timer.ElapsedMilliseconds}ms\n");

                if (isToClear)
                    ClearDatabase(pathDB);
            }
        }

        public void Output()
        {

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
    }
}
