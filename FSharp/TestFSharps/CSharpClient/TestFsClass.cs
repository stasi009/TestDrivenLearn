using System;
using NUnit.Framework;

using TestFSharps;

namespace CSharpClient
{
    [TestFixture]
    sealed class TestFsClass
    {
        /// <summary>
        /// from this test, we can see that 
        /// no matter how that F# method accepts its arguments
        /// whether they are seperate arguments, or wrapped in a tuple
        /// when invoked by C# codes, we always pass the arguments in ()
        /// (also no need to explicitly create a C# tuple and pass in)
        /// </summary>
        [Test]
        public void TestCallMethods()
        {
            TestClassInteroperate src = new TestClassInteroperate();
            Assert.AreEqual(3, src.Add(1, 2));
            Assert.AreEqual(300,src.AddwithTuple(100,200));
        }
    }
}
