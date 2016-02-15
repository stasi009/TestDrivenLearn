using System;
using NUnit.Framework;

namespace CSharpBasicTest.Advance
{
    [TestFixture]
    public sealed class TestNullableValues
    {
        [Test]
        [ExpectedException(typeof(InvalidOperationException))]
        public void TestCastException()
        {
            int? number = null;
            int value = (int)number;
        }

        [Test]
        public void TestEqual()
        {
            int? empty = null;
            Assert.IsFalse(empty == 8);

            int? filled = 9;
            Assert.IsTrue(filled == 9);
            Assert.IsFalse(filled == 8);
        }

        [Test]
        public void TestCompareOrder()
        {
            // chekanote: below codes demonstrate that null nullable-value
            // will always return false when comparing with normal value
            // whether checking equality or order
            int? empty = null;
            Assert.IsFalse(empty < int.MinValue);
            Assert.IsFalse(empty == int.MinValue);
            Assert.IsFalse(empty > int.MinValue);

            int? filled = 9;
            Assert.IsTrue(filled > 8);
            Assert.IsTrue(filled <= 10);
        }
    }
}
