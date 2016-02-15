
using System;

using NUnit.Framework;

namespace CSharpBasicTest
{
    [TestFixture]
    sealed class MultiLevelHeritTest
    {
        // ------------------------------------------------------------- //
        #region [ helper class and method ]

        enum Invoker
        {
            Grandpa,
            Father,
            Son
        }

        sealed class TestParm
        {
            public Invoker WhoInvoked { get; set; }
        }

        class Grandpa
        {
            protected TestParm m_parm = new TestParm();

            public Invoker WhoInvoked { get { return m_parm.WhoInvoked; } }

            public void TemplateMethod() { Foo(m_parm); }

            public virtual void Foo(TestParm parm) { parm.WhoInvoked = Invoker.Grandpa; }
        }

        class Father : Grandpa
        {
            public override void Foo(TestParm parm) { parm.WhoInvoked = Invoker.Father; }
        }

        class Son : Father
        {
            public override void Foo(TestParm parm) { parm.WhoInvoked = Invoker.Son; }
        }

        #endregion

        // ------------------------------------------------------------- //
        #region [ test ]

        /// <summary>
        /// chekanote: this test shows that the "override" method in father class
        /// is automatically declared as virtual, and can be "override" by son class
        /// </summary>
        [Test]
        public void TestOverrideAutoVirtual()
        {
            Son son = new Son();
            son.TemplateMethod();
            Assert.AreEqual(Invoker.Son,son.WhoInvoked);

            Father fatherRefernce = new Son();
            fatherRefernce.TemplateMethod();
            Assert.AreEqual(Invoker.Son,son.WhoInvoked);
        }

        #endregion
    }
}