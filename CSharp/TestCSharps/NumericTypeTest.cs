
using System;

using NUnit.Framework;

namespace CSharpBasicTest
{
    [TestFixture]
    public sealed class NumericTypeTest
    {
        [Test]
        [ExpectedException(typeof(OverflowException))]
        public void TestOverflowException()
        {
            ushort uplimit = (ushort)(Math.Pow(2, 16) - 1);

            checked
            {
                uplimit += 1;
            }
        }

        [Test]
        [ExpectedException(typeof(OverflowException))]
        public void TestOverflowWhenCast()
        {
            ushort overflowed = checked((ushort)(Math.Pow(2, 16) + 1));
        }

        [Test]
        public void TestStringParse()
        {
            int oriNumber = 100;
            string numString = oriNumber.ToString();
            int cpyNumber = int.Parse(numString);
            Assert.AreEqual(cpyNumber, oriNumber);
        }

        /// <summary>
        /// to check when int.parse a float representation, to see whether it will throw an exception
        /// or first parse and automatically cast
        /// !!!!!!!! Answer is : throw an exception
        /// </summary>
        [Test]
        [ExpectedException(typeof(FormatException))]
        public void TestParseAndCast()
        {
            Assert.AreEqual(100, int.Parse("100.9"));
        }

        /// <summary>
        /// this testcase shows that "Parse" function can parse numerical strings with white spaces inside it
        /// </summary>
        [Test]
        public void TestParseWithSpace()
        {
            Assert.AreEqual(12, int.Parse("12"));
            Assert.AreEqual(108, int.Parse("                   108                "));
        }

        /// <summary>
        /// test the usage of positive and negative infinity
        /// </summary>
        [Test]
        public void TestInfinity()
        {
            Assert.AreEqual(double.PositiveInfinity, 1.0 / 0.0);
            Assert.AreEqual(double.NegativeInfinity, -1.0 / 0.0);
        }

        /// <summary>
        /// test the usage of NaN (NaN: Not A Number)
        /// </summary>
        [Test]
        public void TestNan()
        {
            Assert.AreEqual(double.NaN, 0.0 / 0.0);
            Assert.IsTrue(double.IsNaN(0.0 / 0.0));
        }

        public void TestNonAssigned()
        {
            int src;
            // chekanote: only working as the member field, these number variables will be automatically initialized (to be zero)
            // but working as local variable, these number will not be auto-initialized, so it is a syntax error
            // to use them before assigning them. Fortunately, the compiler will not pass the compilation in such case
            // int target = src;
        }

        [Test]
        public void TestBoxing()
        {
            int oriNumber = 10;
            object boxed = oriNumber;
            int cpyNumber = (int)boxed;// static cast to unbox
            Assert.AreEqual(oriNumber, cpyNumber);
        }

        /// <summary>
        /// get a string representation of a number in a specific format
        /// </summary>
        [Test]
        public void TestToString()
        {
            const float number = 31.415926f;

            Assert.AreEqual("31.4", number.ToString("0.#"));
            Assert.AreEqual("31.42", number.ToString("0.##"));
            Assert.AreEqual("31.416", number.ToString("0.###"));

            Assert.AreEqual("31.4", number.ToString("#.#"));
            Assert.AreEqual("31.42", number.ToString("#.##"));
            Assert.AreEqual("31.416", number.ToString("#.###"));

            // ------------ "0.#" and "#.#" is different when
            // ------------ dealing with 0 before the "."
            Assert.AreEqual(".314",0.314.ToString("#.###"));
            Assert.AreEqual("0.314", 0.314.ToString("0.###"));

            Assert.AreEqual(" 31.4", string.Format("{0,5:#.#}", number));
            Assert.AreEqual("  31.4", string.Format("{0,6:#.#}", number));
            Assert.AreEqual("31.4  ", string.Format("{0,-6:#.#}", number));
            Assert.AreEqual("31.42", string.Format("{0,5:#.##}", number));
        }
    }

    [TestFixture]
    public sealed class NullableTest
    {
        [Test]
        [ExpectedException(typeof(InvalidOperationException))]
        public void TestCastException()
        {
            int? number = null;
            int value = (int)number;
        }

        [Test]
        public void TestCompare()
        {
            int? empty = null;
            Assert.IsFalse(empty == 8);

            int? filled = 9;
            Assert.IsTrue(filled == 9);
            Assert.IsFalse(filled == 8);
        }
    }

    [TestFixture]
    public sealed class MathTest
    {
        [Test]
        public void TestFloor()
        {
            Assert.AreEqual(11, (int)Math.Floor(11.1));
            Assert.AreEqual(11, (int)Math.Floor(11.9));
            Assert.AreEqual(-12, (int)Math.Floor(-11.1));
            Assert.AreEqual(-12, (int)Math.Floor(-11.9));
        }

        [Test]
        public void TestCeiling()
        {
            Assert.AreEqual(12, (int)Math.Ceiling(11.1));
            Assert.AreEqual(12, (int)Math.Ceiling(11.9));
            Assert.AreEqual(-11, (int)Math.Ceiling(-11.1));
            Assert.AreEqual(-11, (int)Math.Ceiling(-11.9));
        }

        [Test]
        public void TestRoundToInt()
        {
            Assert.AreEqual(11, (int)Math.Round(11.1));
            Assert.AreEqual(12, (int)Math.Round(11.9));
            Assert.AreEqual(-11, (int)Math.Round(-11.1));
            Assert.AreEqual(-12, (int)Math.Round(-11.9));
        }

        [Test]
        public void TestCastToInt()
        {
            Assert.AreEqual(11, (int)11.1);
            Assert.AreEqual(11, (int)11.9);
            Assert.AreEqual(-11, (int)-11.1);
            Assert.AreEqual(-11, (int)-11.9);
        }

        /// <summary>
        /// a convenient devide method which can both give quotient and remainder
        /// </summary>
        [Test]
        public void TestDivRem()
        {
            int remainder = 0;
            int quotient = Math.DivRem(10, 3, out remainder);

            Assert.AreEqual(3, quotient);
            Assert.AreEqual(1, remainder);
        }
    }
}