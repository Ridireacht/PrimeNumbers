using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;

[assembly: InternalsVisibleTo("PrimeNumbers.Tests")]



namespace PrimeNumbers
{
    internal class IO
    {
        int a, b;
        bool isOutput, isDatabase;


        public void Input()
        {
            Console.WriteLine("Both of your range ends have to be >= 2.\n");

            // getting range ends
            SetByInput(ref a, "Input first value: ");
            SetByInput(ref b, "Input second value: ");

            // swap their if it's incorrect (using tuples)
            if (a > b)
                (a, b) = (b, a);

            // getting conditions
            SetByInput(ref isOutput, "there be an output of calculated primes");
            SetByInput(ref isDatabase, "the program use DB");
        }

        public static void Output(List<int> numList)
        {
            Console.WriteLine("\n\nPrime numbers:\n");
            foreach (int i in numList)
                Console.Write($"{i}  ");
        }

        public static void SetByInput(ref int num, string text)
        {
            string? input;

            while (true)
            {
                Console.Write(text);
                input = Console.ReadLine();

                if (int.TryParse(input, out int x) && (x > 1))
                {
                    num = x;
                    break;
                }

                else
                    Console.WriteLine("\nIncorrect input! Try again.");
            }

            Console.WriteLine();
        }

        public static void SetByInput(ref bool flag, string text)
        {
            string? input;

            while (true)
            {
                Console.Write("Should " + text + "? (y/n): ");
                input = Console.ReadLine();

                if (Regex.IsMatch(input, "^y$"))
                {
                    flag = true;
                    break;
                }

                else if (Regex.IsMatch(input, "^n$"))
                {
                    flag = false;
                    break;
                }

                else
                    Console.WriteLine("\nIncorrect answer! Try again.");
            }

            Console.WriteLine();
        }
    }
}
