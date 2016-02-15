
using System;

using NUnit.Framework;

namespace CSharpBasicTest
{
    [TestFixture]
    class EnumTypeTest
    {
        enum Direction
        {
            NORTH,
            EAST,
            WEST,
            SOUTH
        }

        [Test]
        public void TestDefaultValue()
        {
            // need explicit cast from ENUM to INT
            Assert.AreEqual(0,(int)Direction.NORTH);
            Assert.AreEqual(1, (int)Direction.EAST);

            // need explicit cast from INT to ENUM
            Assert.AreEqual(Direction.WEST,(Direction)2);
            Assert.AreEqual(Direction.SOUTH,(Direction)3);
        }

        [Test]
        public void TestStringCast()
        {
            Assert.AreEqual("NORTH",Direction.NORTH.ToString());

            Assert.AreEqual(Direction.EAST,Enum.Parse(typeof(Direction),"east",true));
            Assert.AreEqual(Direction.SOUTH,Enum.Parse(typeof(Direction),"SOUTH"));
        }
    }
}