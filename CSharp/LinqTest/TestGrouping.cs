
using System;
using System.Collections.Generic;
using System.Linq;

using NUnit.Framework;

namespace LinqTest
{
    [TestFixture]
    sealed class TestGrouping
    {
        // ---------------------------------------------------- //
        #region [ helper ]

        private Tuple<int, string>[] m_records;

        [SetUp]
        public void Setup()
        {
            m_records = new Tuple<int, string>[] 
            {
                Tuple.Create(1,"cheka"),
                Tuple.Create(2,"tom"),
                Tuple.Create(1,"henry"),
                Tuple.Create(2,"dick"),
                Tuple.Create(2,"mary")
            };
        }

        /// <summary>
        /// !!!!!!!!!!!! actually we cannot guarantee the order
        /// </summary>
        private void CheckGroupResult(IGrouping<int, string>[] classGroups)
        {
            Assert.AreEqual(1, classGroups[0].Key);
            CollectionAssert.AreEqual(new[] { "cheka", "henry" }, classGroups[0]);

            Assert.AreEqual(2, classGroups[1].Key);
            CollectionAssert.AreEqual(new[] { "tom", "dick", "mary" }, classGroups[1]);
        }

        #endregion

        // ---------------------------------------------------- //
        #region [ test ]

        [Test]
        public void Demo1()
        {
            var classGroups = m_records.GroupBy(t => t.Item1, t => t.Item2).ToArray();
            CheckGroupResult(classGroups);
        }

        [Test]
        public void Demo2WithQuerySyntax()
        {
            IEnumerable<IGrouping<int, string>> groupQuery = from record in m_records
                                                             group record.Item2 by record.Item1;
            IGrouping<int, string>[] groupResults = groupQuery.ToArray();
            CheckGroupResult(groupResults);
        }

        [Test]
        public void TestContinueQuery()
        {
            var query = from record in m_records
                        group record.Item2 by record.Item1
                            into grouping
                            where grouping.Count() == 3
                            select grouping;
            IGrouping<int, string>[] results = query.ToArray();
            Assert.AreEqual(1, results.Length);
            Assert.AreEqual(2, results[0].Key);
            CollectionAssert.AreEqual(new[] { "tom", "dick", "mary" }, results[0]);
        }

        /// <summary>
        /// use the "element selector" to select element in each group
        /// use the "result selector" to 'reduce' multiple elements into a single result
        /// </summary>
        [Test]
        public void TestElementAndResultSelector()
        {
            var originalInputs = new Tuple<int, float>[]
                                     {
                                         Tuple.Create(1,1.1f),
                                         Tuple.Create(2,2.2f),
                                         Tuple.Create(1,1.2f),
                                         Tuple.Create(2,2.3f),
                                         Tuple.Create(1,1.8f)
                                     };
            Tuple<int, float>[] results = originalInputs.GroupBy(
                t => t.Item1,
                t => t.Item2 * 2,
                (key, elements) => Tuple.Create(key, elements.Sum()))
                .OrderBy(t => t.Item1)
                .ToArray();

            Assert.AreEqual(2, results.Length);

            Assert.AreEqual(1, results[0].Item1);
            Assert.AreEqual(8.2f, results[0].Item2, 0.001);

            Assert.AreEqual(2, results[1].Item1);
            Assert.AreEqual(9.0f, results[1].Item2, 0.001);
        }

        #endregion
    }
}