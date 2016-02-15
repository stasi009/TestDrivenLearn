
using System;
using System.Linq;

using NUnit.Framework;

namespace LinqTest
{
    [TestFixture]
    sealed class TestSetOperation
    {
        private readonly int[] m_seq1 = { 1, 2, 3 };
        private readonly int[] m_seq2 = { 3, 4, 5 };

        [Test]
        public void TestConcat()
        {
            CollectionAssert.AreEqual(new int[] { 1, 2, 3, 3, 4, 5 }, m_seq1.Concat(m_seq2));
        }

        [Test]
        public void TestUnion()
        {
            CollectionAssert.AreEqual(new int[] { 3, 4, 5, 1, 2 }, m_seq2.Union(m_seq1));
        }

        [Test]
        public void TestIntersect()
        {
            int[] expected = new int[] { 3 };
            CollectionAssert.AreEqual(expected, m_seq1.Intersect(m_seq2));
            CollectionAssert.AreEqual(expected, m_seq2.Intersect(m_seq1));
        }

        [Test]
        public void TestExcept()
        {
            CollectionAssert.AreEqual(new int[] { 1, 2 }, m_seq1.Except(m_seq2));
            CollectionAssert.AreEqual(new int[] { 4, 5 }, m_seq2.Except(m_seq1));
        }
    }// TestSetOperation
}
