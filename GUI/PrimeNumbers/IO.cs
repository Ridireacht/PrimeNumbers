﻿using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;

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

        public static bool isCorrectRange(string input)
        {
            if (int.TryParse(input, out int x) && (x > 1))
                return true;

            else
                return false;
        }

    }
}
