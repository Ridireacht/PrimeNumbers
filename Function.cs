using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrimeNumbers
{
    class Function
    {
        private List<int> verificationPrimes = new List<int>();
        private List<int> primes = new List<int>();
        private bool isPrime;
        private string input;
        private int a, b;

        public Function()
        {
            InputRange();
            Calculate();
            Output();
            Verify();
        }

        public void InputRange()
        {
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
        }

        public void Calculate()
        {
            for (int i = a; i <= b; i++)
            {
                isPrime = true;

                for (int j = 2; j < i; j++)
                {
                    if (i % j == 0)
                        isPrime = false;
                }

                if (isPrime)
                    primes.Add(i);
            }
        }

        public void Output()
        {
            Console.WriteLine("\n\nPrime numbers:\n");
            foreach (int i in primes)
                Console.Write($"{i}  ");
            Console.WriteLine();
        }

        public void Verify()
        {
            bool isCorrect = true;

            // getting file numbers and parse them into a new list
            input = System.IO.File.ReadAllText("../../../verification_primes.txt");
            input = input.Replace('\n', '\t');
            verificationPrimes = input.Split('\t').Select(n => Convert.ToInt32(n)).ToList();


            // if no primes calculated (their list is empty)
            if (primes.Equals(null))
                Console.WriteLine("There is nothing to check as no primes were calculated.");

            else
            {
                if (b > verificationPrimes.Last())
                    Console.WriteLine($"\nUnable to check all the numbers calculated as the biggest prime in verification list is {verificationPrimes.Last()}.\n");

                // remove unnecessary elements to match this list with the actual 'calculated primes' list by starting position
                verificationPrimes.RemoveRange(0, verificationPrimes.IndexOf(primes[0]));

                // compare the nums between two lists, one to one
                for (int i = 0; i < primes.Count; i++)
                    if (verificationPrimes[i] != primes[i])
                    {
                        isCorrect = false;
                        break;
                    }

                // outcome of checks
                if (isCorrect)
                    Console.WriteLine("\nAll calculations were done right.");
                else
                    Console.WriteLine("\nCalculations done wrong.");

            }
            
        }

    }
}
