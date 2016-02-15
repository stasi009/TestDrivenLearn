
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using NUnit.Framework;

namespace LinqTest
{
    [TestFixture]
    sealed class TestAggregation
    {
        [Test]
        public void TestCount()
        {
            int[] numbers = { 1, 2, 3, 4, 5 };
            Assert.AreEqual(2, numbers.Count(n => n % 2 == 0));
        }

        /// <summary>
        /// Pay much attention that if we assign a delegate for "Min" and "Max"
        /// that delegate is not the predicate to help us find the maxinum and minimun value within a certain range
        /// but that delegate should be a "selector", that first project each element
        /// and find the maximum and mininum among the values after being projected
        /// and the maximum and minimum value is the value after being projected, not the original value
        /// </summary>
        [Test]
        public void TestMaxMin()
        {
            int[] numbers = { 1, 2, 3, 4, 5, 6, 7 };

            Assert.AreEqual(1, numbers.Min());
            Assert.AreEqual(7, numbers.Max());

            Assert.AreEqual(0, numbers.Min(n => n % 3));
            Assert.AreEqual(3, numbers.Max(n => n % 4));
        }

        [Test]
        public void TestStringReduce()
        {
            int[] numbers = new int[] { 1, 2, 3 };

            StringBuilder strBuilder = new StringBuilder();
            numbers.Aggregate(strBuilder, (sb, number) => sb.Append(string.Format("{0},", number.ToString())));

            Assert.AreEqual("1,2,3,", strBuilder.ToString());
        }

        [Test]
        public void TestFindOriItemWithLimitValue()
        {
            Tuple<string, int>[] records = 
            {
                Tuple.Create("henry",50),
                Tuple.Create("cheka",100),
                Tuple.Create("dick",10),
                Tuple.Create("mary",80)
            };

            // these two numbers will be used in a subquery, so it will efficient to calculate them
            // before being used in sub-query
            // instead of being repeatedly calculated each time in the subquery
            int maxScore = records.Max(r => r.Item2);
            int minScore = records.Min(r => r.Item2);

            string maxPerson = (from record in records
                                where record.Item2 == maxScore
                                select record.Item1).First();
            Assert.AreEqual("cheka", maxPerson);

            string minPerson = (from record in records
                                where record.Item2 == minScore
                                select record.Item1).First();
            Assert.AreEqual("dick", minPerson);
        }

        private Tuple<string, int>[] CreateRecords()
        {
            Tuple<string, int>[] records = 
            {
                Tuple.Create("henry",50),
                Tuple.Create("cheka",100),
                Tuple.Create("dick",10),
                Tuple.Create("mary",80)
            };
            return records;
        }

        [Test]
        public void TestSumAverage()
        {
            Tuple<string, int>[] records = CreateRecords();

            // -------------- calculate by manually loop
            int sumByLoop = 0;
            foreach (Tuple<string, int> record in records)
                sumByLoop += record.Item2;
            float averageByLoop = ((float)sumByLoop) / records.Length;

            // -------------- check with result from linq
            Assert.AreEqual(sumByLoop, records.Sum(r => r.Item2));
            Assert.AreEqual(averageByLoop, records.Average(r => r.Item2), 1e-6);
        }

        [Test]
        public void TestSumWithSelector()
        {
            int[] numbers = { 1, 2, 3 };
            Assert.AreEqual(14, numbers.Sum(n => n * n));
        }

        /// <summary>
        /// unseed aggregation will use the first element as the seed
        /// and loop from the second element
        /// !!!!!!!!!!!!!!!!!!! from this test, we can see that no-seed aggregation has the signature as 
        /// "public static TSource Aggregate<TSource>(	this IEnumerable<TSource> source,	Func<TSource, TSource, TSource> func)"
        /// so it cannot project into another type, we can only use the same type of each element
        /// </summary>
        [Test]
        public void TestUnseedAggregation()
        {
            Tuple<string, int>[] records = CreateRecords();
            var sumRecord = records.Aggregate((oriSum, r) => Tuple.Create(string.Empty, oriSum.Item2 + r.Item2));
            Assert.AreEqual(240, sumRecord.Item2);
        }

        /// <summary>
        /// always remember, different from seeded aggregation, unseeded aggregation starts looping
        /// from the second element, instead for the first
        /// </summary>
        [Test]
        public void TestUnseedAggLoopFromSecond()
        {
            int[] numbers = { 2, 3, 4 };
            int result = numbers.Aggregate((total, n) => total + n * n);
            Assert.AreNotEqual(2 * 2 + 3 * 3 + 4 * 4, result);
            Assert.AreEqual(2 + 3 * 3 + 4 * 4, result);
        }

        [Test]
        public void TestSeedAggregation()
        {
            Tuple<string, int>[] records = CreateRecords();
            // !!!!!!!!!! we can use "+=", but use "+" is enough
            // because the expression will be translated into "return total + r.item2", no need to keep change of total
            // the state of "total" will not be kept, totally stateless
            int sum = records.Aggregate(0, (total, r) => total + r.Item2);
            Assert.AreEqual(240, sum);
        }

        [Test]
        public void TestAppendByAggregate()
        {
            int[] numArray = { 1, 2, 3, 4, 5 };

            IList<int> numList = numArray.Aggregate(
                new List<int>(),
                (alist, number) =>
                {
                    alist.Add(number);
                    return alist;
                });

            CollectionAssert.AreEqual(numArray, numList);
        }
    }
}