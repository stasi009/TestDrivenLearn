
using System;
using System.Linq;

using NUnit.Framework;

namespace LinqTest
{
    [TestFixture]
    sealed class TestElementOperation
    {
        private readonly int[] m_numbers = { 1, 2, 3, 4, 5 };

        [Test]
        public void TestFirst()
        {
            Assert.AreEqual(1, m_numbers.First());
            Assert.AreEqual(2, m_numbers.First(n => n % 2 == 0));

            // -------------------- dealing with non-exist situation
            Assert.Throws<InvalidOperationException>(() => { int nonExist = m_numbers.First(n => n > 10); });
            Assert.AreEqual(0, m_numbers.FirstOrDefault(n => n > 10));
        }

        [Test]
        public void TestLast()
        {
            Assert.AreEqual(5, m_numbers.Last());
            Assert.AreEqual(4, m_numbers.Last(n => n % 2 == 0));

            // -------------------- dealing with non-exist situation
            Assert.Throws<InvalidOperationException>(() => { int nonExist = m_numbers.Last(n => n > 100); });
            Assert.AreEqual(0, m_numbers.LastOrDefault(n => n > 100));
        }

        [Test]
        public void TestSingle()
        {
            Assert.AreEqual(3, m_numbers.Single(n => n % 3 == 0));

            // throw exception due to mutiple qualified elements
            Assert.Throws<InvalidOperationException>(() => { int duplicated = m_numbers.Single(n => n % 2 == 0); });

            // throw exception due to no qualified elements
            Assert.Throws<InvalidOperationException>(() => { int nonExist = m_numbers.Single(n => n > 100); });
            Assert.AreEqual(0, m_numbers.SingleOrDefault(n => n > 100));
        }

        [Test]
        public void TestElementAt()
        {
            for (int index = 0; index < m_numbers.Length; ++index)
                Assert.AreEqual(m_numbers[index], m_numbers.ElementAt(index));

            Assert.Throws<ArgumentOutOfRangeException>(() => { int nonExist = m_numbers.ElementAt(m_numbers.Length + 100); });
            Assert.AreEqual(0, m_numbers.ElementAtOrDefault(m_numbers.Length + 200));
        }
    }
}