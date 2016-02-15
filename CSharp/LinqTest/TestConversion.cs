using System;
using System.Collections.Generic;
using System.Linq;

using NUnit.Framework;

namespace LinqTest
{
    [TestFixture]
    class TestConversion
    {
        [Test]
        [ExpectedException(typeof(ArgumentException))]
        public void TestToDictionaryKeyError()
        {
            Tuple<int, string>[] records = new Tuple<int, string>[] 
            {
                Tuple.Create(1,"cheka"),
                Tuple.Create(1,"duplicated")
            };

            // !!!!!!!!! will throw exception due to same key existed
            IDictionary<int, string> dict = records.ToDictionary(t => t.Item1, t => t.Item2);
        }
    }
}
