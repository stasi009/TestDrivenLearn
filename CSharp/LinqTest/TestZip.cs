
using System;
using System.Linq;

using NUnit.Framework;

namespace LinqTest
{
    [TestFixture]
    sealed class TestZip
    {
        [Test]
        public void TestZipOperation()
        {
            int[] numbers = { 1, 2, 3 };
            string[] shortStrings = { "x", "y" };
            string[] longStrings = { "a", "b", "c", "d" };

            // --------------- ignore numbers
            var query1 = numbers.Zip(shortStrings, (n, s) => string.Format("{0}.{1}", n.ToString(), s));
            CollectionAssert.AreEqual(new string[] { "1.x", "2.y" }, query1);

            // --------------- ignore long strings
            var query2 = numbers.Zip(longStrings, (n, s) => string.Format("{0}{1}", n.ToString(), s));
            CollectionAssert.AreEqual(new string[] { "1a", "2b", "3c" }, query2);
        }
    }
}