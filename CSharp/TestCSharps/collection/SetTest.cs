using System;
using System.Collections.Generic;

using NUnit.Framework;

namespace CSharpBasicTest.collection
{
    [TestFixture]
    sealed class SetTest
    {
        [Test]
        public void TestAdd()
        {
            HashSet<int> set1 = new HashSet<int>(new int[]{1,2,3});
            CollectionAssert.AreEqual(new int[]{1,2,3},set1);

            // ----------------- add duplicates
            // different from IDictionary, adding duplicate items into set
            // will not throw any exception, but will return false
            bool success = set1.Add(1);
            Assert.IsFalse(success);

            // ----------------- add non-existing
            success = set1.Add(88);
            Assert.IsTrue(success);

        } // TestAdd
    }// SetTest
}
