
using System;
using NUnit.Framework;

namespace CSharpBasicTest
{
    [TestFixture]
    sealed class TupleTest
    {
        [Test]
        public void TestCreateAsVar()
        {
            string name = "name";
            int value = 1;
            var nameValueTuple = Tuple.Create(name, value);

            // below clause testify that Tuple is readonly
            int temp = nameValueTuple.Item2;
            // nameValueTuple.Item2 = temp;

            Assert.AreEqual(name, nameValueTuple.Item1);
            Assert.AreEqual(value, nameValueTuple.Item2);
        }

        [Test]
        public void TestCreateWithConstructor()
        {
            int id = 5;
            float score = 99.9f;

            Tuple<int, float> t = new Tuple<int, float>(id, score);

            Assert.AreEqual(id, t.Item1);
            Assert.AreEqual(score, t.Item2, 1e-6);

            // !!!!!!!!!!!! property of tuple is readonly
            // t.Item2 += 1;
        }

        [Test]
        public void TestEqual()
        {
            string name = "STASI+CHEKA+KGB";
            int id = 100;

            var tuple1 = Tuple.Create(name, id);
            var tuple2 = Tuple.Create(name, id);

            Assert.IsFalse(tuple1 == tuple2);
            Assert.AreEqual(tuple1, tuple2);
        }
    }
}