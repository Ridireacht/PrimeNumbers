using System;

namespace PrimeNumbers
{
    class Program
    {
        static void Main(string[] args)
        {

            string input;
            int a, b;

            // inputting and checking first var
            while (true)
            {
                Console.Write("Input first value: ");
                input = Console.ReadLine();

                if (int.TryParse(input, out int x))
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

                if (int.TryParse(input, out int x))
                {
                    b = x;
                    break;
                }

                else
                    Console.WriteLine("\nIncorrect input! Try again.");
            }


        }
    }
}
