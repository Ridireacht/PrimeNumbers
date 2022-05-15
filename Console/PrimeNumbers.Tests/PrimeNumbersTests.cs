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
            List<int> numList = new() { 32213, 85829, 91297, 50923, 92767, 70099, 51713, 84299, 74317, 52571 };
            bool flag = true;


            // act
            foreach (int i in numList)
                if (!Calculator.IsPrime(i))
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
            List<int> numList = new() { 14133, 51781, 12072, 77405, 41456, 11989, 74389, 21387, 67406, 92025 };
            bool flag = true;


            // act
            foreach (int i in numList)
                if (Calculator.IsPrime(i))
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
            List<string> inputList = new() { "7", "12", "y", "-5", "1.1", "4.6", "100000", "-121212", "y", "sample_text", "8xybr8y3b1x8br8", "nn", "#!@#%*&$^)**!&?¹;%:+", "n", "12 31 42", "yyy" };
            int ints = 0;


            // act
            try
            {
                foreach (string input in inputList)
                    if (int.TryParse(input, out int x) && (x > 1))
                        ints++;
            }

            catch (Exception ex)
            {
                Assert.Fail($"Expected no exceptions, but got {ex.Message}");
            }


            // assert
            Assert.AreEqual(true, ints == 3);
        }


        [TestMethod]
        public void SetByInputFlag_randomInput_correctChecks()
        {
            // arrange
            List<string> inputList = new() { "7", "12", "y", "-5", "1.1", "4.6", "100000", "-121212", "y", "sample_text", "8xybr8y3b1x8br8", "nn", "#!@#%*&$^)**!&?¹;%:+", "n", "12 31 42", "yyy" };
            int yes_nots = 0;


            // act
            try
            {
                foreach (string input in inputList)
                    if (Regex.IsMatch(input, "^y$") || Regex.IsMatch(input, "^n$"))
                        yes_nots++;
            }

            catch (Exception ex)
            {
                Assert.Fail($"Expected no exceptions, but got {ex.Message}");
            }


            // assert
            Assert.AreEqual(true, yes_nots == 3);
        }


        [TestMethod]
        public void CreateDatabase_createDB_noExceptionIsThrown()
        {
            // arrange & act & assert
            try
            {
                DB.CreateDatabase();
            }

            catch (Exception ex)
            {
                Assert.Fail($"Expected no exceptions, but got {ex.Message}");
            }
        }


        [TestMethod]
        public void FillDatabase_fillWithNums_noExceptionIsThrown()
        {
            // arrange
            List<int> numList = new();
            Calculator.CalculateNoDB(ref numList, 13, 200000);


            // act & assert
            try
            {
                DB.CreateDatabase();
                DB.FillDatabase(numList);
            }

            catch (Exception ex)
            {
                Assert.Fail($"Expected no exceptions, but got {ex.Message}");
            }
        }


        [TestMethod]
        public void GetFromDatabase_getNums_correctLists()
        {
            // arrange
            List<int> numList = new();
            List<int> checkList = new();


            // act & assert
            try
            {
                DB.CreateDatabase();
                DB.GetFromDatabase(ref numList, 13, 200000);
                Calculator.CalculateNoDB(ref checkList, 13, 200000);

                Assert.AreEqual(true, numList.SequenceEqual(checkList));
            }

            catch (Exception ex)
            {
                Assert.Fail($"Expected no exceptions, but got {ex.Message}");
            }
        }


        [TestMethod]
        public void I_ClearDatabase_clearDB_noExceptionIsThrown()
        {
            // act & assert
            try
            {
                DB.CreateDatabase();
                DB.ClearDatabase();
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
            List<int> numList = new();


            // act & assert
            try
            {
                Calculator.CalculateNoDB(ref numList, 2, 100000);

                foreach (int i in numList)
                    if (!Calculator.IsPrime(i))
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
            List<int> numList = new();


            // act & assert
            try
            {
                Calculator.CalculateNoDB(ref numList, 2, 200000);

                foreach (int i in numList)
                    if (!Calculator.IsPrime(i))
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
            List<int> numList = new();


            // act & assert
            try
            {
                DB.CreateDatabase();

                Calculator.CalculateNoDB(ref numList, 2, 100000);

                SqliteConnection.ClearAllPools();
                File.Delete("calculated_primes.db");

                foreach (int i in numList)
                    if (!Calculator.IsPrime(i))
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
            List<int> numList = new();


            // act & assert
            try
            {
                DB.CreateDatabase();

                Calculator.CalculateNoDB(ref numList, 2, 200000);

                SqliteConnection.ClearAllPools();
                File.Delete("calculated_primes.db");

                foreach (int i in numList)
                    if (!Calculator.IsPrime(i))
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
                    if (!Calculator.IsPrime(i))
                        Assert.Fail($"Wrong calculations - {i} is not a prime number!");
            }

            catch (Exception ex)
            {
                Assert.Fail($"Expected no exceptions, but got {ex.Message}");
            }
        }
    }
}
