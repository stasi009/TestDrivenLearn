
using System;
using System.Collections.Generic;
using System.Linq;

using NUnit.Framework;

namespace LinqTest
{
    sealed class Record
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;

            if (object.ReferenceEquals(this, obj))
                return true;

            if (!(obj is Record))
                return false;

            Record otherrecord = (Record)obj;
            return (this.Id == otherrecord.Id)
                && (this.Name.Equals(otherrecord.Name));
        }

        public override int GetHashCode()
        {
            return this.ToString().GetHashCode();
        }

        public override string ToString()
        {
            return string.Format("{0}.{1}", this.Id.ToString(), this.Name);
        }

        public static IEnumerable<Record> MakeSome()
        {
            yield return new Record { Id = 1, Name = "record1" };
            yield return new Record { Id = 2, Name = "record2" };
        }
    }

    [TestFixture]
    sealed class TestBasicFunctions
    {
        [Test]
        public void TestFluentSyntaxWhere()
        {
            string[] arStrings = new string[] { "Tom", "Dick", "Harry" };

            CollectionAssert.AreEqual(new string[] { "Dick", "Harry" }, arStrings.Where(s => s.Length >= 4));

            CollectionAssert.AreEqual(new[] { "Harry" }, arStrings.Where(s => s.Contains("a")));
        }

        [Test]
        public void TestQueryExpressionWhere()
        {
            string[] arStrings = new string[] { "Tom", "Dick", "Harry" };

            IEnumerable<string> results = from s in arStrings
                                          where s.Contains("a")
                                          select s;
            CollectionAssert.AreEqual(new[] { "Harry" }, results);

            results = from s in arStrings
                      where s.Length >= 4
                      select s;
            CollectionAssert.AreEqual(new string[] { "Dick", "Harry" }, results);
        }

        [Test]
        public void DemoTest1()
        {
            string[] names = { "Tom", "Dick", "Harry", "Mary", "Jay" };
            string[] expected = new[] { "JAY", "MARY", "HARRY" };

            IEnumerable<string> results = names
                .Where(s => s.Contains("a"))
                .OrderBy(s => s.Length)
                .Select(s => s.ToUpper());

            CollectionAssert.AreEqual(expected, results);

            results = from name in names
                      where name.Contains("a")
                      orderby name.Length
                      select name.ToUpper();
            CollectionAssert.AreEqual(expected, results);
        }

        [Test]
        public void TestOrderOperation()
        {
            int[] oriNumers = { 10, 9, 8, 7, 6 };

            CollectionAssert.AreEqual(new[] { 10, 9, 8 }, oriNumers.Take(3));
            CollectionAssert.AreEqual(new[] { 7, 6 }, oriNumers.Skip(3));
            CollectionAssert.AreEqual(new[] { 6, 7, 8, 9, 10 }, oriNumers.Reverse());
        }

        [Test]
        public void TestElementOperator()
        {
            int[] oriNumbers = { 1, 2, 3, 4, 5 };

            Assert.AreEqual(1, oriNumbers.First());
            Assert.AreEqual(5, oriNumbers.Last());
            Assert.AreEqual(2, oriNumbers.ElementAt(1));
            Assert.AreEqual(5, oriNumbers.OrderBy(n => 10 - n).First());
        }

        [Test]
        public void TestAggregatorOperator()
        {
            int[] oriNumbers = { 100, -5, 99, -3, 1, 2, -89, 3, 4, 5 };
            Assert.AreEqual(oriNumbers.Length, oriNumbers.Count());
            Assert.AreEqual(-89, oriNumbers.Min());
            Assert.AreEqual(100, oriNumbers.Max());

            Assert.AreEqual(2, (from n in oriNumbers where n > 10 select n).Count());
            Assert.AreEqual(-5, (from n in oriNumbers where n < 0 select n).First());
        }

        [Test]
        public void TestMinMax()
        {
            int[] values = { -100, 2, 3, -5, 1 };

            Assert.AreEqual(-100, values.Min());
            Assert.AreEqual(1, values.Min(n => Math.Abs(n)));

            Assert.AreEqual(3, values.Max());
            // !!!!!!!!!!!!!!!! pay attention that Max return the value after being transformed by selector
            Assert.AreEqual(100, values.Max(n => Math.Abs(n)));
        }

        [Test]
        public void TestQualifyOperator()
        {
            int[] oriNumbers = { 100, 99, -3, 1, 2, 3, 4, 5 };

            Assert.IsTrue(oriNumbers.Contains(-3));
            Assert.IsFalse(oriNumbers.Contains(89));

            // ---------------- equal to ask whether empty
            Assert.IsTrue(oriNumbers.Any());
            Assert.IsFalse((new string[] { }).Any());

            Assert.IsTrue(oriNumbers.Any(n => n > 50));
            Assert.IsFalse(oriNumbers.Any(n => { return n > 10 && n < 20; }));
        }

        [Test]
        public void TestTwoEnumOperator()
        {
            int[] seq1 = { 1, 2, 3 };
            int[] seq2 = { 3, 4, 5 };

            CollectionAssert.AreEqual(new int[] { 1, 2, 3, 3, 4, 5 }, seq1.Concat(seq2));
            CollectionAssert.AreEqual(new int[] { 3, 4, 5, 1, 2 }, seq2.Union(seq1));
        }

        [Test]
        public void TestDeferredExecute()
        {
            IList<int> oriList = new List<int> { 1 };

            // construct but no executed
            var results = from n in oriList select n * 10;

            oriList.Add(2);

            CollectionAssert.AreEqual(new int[] { 10, 20 }, results);
        }

        /// <summary>
        /// this test also testify the "Deferred Execution" feature of LINQ expression
        /// modification can be done on a on-fly IEnumerable which returned by LINQ expression
        /// but modification will not be saved, becaue when using IEnumerable next time, LINQ expression
        /// will be executed once more, a totally new sequence will be returned
        /// </summary>
        [Test]
        public void TestChaningEnumerable()
        {
            Record[] expected = new Record[]
                {
                    new Record { Id = 1, Name = "record1" },
                    new Record { Id = 2, Name = "record2" }
                };

            Action<IEnumerable<Record>> changeAction = (records) =>
                {
                    foreach (Record rd in records)
                    {
                        // changes will not be saved back to sequence
                        rd.Id *= 10;
                        rd.Name = "NewName";
                    }
                };

            // ----------------- LINQ sequence cannot save changes
            IEnumerable<Record> linqExpression = Record.MakeSome();
            changeAction(linqExpression);
            CollectionAssert.AreEqual(expected, linqExpression);// linqExpression will return a totally new sequence

            // ----------------- concrete container save changes
            IEnumerable<Record> concreteContainer = Record.MakeSome().ToArray();
            changeAction(concreteContainer);
            CollectionAssert.AreNotEqual(expected, concreteContainer);
        }

        /// <summary>
        /// LINQ expression will be executed each time when enumerated
        /// </summary>
        [Test]
        public void TestRevaluated()
        {
            IList<int> oriList = new List<int> { 2 };

            // construct but no executed
            var results = from n in oriList select n * n;

            CollectionAssert.AreEqual(new int[] { 4 }, results);

            int[] freezeArray = results.ToArray();

            // executed again
            oriList.Add(5);
            CollectionAssert.AreEqual(new int[] { 4, 25 }, results);

            // test freeze array
            CollectionAssert.AreEqual(new int[] { 4 }, freezeArray);
        }

        [Test]
        public void TestSubquery()
        {
            string[] names = { "cheka", "abc", "hj", "56", "absxy" };
            string[] expected = { "hj", "56" };

            var shortestNames = from s in names
                                where s.Length == (from s2 in names select s2.Length).Min()
                                select s;
            CollectionAssert.AreEqual(expected, shortestNames);

            // -------------------- show that subquery are repeated executed
            int exeCounter = 0;
            shortestNames = names.Where(s =>
            {
                ++exeCounter;
                return s.Length == names.Min(s2 => s2.Length);
            });
            CollectionAssert.AreEqual(expected, shortestNames);
            Assert.AreEqual(names.Length, exeCounter);
        }
    }

    [TestFixture]
    sealed class TestDefaultIfEmpty
    {
        private bool m_isVisited;
        private int? m_lastNumber;
        private int m_visitCounter;

        [SetUp]
        public void Setup()
        {
            m_isVisited = false;
            m_lastNumber = null;
            m_visitCounter = 0;
        }

        private void Iterate(IEnumerable<int> enumerableInts)
        {
            foreach (int number in enumerableInts)
            {
                m_isVisited = true;
                m_lastNumber = number;
                ++m_visitCounter;
            }
        }

        [Test]
        public void TestCommonEmptyList()
        {
            Iterate(new List<int>());
            Assert.IsFalse(m_isVisited);
            Assert.IsNull(m_lastNumber);
            Assert.AreEqual(0, m_visitCounter);
        }

        [Test]
        public void TestWithDefaultIfEmpty()
        {
            Iterate(new List<int>().DefaultIfEmpty());
            Assert.IsTrue(m_isVisited);
            Assert.AreEqual(0, m_lastNumber);
            Assert.AreEqual(1, m_visitCounter);
        }
    }

    [TestFixture]
    sealed class TestCompositeQuery
    {
        private string[] m_names;

        [SetUp]
        public void Setup()
        {
            m_names = new string[] { "Tom", "Dick", "Harry", "Mary", "Jay" };
        }

        [Test]
        public void TestInto()
        {
            // -------------------- using keyword into
            var result = from s in m_names
                         select s.Replace("a", "").Replace("e", "").Replace("i", "").Replace("o", "").Replace("u", "")
                             into noVowls
                             where noVowls.Length > 2
                             orderby noVowls
                             select noVowls;
            CollectionAssert.AreEqual(new[] { "Dck", "Hrry", "Mry" }, result);
        }

        [Test]
        public void TestProgressiveQuery()
        {
            // -------------------- equal to progressive query
            var noVowelQuery = from s1 in m_names
                               select s1.Replace("a", "").Replace("e", "").Replace("i", "").Replace("o", "").Replace("u", "");
            var result = from s2 in noVowelQuery
                         where s2.Length > 2
                         orderby s2.Length
                         select s2;
            CollectionAssert.AreEqual(new[] { "Dck", "Mry", "Hrry" }, result);
        }

        [Test]
        public void TestWrapQuery()
        {
            // -------------------- equal to use wrapping query
            // !!!!!!!!!!!!!!!!!!! we can see from here the difference between wrapping and subquery
            // !!!!!!!!!!!!!!!!!!! is that the position where that child query appears
            // !!!!!!!!!!!!!!!!!!! if it appears in the position of a delegate used in where, select, then it is subquery
            // !!!!!!!!!!!!!!!!!!! if it appears in "in", which is used as a chain query, then it is "composite query"
            var result = from s1 in
                             (
                             from s2 in m_names
                             select s2.Replace("a", "").Replace("e", "").Replace("i", "").Replace("o", "").Replace("u", "")
                             )
                         where s1.Length > 2
                         orderby s1.Length descending
                         select s1;
            CollectionAssert.AreEqual(new[] { "Hrry", "Dck", "Mry" }, result);
        }
    }
}