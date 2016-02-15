using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

using NUnit.Framework;

namespace CSharpBasicTest.collection
{
    [TestFixture]
    public sealed class SequentialEqualTest
    {
        private sealed class DoubleComparer : IEqualityComparer<double>
        {
            private readonly double m_delta;
            public DoubleComparer(double delta)
            {
                m_delta = Math.Abs( delta);
            }

            public bool Equals(double x, double y)
            {
                return Math.Abs(x - y) < m_delta;
            }

            public int GetHashCode(double obj)
            {
                return obj.GetHashCode();
            }
        }

        [Test]
        public void TestOrderMatters()
        {
            int[] array1 = {1, 2};
            int[] array2 = {2, 1};
            IList<int> list1 = new List<int>{1,2};

            bool isEqual = array1.SequenceEqual(list1);
            Assert.IsTrue(isEqual);

            isEqual = list1.SequenceEqual(array2);
            Assert.IsFalse(isEqual);
        }

        [Test]
        public void TestCustomEqual()
        {
            double[] array = {4.0, 3.14};
            IList<double> list = new List<double> {4.01,3.16};

            bool defaultEqual = array.SequenceEqual(list);
            Assert.IsFalse(defaultEqual);

            bool customEqual = array.SequenceEqual(list, new DoubleComparer(0.1));
            Assert.IsTrue(customEqual);
        }

        [Test]
        public void TestDictEqual()
        {
            IDictionary<int,string> dict1 = new Dictionary<int, string>
                                                {
                                                    {1,"stasi"},
                                                    {88,"cheka"}
                                                };
            IDictionary<int, string> dict2 = new Dictionary<int, string>
                                                {
                                                    {88,"cheka"},
                                                    {1,"stasi"}
                                                };
            Assert.IsFalse(dict1.Equals(dict2));
            Assert.IsTrue(dict1.OrderBy(kv=>kv.Key).SequenceEqual(dict2.OrderBy(kv=>kv.Key)));
        }
    }// SequentialEqualTest
}
