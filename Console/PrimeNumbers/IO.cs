using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;

[assembly: InternalsVisibleTo("PrimeNumbers.Tests")]



namespace PrimeNumbers
{
    internal class IO
    {
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
