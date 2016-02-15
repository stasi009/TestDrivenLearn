
using System;
using System.Collections.Generic;
using NUnit.Framework;

namespace CSharpBasicTest
{
    public struct Point
    {
        int x, y;

        public int X
        {
            get { return x; }
            set { x = value; }
        }

        public int Y
        {
            get { return y; }
            set { y = value; }
        }

        // chekanote: we don't need to explicitly define no-argument constructor
        public Point(int x, int y) { this.x = x; this.y = y; }
    }

    [TestFixture]
    public sealed class StructTest
    {
        private Point m_pnt;

        // chekanote: A struct CANNOT have a parameterless constructor
        // A parameterless constructor that you can’t override implicitly exists. This per-forms a bitwise-zeroing of its fields.
        [Test]
        public void TestDefaultConstructor()
        {
            var pnt = new Point();
            Assert.AreEqual(0, pnt.X);
            Assert.AreEqual(0, pnt.Y);
        }

        [Test]
        public void TestConstruction()
        {
            int number = 10;

            ///////////////////////// local variable
            Point pnt = new Point { X = number };
            Assert.AreEqual(number, pnt.X);

            ///////////////////////// member variable
            // !!!!!!!!!!!!!!!!!! this test shows that for local struct, this struct instance must be assigned
            // !!!!!!!!!!!!!!!!!! explicitly, but for member variable, 
            // !!!!!!!!!!!!!!!!!! this constructor will be invoked automatically, so this member struct instance
            // !!!!!!!!!!!!!!!!!! has no need to be assigned explicitly
            m_pnt.Y = number;
            Assert.AreEqual(number, m_pnt.Y);
        }

        [Test]
        public void TestClone()
        {
            Point pnt1 = new Point(10, 20);
            Point pnt2 = pnt1;
            // actually, below codes are comparing the boxed object, it should always shows that the two objects are different
            Assert.AreNotSame(pnt1, pnt2);
            Assert.AreEqual(pnt1, pnt2);

            // when assign, a new copy is made
            // then future changes on one of them will not affect the other one
            pnt2.X = 100;
            pnt2.Y = 2200;
            Assert.AreEqual(100, pnt2.X);
            Assert.AreEqual(2200, pnt2.Y);

            Assert.AreEqual(10, pnt1.X);
            Assert.AreEqual(20, pnt1.Y);
        }

        // The default implementation of the Equals method uses reflection to compare the corresponding fields of obj and this instance. 
        // chekanote: this means without explicit definition, 
        // we can have "value-based equal" comparing feature
        // But since the default implementation is based on reflection, 
        // so Override the Equals method for a particular type to improve the performance of the method and more closely represent the concept of equality for the type.
        [Test]
        public void TestDefaultEqual()
        {
            var pnt1 = new Point(1, 2);
            var pnt2 = new Point(1, 2);
            Assert.AreEqual(pnt1, pnt2);
            Assert.IsTrue(pnt1.Equals(pnt2));
            // Assert.IsTrue(pnt1 == pnt2);// chekanote: "==" cannot be applied

            var pnt3 = new Point(100, 200);
            Assert.AreNotEqual(pnt1, pnt3);
            Assert.IsFalse(pnt1.Equals(pnt3));
        }

        /// <summary>
        /// this test demonstrate that a struct will be copied when being inserted into a collection
        /// </summary>
        [Test]
        public void TestCopyWhenInsertIntoCollection()
        {
            const int PosX0 = 10;
            const int PosY0 = 20;
            Point pnt1 = new Point(PosX0, PosY0);
            // chekanote: when inserted into collection, struct will be copied
            IList<Point> points = new List<Point> { pnt1 };

            // so when original struct changes, the copied one in the collection remains the same
            pnt1.X = 888;
            pnt1.Y = 999;

            Assert.AreEqual(PosX0, points[0].X);
            Assert.AreEqual(PosY0, points[0].Y);
        }
    }
}