using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrimeNumbers
{
    class Function
    {
        private List<int> primes = new List<int> ();
        private bool isPrime;
        private string input;
        private int a, b;

        public Function()
        {
            InputRange();
            Calculate();
            Output();
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

    }
}
