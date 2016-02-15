using System;
using NUnit.Framework;

namespace CSharpBasicTest.Advance
{
    [TestFixture]
    sealed class TestFuncFeature
    {
        /// <summary>
        /// this test demonstrate that the captured variables in the closure
        /// are evaluated during the execution, other than during definition
        /// (it is like the F# case that capture a reference cell, other than
        /// capturing a 'let'-defined variable)
        /// </summary>
        [Test]
        public void TestDeferExeFeature()
        {
            string target = "cheka";
            Func<string> func = () => string.Format("hello,{0}", target);

            target = "stasi";
            Assert.AreEqual("hello,stasi", func());
        }
    }// class
}
