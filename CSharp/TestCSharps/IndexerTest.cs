
using System.Collections.Generic;

using NUnit.Framework;

namespace CSharpBasicTest
{
    sealed class SingleDimIndexer
    {
        #region "Method to be Tested"

        private readonly int[] m_arrays = new int[10];
        private readonly IDictionary<string, int> m_map = new Dictionary<string, int>();

        public int this[int index]
        {
            get { return m_arrays[index]; }
            set { m_arrays[index] = value; }
        }

        public int this[string key]
        {
            get { return m_map[key]; }
            set { m_map[key] = value; }
        }

        #endregion
    }

    /// <summary>
    /// chekanote: just for demonstration, use an index to work as a calculator is never a good idea
    /// </summary>
    sealed class MultiDimIndexer
    {
        public int this[int x,int y]
        {
            get { return x*y;}
        }

        public string this[int x,string content]
        {
            get { return string.Format("{0}-{1}",x,content); }
        }
    }

    [TestFixture]
    sealed class IndexerTest
    {
        #region "Test Method"

        /// <summary>
        /// indexer with an integer as the index
        /// </summary>
        [Test]
        public void TestSingleIndex()
        {
            const int Index = 5;
            const int Value = 100;

            SingleDimIndexer container = new SingleDimIndexer();

            // --------------- set
            container[Index] = Value;

            // --------------- get
            Assert.AreEqual(Value, container[Index]);
        }

        [Test]
        public void TestSingleKey()
        {
            const string Key = "cheka";
            const int Value = 1000;

            SingleDimIndexer container = new SingleDimIndexer();

            // --------------- set
            container[Key] = Value;

            // --------------- get
            Assert.AreEqual(Value, container[Key]);
        }

        [Test]
        public void TestMultiIntIndex()
        {
            MultiDimIndexer calculator = new MultiDimIndexer();
            Assert.AreEqual(10,calculator[2,5]);
            Assert.AreEqual(10,calculator[5,2]);
        }

        [Test]
        public void TestMixedIndexer()
        {
            MultiDimIndexer calculator = new MultiDimIndexer();

            Assert.AreEqual("1-cheka",calculator[1,"cheka"]);
        }

        #endregion
    }
}
