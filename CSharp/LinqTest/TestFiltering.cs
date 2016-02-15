
using System;
using System.Text;
using System.Linq;

using NUnit.Framework;

namespace LinqTest
{
    [TestFixture]
    sealed class TestWhere
    {
        // ================================================= //
        #region [ member variables ]

        private string[] m_names;

        #endregion

        // ================================================= //

        [SetUp]
        public void Setup()
        {
            m_names = new string[] { "Tom", "Dick", "Harry", "Mary", "Jay" };
        }

        [Test]
        public void Demo1()
        {
            var filterResult = from s in m_names
                               where s.EndsWith("y")
                               select s;
            CollectionAssert.AreEqual(new[] { "Harry", "Mary", "Jay" }, filterResult);
        }

        [Test]
        public void Demo2()
        {
            var filterResult = from s0 in m_names
                               where s0.Length > 3
                               let s1 = s0.ToUpper()
                               where s1.EndsWith("Y")
                               select s1;
            CollectionAssert.AreEqual(new[] { "HARRY", "MARY" }, filterResult);
        }

        [Test]
        public void TestIndexFiltering()
        {
            var filterResult = m_names.Where((s, index) => index % 2 == 0);
            CollectionAssert.AreEqual(new[] { "Tom", "Harry", "Jay" }, filterResult);
        }

        /// <summary>
        /// this test wants to show that the object instance after being filtered out
        /// is just the same as the original elements
        /// </summary>
        [Test]
        public void TestReferenceEquality()
        {
            string[] strarrays = { "cheka", "stasi", "kgb" };

            string qualified = (from s in strarrays
                                where s.Length == 3
                                select s).Single();
            Assert.AreSame(strarrays[2], qualified);
        }
    }

    [TestFixture]
    sealed class TestTakeSkip
    {
        private readonly string[] m_names = { "cheka", "KGB", "STASI", "MSS", "GRU" };

        [Test]
        public void TestTake()
        {
            var filterResult = m_names.Take(2);
            CollectionAssert.AreEqual(new[] { "cheka", "KGB" }, filterResult);
        }

        [Test]
        public void TestSkip()
        {
            var filterResult = m_names.Skip(3);
            CollectionAssert.AreEqual(new[] { "MSS", "GRU" }, filterResult);
        }

        [Test]
        public void TestPage()
        {
            // fetch the 3rd and 4th elements
            var result = m_names.Skip(2).Take(2);
            CollectionAssert.AreEqual(new[] { "STASI", "MSS" }, result);
        }

        /// <summary>
        /// test the usage of "TakeWhile": take until predicate is false
        /// </summary>
        [Test]
        public void TestTakeSkipWhile()
        {
            int[] numbers = { 3, 5, 2, 234, 4, 1 };

            CollectionAssert.AreEqual(new[] { 3, 5, 2 }, numbers.TakeWhile(n => n < 100));

            CollectionAssert.AreEqual(new[] { 234, 4, 1 }, numbers.SkipWhile(n => n < 100));
        }
    }

    [TestFixture]
    sealed class TestDistinct
    {
        [Test]
        public void TestRemoveDuplicate()
        {
            var filterResult = "HelloWorld".Distinct().ToArray();
            Assert.AreEqual("HeloWrd", new string(filterResult));
        }
    }
}