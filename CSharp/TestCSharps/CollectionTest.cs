
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Collections.ObjectModel;

using NUnit.Framework;

/// <remark>
/// 1.  IEnumerable and IEnumerable<T>: Provides minimum functionality (enumeration only)
/// 
/// 2.  ICollection and ICollection<T>: Provides medium functionality (e.g., the Count property)
///     Pay attention that 'ICollection<T>' doesn't derive from 'ICollection'
/// 
/// 3.  IList / IDictionary and their generic counterparts: Provides maximum functionality (including "random" access by index/key)
/// </remark>
namespace CSharpBasicTest
{
    [TestFixture]
    sealed class EnumerableTest
    {
        [Test]
        public void TestArray()
        {
            int[] arrInt = new int[3];

            // test whether array implements the "IEnumerable" interface
            Assert.IsInstanceOf(typeof(IEnumerable<int>), arrInt);
            // IEnumberable<T> also derives from IEnumerable
            Assert.IsInstanceOf(typeof(IEnumerable), arrInt);

            // test whether array implements the "ICollection" interface
            Assert.IsInstanceOf(typeof(ICollection<int>), arrInt);

            /// <remark>
            /// C# arrays also implement both the generic and nongeneric IList s 
            /// (although the methods that add or remove elements are hidden via explicit 
            /// interface implementation and throw a NotSupportedException if called)
            /// </remark>
            // test whether array implements the "IList" interface
            Assert.IsInstanceOf(typeof(IList<int>), arrInt);
            Assert.IsInstanceOf(typeof(IList), arrInt);
        }

        class MyList : IEnumerable<string>
        {
            private List<string> m_list = new List<string>();

            public string this[int index] { get { return m_list[index]; } }
            public void Add(string element) { m_list.Add(element); }

            // we must override both the nongeneric and generic version
            // but hide the nongeneric version
            public IEnumerator<string> GetEnumerator() { return m_list.GetEnumerator(); }
            IEnumerator IEnumerable.GetEnumerator() { return GetEnumerator(); }
        }

        [Test]
        public void TestCustomEnumerable()
        {
            MyList strlist = new MyList();
            strlist.Add("cheka");
            strlist.Add("hello");
            strlist.Add("hello from WSU");

            string[] strArrays = new string[3];
            int position = 0;
            foreach (string element in strlist)
            {
                strArrays[position] = element;
                ++position;
            }

            string[] expected = { strlist[0], strlist[1], strlist[2] };
            CollectionAssert.AreEqual(expected, strArrays);
        }

        class YieldTestObj
        {
            private int m1 = 1;
            private int m2 = 2;
            private int m3 = 3;

            public IEnumerable<int> EachValue
            {
                get
                {
                    yield return m1;
                    yield return m2;
                    yield return m3;
                }
            }
        }

        /// <summary>
        /// test implementing IEnumerable by using "yield"
        /// </summary>
        [Test]
        public void TestEnumerableByYield()
        {
            YieldTestObj instance = new YieldTestObj();

            int[] gotValues = new int[3];
            int position = 0;
            foreach (int value in instance.EachValue)
            {
                gotValues[position] = value;
                ++position;
            }

            CollectionAssert.AreEqual(new int[] { 1, 2, 3 }, gotValues);
        }
    }

    [TestFixture]
    sealed class CustomCollectionTest
    {
        class IdObjBase
        {
            public int Id { get; set; }
        }

        class Student : IdObjBase { }
        class Equipment : IdObjBase { }

        class IdObjectCollection<T> : Collection<T> where T : IdObjBase
        {
            protected override void InsertItem(int index, T item)
            {
                base.InsertItem(index, item);
                item.Id = index;
            }
        }

        private void CheckId<T>(IdObjectCollection<T> idobjCollection, int number) where T : IdObjBase, new()
        {
            for (int index = 0; index < number; ++index)
            {
                T item = new T();
                idobjCollection.Add(item);
                Assert.AreEqual(index, item.Id);
            }
        }

        [Test]
        public void TestIdGeneration()
        {
            IdObjectCollection<Student> studCollection = new IdObjectCollection<Student>();
            CheckId(studCollection, 10);

            IdObjectCollection<Equipment> equipCollection = new IdObjectCollection<Equipment>();
            CheckId(equipCollection, 10);
        }
    }

    /// <summary>
    /// yield return can be used in both case: a method returning IEnumerable<T> and a method returning IEnumerator<T>
    /// both case are legal, and the compiler will generate some anonymous class to do the rest of work for us
    /// (maybe the only requirement to return IEnumerator<T> is in the case that write user-defined collection)
    /// </summary>
    [TestFixture]
    sealed class YieldTest
    {
        /// <summary>
        /// "yield" is not allowed in some cases, like in anonymous methods
        /// </summary>
        private static IEnumerable<int> YieldReturn(int total)
        {
            for (int index = 0; index < total; ++index)
                yield return index;
        }

        [Test]
        public void TestYieldReturn()
        {
            int total = 100;
            CollectionAssert.AreEqual(Enumerable.Range(0, total), YieldReturn(total));
        }

        private static IEnumerable<string> MultipleYield()
        {
            yield return "cheka";
            yield return "stasi";
            yield return "mss";
        }

        [Test]
        public void TestMultipleYield()
        {
            CollectionAssert.AreEqual(new[] { "cheka", "stasi", "mss" }, MultipleYield());
        }

        private static IEnumerable<string> YieldBreak(bool isEarlyBreak)
        {
            yield return "cheka";
            yield return "mss";

            if (isEarlyBreak)
                yield break;

            yield return "stasi";
        }

        [Test]
        public void TestYieldBreak()
        {
            CollectionAssert.AreEqual(new[] { "cheka", "mss", "stasi" }, YieldBreak(false));
            CollectionAssert.AreEqual(new[] { "cheka", "mss" }, YieldBreak(true));
        }

        // =========================================== //
        #region [ implement IEnumerable by yield ]

        sealed class MyEnumerable : IEnumerable<int>
        {
            public IEnumerator<int> GetEnumerator()
            {
                yield return 1;
                yield return 2;
                yield return 3;
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return this.GetEnumerator();
            }
        }

        [Test]
        public void TestUserdefEnumerableByYield()
        {
            MyEnumerable em = new MyEnumerable();
            CollectionAssert.AreEqual(new[] { 1, 2, 3 }, em);
        }

        #endregion
    }

    [TestFixture]
    sealed class EnumeratorTest
    {
        [Test]
        public void TestEnumerate()
        {
            IList<int> intList = new List<int> { 3, 78, 95 };
            IEnumerator<int> enumerator = intList.GetEnumerator();

            // --------- when there are elements available
            // --------- MoveNext returns true and Current returns valid data
            Assert.IsTrue(enumerator.MoveNext());
            Assert.AreEqual(3, enumerator.Current);

            Assert.IsTrue(enumerator.MoveNext());
            Assert.AreEqual(78, enumerator.Current);

            Assert.IsTrue(enumerator.MoveNext());
            Assert.AreEqual(95, enumerator.Current);

            // --------- when no data is available any more
            // --------- MoveNext returns false 
            // !!!!!!!!! More important,when called after MoveNext returns false
            // !!!!!!!!! Current will return a undefined value, other than throwing exception
            Assert.IsFalse(enumerator.MoveNext());
            int undefined = enumerator.Current;
            Assert.AreEqual(0, undefined);
        }
    }
}