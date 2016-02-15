
using System;
using System.Collections.Generic;
using NUnit.Framework;

namespace CSharpBasicTest
{
    [TestFixture]
    public sealed class OtherTest
    {
        private sealed class Fool1
        {
            public int Id { get; set; }
            public string Name { get; set; }
        }

        private sealed  class Fool2
        {
            private Fool1 m_fool1;
            private int[] m_ids;

            public Fool1 Fool1 { get { return m_fool1; } }
            public int[] Ids { get { return m_ids; } }
        }

        [Test]
        public void TestCoalescingOperator()
        {
            int? x = null;
            Assert.AreEqual(5, x ?? 5);

            // -------------- first non-null value
            int? a = null, b = 1, c = 4;
            Assert.AreEqual(4, a ?? c ?? b);
        }

        [Test]
        public void TestEnvironment()
        {
            int procCount = Environment.ProcessorCount;
            Assert.AreEqual(8, procCount);
        }

        /// <summary>
        /// demonstrate different ways to initialize a collection with "initializer" syntax
        /// </summary>
        [Test]
        public void TestCollectionInitializer()
        {
            IList<Fool1> testobjlist = new List<Fool1> { new Fool1 { Id = 6, Name = "henry" } };
            Assert.AreEqual(1, testobjlist.Count);

            IDictionary<int, Fool1> testDict = new Dictionary<int, Fool1>
            {
                {1,new Fool1{Id = 1,Name = "cheka"}}
            };
            Assert.AreEqual(1, testDict.Count);
        }

        [Test]
        public void TestMemberDefaultValue()
        {
            Fool2 footballteam = new Fool2();
            Assert.IsNull(footballteam.Fool1);
            Assert.IsNull(footballteam.Ids);
        }

        // ============================================================= //
        #region [ readonly test ]
        class SimpleClass
        {
            public readonly int[] m_array = new int[1];
        };

        /// <summary>
        /// this test shows that when reference type is declared with "readonly" keyword
        /// only the reference itself cannot be changed (such as being assigned once again)
        /// but the content of that reference type can still be changed, still not safe
        /// </summary>
        [Test]
        public void TestReadonly()
        {
            SimpleClass instance = new SimpleClass();

            int value = 100;
            instance.m_array[0] = value;

            Assert.AreEqual(value, instance.m_array[0]);
        }
        #endregion
    }
}