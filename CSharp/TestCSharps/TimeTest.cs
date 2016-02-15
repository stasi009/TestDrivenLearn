using System;

using NUnit.Framework;

namespace CSharpBasicTest
{
    [TestFixture]
    sealed class TimespanTest
    {
        [Test]
        public void SimpleTest()
        {
            TimeSpan interval1 = new TimeSpan(2, 30, 0);
            TimeSpan interval2 = TimeSpan.FromHours(2.5);

            Assert.AreEqual(interval1, interval2);
        }

        [Test]
        public void TestMath()
        {
            TimeSpan ts1 = TimeSpan.FromHours(4.5);
            TimeSpan ts2 = TimeSpan.FromHours(4) + TimeSpan.FromMinutes(30);
            Assert.AreEqual(ts1, ts2);
        }

        [Test]
        public void TestGetProperty()
        {
            TimeSpan interval = TimeSpan.FromDays(10) - TimeSpan.FromSeconds(1);
            Assert.AreEqual(9, interval.Days);
            Assert.AreEqual(23, interval.Hours);
            Assert.AreEqual(59, interval.Minutes);
            Assert.AreEqual(59, interval.Seconds);
            Assert.AreEqual(0, interval.Milliseconds);
        }

        [Test]
        public void TestTotal()
        {
            TimeSpan interval = TimeSpan.FromMinutes(1.5);
            Assert.AreEqual(0.025, interval.TotalHours, 1e-6);
            Assert.AreEqual(1.5, interval.TotalMinutes);
            Assert.AreEqual(90, interval.TotalSeconds);
            Assert.AreEqual(90000, interval.TotalMilliseconds);
        }

        [Test]
        public void TestToString()
        {
            TimeSpan interval1 = new TimeSpan(2, 30, 45);
            string description = interval1.ToString(@"mm\:ss");
        }
    }// TimespanTest

    internal sealed class TimeTest
    {
        private static void TestString()
        {
            DateTime dt = DateTime.Now;
            string pattern = "mm:ss:fff";

            string description = dt.ToString(pattern);
            Console.WriteLine("description: {0}", description);

            DateTime parsed = DateTime.ParseExact(description, pattern, null);
            Console.WriteLine("Minutes={0},Seconds={1},MilliSeconds={2}", parsed.Minute, parsed.Second, parsed.Millisecond);
        }

        public static void TestMain()
        {
            TestString();
        }
    }
}
