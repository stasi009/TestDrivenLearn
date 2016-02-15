using System;
using NUnit.Framework;

namespace CSharpBasicTest.Reflection
{
    [TestFixture]
    public sealed class TestType
    {
        private sealed class Point
        {
            public int X { get; set; }
            public int Y { get; set; }
        }

        [Test]
        public void TestGetType()
        {
            var p = new Point();
            Assert.AreEqual(p.GetType(), typeof(Point));

            // class 'Type' has override "=="
            Assert.True(p.GetType() == typeof(Point));
            Assert.True(p is Point);
        }

        [Test]
        public void TestTypeProperties()
        {
            Type type = typeof(Point);
            Assert.AreEqual("Point", type.Name);
            Assert.AreEqual("CSharpBasicTest.Reflection.TestType+Point", type.FullName);

            var p = new Point();
            Assert.AreEqual("Int32", p.X.GetType().Name);
            Assert.AreEqual("System.Int32", p.X.GetType().FullName);
        }
    }
}
