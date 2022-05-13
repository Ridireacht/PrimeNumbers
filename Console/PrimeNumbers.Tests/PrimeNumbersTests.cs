using Microsoft.Data.Sqlite;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PrimeNumbers.Properties;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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
                if (!Calculation.IsPrime(i))
                {
                    flag = false;
                    break;
                }


            // assert
            Assert.AreEqual(true, flag);
        }


        [TestMethod]
        public void IsPrime_randomNonPrimes_returnsFalse()
        {
            // arrange
            List<int> nums = new() { 14133, 51781, 12072, 77405, 41456, 11989, 74389, 21387, 67406, 92025 };
            bool flag = true;


            // act
            foreach (int i in nums)
                if (Calculation.IsPrime(i))
                {
                    flag = false;
                    break;
                }


            // assert
            Assert.AreEqual(true, flag);
        }


        [TestMethod]
        public void SetByInputNum_randomInput_correctChecks()
        {
            // arrange
            List<string> inputs = new() { "7", "12", "y", "-5", "1.1", "4.6", "100000", "-121212", "y", "sample_text", "8xybr8y3b1x8br8", "nn", "#!@#%*&$^)**!&?¹;%:+", "n", "12 31 42", "yyy" };
            int count_ints = 0;


            // act
            try
            {
                foreach (string input in inputs)
                    if (int.TryParse(input, out int x) && (x > 1))
                        count_ints++;
            }

            catch (Exception ex)
            {
                Assert.Fail($"Expected no exceptions, but got {ex.Message}");
            }


            // assert
            Assert.AreEqual(true, count_ints == 3);
        }


        [TestMethod]
        public void SetByInputFlag_randomInput_correctChecks()
        {
            // arrange
            List<string> inputs = new() { "7", "12", "y", "-5", "1.1", "4.6", "100000", "-121212", "y", "sample_text", "8xybr8y3b1x8br8", "nn", "#!@#%*&$^)**!&?¹;%:+", "n", "12 31 42", "yyy" };
            int count_yes_not = 0;


            // act
            try
            {
                foreach (string input in inputs)
                    if (Regex.IsMatch(input, "^y$") || Regex.IsMatch(input, "^n$"))
                        count_yes_not++;
            }

            catch (Exception ex)
            {
                Assert.Fail($"Expected no exceptions, but got {ex.Message}");
            }


            // assert
            Assert.AreEqual(true, count_yes_not == 3);
        }


        [TestMethod]
        public void CreateDatabase_createAndDelete_noExceptionIsThrown()
        {
            // arrange & act & assert
            try
            {
                DB.CreateDatabase();
                SqliteConnection.ClearAllPools();
                File.Delete("calculated_primes.db");
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
                Calculation.CalculateNoDB(2, 100000, ref primes);

                foreach (int i in primes)
                    if (!Calculation.IsPrime(i))
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
                Calculation.CalculateNoDB(2, 200000, ref primes);

                foreach (int i in primes)
                    if (!Calculation.IsPrime(i))
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

            // act & assert
            try
            {
                DB.CreateDatabase();

                Calculation.CalculateNoDB(2, 100000, ref primes);

                SqliteConnection.ClearAllPools();
                File.Delete("calculated_primes.db");

                foreach (int i in primes)
                    if (!Calculation.IsPrime(i))
                        Assert.Fail($"Wrong calculations - {i} is not a prime number!");
            }

            catch (Exception ex)
            {
                Assert.Fail($"Expected no exceptions, but got {ex.Message}");
            }
        }


        [TestMethod]
        public void CalculateDB_multiThreading_correctCalculations()
        {
            // arrange
            List<int> primes = new();

            // act & assert
            try
            {
                DB.CreateDatabase();

                Calculation.CalculateNoDB(2, 200000, ref primes);

                SqliteConnection.ClearAllPools();
                File.Delete("calculated_primes.db");

                foreach (int i in primes)
                    if (!Calculation.IsPrime(i))
                        Assert.Fail($"Wrong calculations - {i} is not a prime number!");
            }

            catch (Exception ex)
            {
                Assert.Fail($"Expected no exceptions, but got {ex.Message}");
            }
        }


        [TestMethod]
        public void Verify_checkVerificationList_correctList()
        {
            // arrange
            List<int> verificationPrimes = Resources.verification_primes.Split('\t').Select(n => Convert.ToInt32(n)).ToList();

            // act & assert
            try
            {
                foreach (int i in verificationPrimes)
                    if (!Calculation.IsPrime(i))
                        Assert.Fail($"Wrong calculations - {i} is not a prime number!");
            }

            catch (Exception ex)
            {
                Assert.Fail($"Expected no exceptions, but got {ex.Message}");
            }
        }
    }
}
