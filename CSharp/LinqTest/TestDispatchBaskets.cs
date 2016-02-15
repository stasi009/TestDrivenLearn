using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

using NUnit.Framework;

namespace LinqTest
{
    [TestFixture]
    sealed class TestDispatchBaskets
    {
        [Test]
        public static void TestSkipTake()
        {
            int[] array = new int[] { 1, 2, 3, 4, 5 };
            IEnumerable<int> selected = array.Skip(2).Take(2);
            CollectionAssert.AreEqual(new int[] { 3, 4 }, selected);
        }

        private static Tuple<int, int>[] MakeBaskets(int total, int numBaskets)
        {
            int quotient, remainder;
            quotient = Math.DivRem(total, numBaskets, out remainder);

            Tuple<int, int>[] baskets = new Tuple<int, int>[numBaskets];
            int start = 0;
            for (int basket = 0; basket < numBaskets; basket++)
            {
                int count = basket < remainder ? quotient + 1 : quotient;
                baskets[basket] = Tuple.Create(start, count);
                start += count;
            }

            Debug.Assert(start == total);
            return baskets;
        }

        [Test]
        public static void TestMakeBaskets()
        {
            Tuple<int, int>[] baskets = MakeBaskets(7, 3);
            Assert.AreEqual(Tuple.Create(0, 3), baskets[0]);
            Assert.AreEqual(Tuple.Create(3, 2), baskets[1]);
            Assert.AreEqual(Tuple.Create(5, 2), baskets[2]);

            baskets = MakeBaskets(11, 4);
            Assert.AreEqual(Tuple.Create(0, 3), baskets[0]);
            Assert.AreEqual(Tuple.Create(3, 3), baskets[1]);
            Assert.AreEqual(Tuple.Create(6, 3), baskets[2]);
            Assert.AreEqual(Tuple.Create(9, 2), baskets[3]);
        }

        [Test]
        public static void TestDispatchIntoBaskets()
        {
            int[] array = new int[] { 9, 10, 3, 7, 88, 63, 21 };
            int numBaskets = 3;

            Tuple<int, int>[] baskets = MakeBaskets(array.Length, numBaskets);
            int[][] dispatched = new int[numBaskets][];

            int index = 0;
            foreach (Tuple<int, int> basket in baskets)
            {
                dispatched[index] = array.Skip(basket.Item1).Take(basket.Item2).ToArray();
                ++index;
            }

            CollectionAssert.AreEqual(new int[] { 9, 10, 3 }, dispatched[0]);
            CollectionAssert.AreEqual(new int[] { 7, 88 }, dispatched[1]);
            CollectionAssert.AreEqual(new int[] { 63, 21 }, dispatched[2]);
        }
    }
}
