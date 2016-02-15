using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;

namespace CSharpBasicTest.collection
{
    [TestFixture]
    sealed class ListTest
    {
        // ------------------------------------------------------------- //
        [Test]
        public void TestCopyConstructor()
        {
            IList<string> oriList = new List<string> { "cheka" };
            IList<string> cpyList = new List<string>(oriList);

            // though the list is copied, but just a shallow copy, the elements remains the same
            Assert.AreNotSame(oriList, cpyList);
            Assert.AreSame(oriList[0], cpyList[0]);
        }

        [Test]
        public void TestConstructByCapacity()
        {
            int capacity = 10;
            IList<int> alist = new List<int>(capacity);

            Assert.AreEqual(capacity, (alist as List<int>).Capacity);
            Assert.AreEqual(0, alist.Count);
        }

        [Test]
        [ExpectedException(typeof(NotSupportedException))]
        public void TestReadonly()
        {
            List<int> oriList = new List<int> { 1, 2, 3 };
            IList<int> readonlyWrapper = oriList.AsReadOnly();
            readonlyWrapper[0] = 1;
        }

        // ------------------------------------------------------------- //
        #region [ test affection by override equals ]

        private class ObjDefaultEqual
        {
            protected int m_id;
            public ObjDefaultEqual(int id) { m_id = id; }
            public int Id { get { return m_id; } }
        }

        private class ObjOverrideEqual : ObjDefaultEqual
        {
            public ObjOverrideEqual(int id) : base(id) { }

            public override bool Equals(object obj)
            {
                ObjOverrideEqual otherOverrides = obj as ObjOverrideEqual;
                return m_id == otherOverrides.m_id;
            }

            public override int GetHashCode() { return m_id; }
        }

        private static void CheckRemove<Titem>(Func<int, Titem> factory, int expectedCount) where Titem : ObjDefaultEqual
        {
            int id = new Random().Next();

            Titem oriItem = factory(id);
            IList<Titem> alist = new List<Titem> { oriItem };

            Titem argumentItem = factory(oriItem.Id);
            Assert.AreNotSame(oriItem, argumentItem);

            alist.Remove(argumentItem);
            Assert.AreEqual(expectedCount, alist.Count);
        }

        /// <summary>
        /// this test shows that when removing items in a list, list determined which one to be removed
        /// by using "This method determines equality using the default equality comparer EqualityComparer(T).Default for T"
        /// and this default comparer:
        /// "The Default property checks whether type T implements the System.IEquatable(T) interface and, if so, 
        /// returns an EqualityComparer(T) that uses that implementation. Otherwise, 
        /// it returns an EqualityComparer(T) that uses the overrides of Object.Equals and Object.GetHashCode provided by T"
        /// </summary>
        [Test]
        public void TestRemoveByOverrideEqual()
        {
            CheckRemove<ObjOverrideEqual>((id) => { return new ObjOverrideEqual(id); }, 0);
        }

        /// <summary>
        /// in this testcase, because the element in the list doesn't overide Equals
        /// so although the element in the list and the element used as the argument has the same content
        /// but the list cannot find qualified element to be removed, so nothing is removed from the list
        /// and the count of the list remains unchanged
        /// </summary>
        [Test]
        public void TestRemoveByDefaultEqual()
        {
            CheckRemove<ObjDefaultEqual>((id) => { return new ObjDefaultEqual(id); }, 1);
        }

        #endregion

        // ------------------------------------------------------------- //
        [Test]
        public void TestFindIndex()
        {
            List<ObjDefaultEqual> alist = new List<ObjDefaultEqual> 
            {
                new ObjDefaultEqual(1),
                new ObjDefaultEqual(2),
                new ObjDefaultEqual(3),
                new ObjDefaultEqual(100)
            };
            Assert.IsTrue(alist.IndexOf(new ObjDefaultEqual(1)) < 0);

            Assert.AreEqual(1, alist.FindIndex(item => item.Id == 2));
            Assert.AreEqual(3, alist.FindIndex(item => item.Id > 10));
        }

        [Test]
        public void TestRemove()
        {
            List<ObjOverrideEqual> alist = new List<ObjOverrideEqual> 
            {
                new ObjOverrideEqual(1),
                new ObjOverrideEqual(2)
            };
            alist.Remove(new ObjOverrideEqual(1));
            CollectionAssert.AreEqual(new[] { new ObjOverrideEqual(2) }, alist);

            alist.RemoveAt(0);
            Assert.AreEqual(0, alist.Count);
        }

        /// <summary>
        /// below test shows that although Array can be casted into IList
        /// but its function remains the same, such as, its size still cannot be changed
        /// </summary>
        [Test]
        public void TestArray2List()
        {
            IList<int> list = new int[] { 1, 2, 3 };
            Assert.Throws<NotSupportedException>(() => list.Add(4));
        }
    }
}
