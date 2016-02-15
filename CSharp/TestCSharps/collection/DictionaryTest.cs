using System;
using System.Collections.Generic;
using NUnit.Framework;

namespace CSharpBasicTest.collection
{
    [TestFixture]
    sealed class DictionaryTest
    {
        private Dictionary<string, int> m_dictionary;

        [SetUp]
        public void Setup()
        {
            m_dictionary = new Dictionary<string, int>();
            m_dictionary["cheka"] = 1;
            m_dictionary.Add("KGB", 2);
            m_dictionary["CIA"] = 3;

            // re-assign the value
            m_dictionary.Add("MSS", 4);
            m_dictionary["MSS"] = 88;
        }

        /// <summary>
        /// the IEnumerable returned by internal collection is read-only
        /// read-only for both keys and values
        /// </summary>
        [Test]
        [ExpectedException(typeof(InvalidOperationException))]
        public void TestErrorChangeWhileEnumerate()
        {
            foreach (KeyValuePair<string, int> kv in m_dictionary)
                m_dictionary[kv.Key] = kv.Value + 1;
        }

        /// <summary>
        /// just following the above testcase, show how to modify the value when enumerating a dictionary by key
        /// when being enumerated, Iterator's Current is always read-only
        /// so we have to enumerate another collection in order to change the original one
        /// </summary>
        [Test]
        public void TestCorrectChangeWhileEnumerate()
        {
            foreach (string key in new List<string>(m_dictionary.Keys))
            {
                m_dictionary[key] += 1;
            }

            IDictionary<string, int> expected = new Dictionary<string, int> 
            {
                {"cheka",2},
                {"KGB",3},
                {"CIA",4},
                {"MSS",89}
            };
            CollectionAssert.AreEqual(expected, m_dictionary);
        }

        [Test]
        public void TestFetching()
        {
            Assert.AreEqual(4, m_dictionary.Count);
            Assert.AreEqual(88, m_dictionary["MSS"]);

            Assert.IsTrue(m_dictionary.ContainsKey("CIA"));
            Assert.IsFalse(m_dictionary.ContainsKey("MI5"));

            int val;
            bool isfound = m_dictionary.TryGetValue("cheka", out val);
            Assert.IsTrue(isfound);
            Assert.AreEqual(1, val);
        }

        [Test]
        public void TestEnumeration()
        {
            string[] keys = new string[m_dictionary.Count];
            int[] values = new int[m_dictionary.Count];

            int position = 0;
            foreach (KeyValuePair<string, int> kv in m_dictionary)
            {
                keys[position] = kv.Key;
                values[position] = kv.Value;
                ++position;
            }

            CollectionAssert.AreEquivalent(keys, m_dictionary.Keys);
            CollectionAssert.AreEquivalent(values, m_dictionary.Values);
        }

        /// <summary>
        /// check that an exception will be thrown when a key already exists
        /// in the dictionary
        /// </summary>
        [Test]
        [ExpectedException(typeof(ArgumentException))]
        public void TestSameKeyException()
        {
            m_dictionary.Add("cheka", 2);
        }

        /// <summary>
        /// check that an exception will be thrown when a key is not found in the dictionary
        /// when fetching values
        /// </summary>
        [Test]
        [ExpectedException(typeof(KeyNotFoundException))]
        public void TestKeyNotFoundException()
        {
            int value = m_dictionary["NotExist"];
        }

        [Test]
        public void TestInitializer()
        {
            IDictionary<int, string> dict = new Dictionary<int, string> 
            {
                {1,"cheka"},
                {2,"stasi"}
            };
            Assert.AreEqual("cheka", dict[1]);
            Assert.AreEqual("stasi", dict[2]);
        }

        /// <summary>
        /// when constructing the dictonary, you can pass an argument
        /// which specifies how to compare two keys
        /// </summary>
        [Test]
        public void TestKeyEqualComparer()
        {
            IDictionary<string, int> dict = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase)
                                                   {
                                                       {"cheka",1},
                                                       {"stasi",9}
                                                   };

            // since case non-sensitive, so it is recognized as the same key
            Assert.Throws<ArgumentException>(() => dict.Add("CHEKA", 3));
            Assert.AreEqual(9, dict["STASI"]);
        }

        [Test]
        public void TestCompareKeyValuePair()
        {
            KeyValuePair<int,string> kv1 = new KeyValuePair<int, string>(1,"stasi");
            KeyValuePair<int, string> kv2 = new KeyValuePair<int, string>(1, "stasi");

            Assert.AreNotSame(kv1,kv2);
            Assert.AreEqual(kv1,kv2);
        }
    }
}
