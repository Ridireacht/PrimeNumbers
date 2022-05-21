using System.Collections.Generic;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("PrimeNumbers.Tests")]



namespace PrimeNumbers
{
    internal class IO
    {
        public static string Output(List<int> numList)
        {
            string output = "";
            output += "\r\n\r\nPrime numbers:\r\n";

            foreach (int i in numList)
                output += $"{i}  ";

            return output;
        }

        public static int? TryRangeEnd(string input)
        {
            if (int.TryParse(input, out int x) && (x > 1))
                return x;
            else
                return null;
        }
    }
}
