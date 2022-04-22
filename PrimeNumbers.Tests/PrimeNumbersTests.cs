using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;


namespace PrimeNumbers.Tests
{
    [TestClass]
    public class PrimeNumbersTests
    {
        [TestMethod]
        public void isPrime_returnsTrue()
        {
            // arrange
            List<int> nums = new List<int> { 32213, 85829, 91297, 50923, 92767, 70099, 51713, 84299, 74317, 52571 };
            bool flag = true;


            // act
            foreach (int i in nums)
                if (!Function.isPrime(i))
                {
                    flag = false;
                    break;
                }


            // assert
            Assert.AreEqual(flag, true);
        }


        [TestMethod]
        public void isPrime_returnsFalse()
        {
            // arrange
            List<int> nums = new List<int> { 14133, 51781, 12072, 77405, 41456, 11989, 74389, 21387, 67406, 92025 };
            bool flag = true;


            // act
            foreach (int i in nums)
                if (Function.isPrime(i))
                {
                    flag = false;
                    break;
                }


            // assert
            Assert.AreEqual(flag, true);
        }
    }
}
