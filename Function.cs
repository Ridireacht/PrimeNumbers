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

                if (int.TryParse(input, out int x) && (x >= 1))
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

                if (int.TryParse(input, out int x) && (x >= 1))
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

            input = System.IO.File.ReadAllText(@"../../../verification_primes.txt");
            verificationPrimes = input.Split('\t').Select(n => Convert.ToInt32(n)).ToList();

            if (b > 104729)
                Console.WriteLine("\nUnable to check all the numbers calculated as verification list only contains primes not exceeding 104729.");
            else
            {
                if (primes.Equals(null))
                    Console.WriteLine("There is nothing to check as no primes were calculated.");

                else
                {
                    for (int i = verificationPrimes.IndexOf(primes[0]), j = 0; j < primes.Count-1; i++, j++)
                        if (verificationPrimes[i] != primes[j])
                        {
                            isCorrect = false;
                            break;
                        }

                    if (isCorrect)
                        Console.WriteLine("All calculations were done right.");
                    else
                        Console.WriteLine("Calculations done wrong.");
                }
            }
        }

    }
}
