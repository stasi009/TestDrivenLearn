
using System;
using System.Collections.Generic;
using System.Linq;

using NUnit.Framework;

namespace LinqTest
{
    [TestFixture]
    sealed class TestOrdering
    {
        [Test]
        public void Demo1()
        {
            IEnumerable<char> chars = "cheka".OrderBy(c => c);
            CollectionAssert.AreEqual(new[] { 'a', 'c', 'e', 'h', 'k' }, chars);
        }

        [Test]
        public void TestOrderByDescending()
        {
            int[] oriNumbers = { 1, 3, 2 };

            int[] ascOrderExpected = new int[] { 1, 2, 3 };
            int[] desOrderExpected = new int[] { 3, 2, 1 };

            CollectionAssert.AreEqual(ascOrderExpected, oriNumbers.OrderBy(n => n));
            CollectionAssert.AreEqual(desOrderExpected, oriNumbers.OrderByDescending(n => n));

            var descOrderResult = from n in oriNumbers orderby n descending select n;
            CollectionAssert.AreEqual(desOrderExpected, descOrderResult);
        }

        [Test]
        public void TestThenBy()
        {
            string[] names = { "Tom", "Dick", "Harry", "Mary", "Jay" };
            string[] expected = { "Jay", "Tom", "Dick", "Mary", "Harry" };

            var fluentQuery = names.OrderBy(s => s.Length).ThenBy(s => s);
            CollectionAssert.AreEqual(expected, fluentQuery);

            var queryExpression = from s in names
                                  orderby s.Length, s
                                  select s;
            CollectionAssert.AreEqual(expected, queryExpression);
        }

        class ComparerWithId : Comparer<Tuple<int, string>>
        {
            public override int Compare(Tuple<int, string> x, Tuple<int, string> y)
            {
                return x.Item1 - y.Item1;
            }
        }

        class ComparerWithName : Comparer<Tuple<int, string>>
        {
            public override int Compare(Tuple<int, string> x, Tuple<int, string> y)
            {
                return x.Item2.CompareTo(y.Item2);
            }
        }

        [Test]
        public void TestCustomComparer()
        {
            Tuple<int, string>[] oriTuples = new Tuple<int, string>[]
            {
                Tuple.Create(1,"stasi"),
                Tuple.Create(2,"cheka")
            };

            var orderByIdQuery = oriTuples.OrderBy(t => t, new ComparerWithId());
            CollectionAssert.AreEqual(oriTuples, orderByIdQuery);

            var orderByNameQuery = oriTuples.OrderBy(t => t, new ComparerWithName());
            CollectionAssert.AreEqual(oriTuples.Reverse(), orderByNameQuery);
        }
    }
}