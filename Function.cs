using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrimeNumbers
{
    class Function
    {
        private int[] primes;
        private string input;
        private int a, b;

        public Function()
        {
            InputRange();
        }

        public void InputRange()
        {
            // inputting and checking first var
            while (true)
            {
                Console.Write("Input first value: ");
                input = Console.ReadLine();

                if (int.TryParse(input, out int x) && (x >= 0))
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

                if (int.TryParse(input, out int x) && (x >= 0))
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

    }
}
