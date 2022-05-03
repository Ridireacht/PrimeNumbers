using Microsoft.Data.Sqlite;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;


namespace PrimeNumbers.Tests
{
    [TestClass]
    public class PrimeNumbersTests
    {
        [TestMethod]
        public void IsPrime_randomPrimes_returnsTrue()
        {
            // arrange
            List<int> nums = new() { 32213, 85829, 91297, 50923, 92767, 70099, 51713, 84299, 74317, 52571 };
            bool flag = true;


            // act
            foreach (int i in nums)
                if (!Function.IsPrime(i))
                {
                    flag = false;
                    break;
                }


            // assert
            Assert.AreEqual(true, flag);
        }


        [TestMethod]
        public void IsPrime__randomNonPrimes_returnsFalse()
        {
            // arrange
            List<int> nums = new() { 14133, 51781, 12072, 77405, 41456, 11989, 74389, 21387, 67406, 92025 };
            bool flag = true;


            // act
            foreach (int i in nums)
                if (Function.IsPrime(i))
                {
                    flag = false;
                    break;
                }


            // assert
            Assert.AreEqual(true, flag);
        }


        [TestMethod]
        public void SetByInput_noExceptionIsThrown()
        {
            // arrange
            List<string> inputs = new() { "7", "12", "-5", "100000", "-121212", "sample_text", "8xybr8y3b1x8br8", "#!@#%*&$^)**!&?¹;%:+", "12 31 42", "yyy"};


            // act & assert
            try
            {
                foreach (string input in inputs)
                    int.TryParse(input, out int x);

                foreach (string input in inputs)
                    Regex.IsMatch(input, "^y$");
            }

            catch (Exception ex)
            {
                Assert.Fail($"Expected no exceptions, but got {ex.Message}");
            }
        }


        [TestMethod]
        public void SetByInput_randomInput_correctChecks()
        {
            // arrange
            List<string> inputs = new() { "7", "12", "y", "-5", "1.1", "4.6", "100000", "-121212", "y", "sample_text", "8xybr8y3b1x8br8", "nn", "#!@#%*&$^)**!&?¹;%:+", "n", "12 31 42", "yyy" };
            int count_ints = 0;
            int count_yes_not = 0;


            // act
            foreach (string input in inputs)
                if (int.TryParse(input, out int x) && (x > 1))
                    count_ints++;

            foreach (string input in inputs)
                if (Regex.IsMatch(input, "^y$") || Regex.IsMatch(input, "^n$"))
                    count_yes_not++;


            // assert
            Assert.AreEqual(true, (count_ints == 3 && count_yes_not == 3));
        }


        [TestMethod]
        public void CreateDatabase_createAndDelete_noExceptionIsThrown()
        {
            // arrange
            string path = "test.db";

            // act & assert
            try
            {
                Function.CreateDatabase(path);
                SqliteConnection.ClearAllPools();
                File.Delete(path);
            }

            catch (Exception ex)
            {
                Assert.Fail($"Expected no exceptions, but got {ex.Message}");
            }
        }


        [TestMethod]
        public void CalculateNoDB_monoThreading_correctCalculations()
        {
            // arrange
            List<int> primes = new();

            // act & assert
            try
            {
                Function f = new();
                f.CalculateNoDB(2, 100000, ref primes);

                foreach (int i in primes)
                    if (!Function.IsPrime(i))
                        Assert.Fail($"Wrong calculations - {i} is not a prime number!");
            }

            catch (Exception ex)
            {
                Assert.Fail($"Expected no exceptions, but got {ex.Message}");
            }
        }


        [TestMethod]
        public void CalculateNoDB_multiThreading_correctCalculations()
        {
            // arrange
            List<int> primes = new();

            // act & assert
            try
            {
                Function f = new();
                f.CalculateNoDB(2, 200000, ref primes);

                foreach (int i in primes)
                    if (!Function.IsPrime(i))
                        Assert.Fail($"Wrong calculations - {i} is not a prime number!");
            }

            catch (Exception ex)
            {
                Assert.Fail($"Expected no exceptions, but got {ex.Message}");
            }
        }


        [TestMethod]
        public void CalculateDB_monoThreading_correctCalculations()
        {
            // arrange
            List<int> primes = new();
            string path = "test.db";

            // act & assert
            try
            {
                Function.CreateDatabase(path);

                Function f = new();
                f.CalculateNoDB(2, 100000, ref primes);

                SqliteConnection.ClearAllPools();
                File.Delete(path);

                foreach (int i in primes)
                    if (!Function.IsPrime(i))
                        Assert.Fail($"Wrong calculations - {i} is not a prime number!");
            }

            catch (Exception ex)
            {
                Assert.Fail($"Expected no exceptions, but got {ex.Message}");
            }
        }
    }
}
