
using System;
using System.Collections.Generic;
using System.Linq;

using NUnit.Framework;

namespace LinqTest
{
    [TestFixture]
    class TestQualify
    {
        [Test]
        public void TestContain()
        {
            Assert.IsTrue(new[] { 1, 2 }.Contains(2));
            Assert.IsFalse(new[] { 1 }.Contains(3));
        }

        [Test]
        public void TestAny()
        {
            Assert.IsFalse(new int[] { }.Any());
            Assert.IsFalse(new[] { 3, 5 }.Any(n => n % 2 == 0));
        }

        [Test]
        public void TestAll()
        {
            int[] numbers = { 1, 2, 3, 4, 5 };
            Assert.IsTrue(numbers.All(n => n < 6));
            Assert.IsFalse(numbers.All(n => n > 2));
        }

        [Test]
        public void TestSequenceEqual()
        {
            Assert.IsTrue(new[] { 1, 2, 3 }.SequenceEqual(new[] { 1, 2, 3 }));
            Assert.IsFalse(new[] { 2, 1, 3 }.SequenceEqual(new[] { 1, 2, 3 }));
        }
    }
}
