
using System;

using NUnit.Framework;

namespace CSharpBasicTest
{
    enum EnumInvokeVersion
    {
        PARENT_CALLED,
        CHILD_CALLED,
        UNDEFINED
    }

    class Parent
    {
        public virtual void Method1(Utility parm)
        {
            parm.InvokeVersion = EnumInvokeVersion.PARENT_CALLED;
        }
    }

    class Child : Parent
    {
        public override void Method1(Utility parm)
        {
            parm.InvokeVersion = EnumInvokeVersion.CHILD_CALLED;
        }
    }

    class Utility
    {
        private EnumInvokeVersion m_invokeVersion = EnumInvokeVersion.UNDEFINED;

        public EnumInvokeVersion InvokeVersion
        {
            get { return m_invokeVersion; }
            set { m_invokeVersion = value; }
        }

        public void Function1(Parent obj)
        {
            m_invokeVersion = EnumInvokeVersion.PARENT_CALLED;
        }

        public void Function1(Child obj)
        {
            m_invokeVersion = EnumInvokeVersion.CHILD_CALLED;
        }
    }

    /// <summary>
    /// test the syntax of polymorphism
    /// </summary>
    [TestFixture]
    public class PolymorphismTest
    {
        /// <summary>
        /// test the resolution order when there are overloaded functions, with parameters involve
        /// a "Parent-Child" relationship
        /// !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!! FROM THIS TESTCASE, WE CAN SEE THAT
        /// overload is static name binding, which occurs at compiling time
        /// whereas, override is dynamic, runtime name binding, which occurs at runtime
        /// </summary>
        [Test]
        public void TestOverload()
        {
            Child child1 = new Child();
            Assert.IsInstanceOf<Parent>(child1);

            Utility helper4Child = new Utility();
            // it will call the method with exact parameter type, will not cast to its base class version
            helper4Child.Function1(child1);
            Assert.AreEqual(EnumInvokeVersion.CHILD_CALLED, helper4Child.InvokeVersion);

            Utility helper4Parent = new Utility();
            // if you want to call the base version, you need explicit cast
            helper4Parent.Function1((Parent)child1);
            Assert.AreEqual(EnumInvokeVersion.PARENT_CALLED, helper4Parent.InvokeVersion);
        }

        [Test]
        public void TestVirtualOverride()
        {
            #region [parent version called]
            Utility parm1 = new Utility();
            Assert.AreEqual(EnumInvokeVersion.UNDEFINED, parm1.InvokeVersion);

            Parent parentObj = new Parent();
            parentObj.Method1(parm1);
            Assert.AreEqual(EnumInvokeVersion.PARENT_CALLED, parm1.InvokeVersion);
            #endregion

            #region [child version called]
            Utility parm2 = new Utility();

            Child childObj = new Child();
            childObj.Method1(parm2);
            Assert.AreEqual(EnumInvokeVersion.CHILD_CALLED, parm2.InvokeVersion);
            #endregion
        }

        [Test]
        public void TestIs()
        {
            object obj = new Child();
            Assert.IsTrue(obj is Parent);
            Assert.IsTrue(obj is Child);
        }

        /// <summary>
        /// test the usage of the type assertation method provided by NUnit
        /// </summary>
        [Test]
        public void TestAssignableInNUnit()
        {
            Parent parentObj = new Parent();
            Child childObj = new Child();

            Assert.IsInstanceOf<Parent>(parentObj);
            Assert.IsNotInstanceOf<Child>(parentObj);
            Assert.IsAssignableFrom<Parent>(parentObj);
            Assert.IsAssignableFrom<Child>(parentObj);

            Assert.IsInstanceOf<Child>(childObj);
            Assert.IsInstanceOf<Parent>(childObj);
            Assert.IsAssignableFrom<Child>(childObj);
            Assert.IsNotAssignableFrom<Parent>(childObj);
        }

        [Test]
        public void TestFailedCast()
        {
            var parent = new Parent();
            Assert.IsNull(parent as Child);

            // below codes will throw exception
            Assert.Throws<InvalidCastException>(() => { var failed = (Child) parent; });
        }
    }

    [TestFixture]
    public class HideTest
    {
        class BaseClass
        {
            public virtual void OverrideFunc(Utility parm) { parm.InvokeVersion = EnumInvokeVersion.PARENT_CALLED; }
            public virtual void NewFunc(Utility parm) { parm.InvokeVersion = EnumInvokeVersion.PARENT_CALLED; }
        }

        class DeriveClass : BaseClass
        {
            public override void OverrideFunc(Utility parm) { parm.InvokeVersion = EnumInvokeVersion.CHILD_CALLED; }
            public new void NewFunc(Utility parm) { parm.InvokeVersion = EnumInvokeVersion.CHILD_CALLED; }
        }

        private DeriveClass m_deriveObj;
        private BaseClass m_baseRef;
        private Utility m_parm;

        [SetUp]
        public void Setup()
        {
            m_deriveObj = new DeriveClass();
            m_baseRef = m_deriveObj;
            m_parm = new Utility();
        }

        [Test]
        public void TestOverrideByBase()
        {
            m_baseRef.OverrideFunc(m_parm);
            Assert.AreEqual(EnumInvokeVersion.CHILD_CALLED,m_parm.InvokeVersion);
        }

        [Test]
        public void TestOverrideByDerive()
        {
            m_deriveObj.OverrideFunc(m_parm);
            Assert.AreEqual(EnumInvokeVersion.CHILD_CALLED,m_parm.InvokeVersion);
        }

        [Test]
        public void TestNewByBase()
        {
            m_baseRef.NewFunc(m_parm);
            Assert.AreEqual(EnumInvokeVersion.PARENT_CALLED,m_parm.InvokeVersion);
        }

        [Test]
        public void TestNewByDerive()
        {
            m_deriveObj.NewFunc(m_parm);
            Assert.AreEqual(EnumInvokeVersion.CHILD_CALLED,m_parm.InvokeVersion);
        }
    }

    [TestFixture]
    public class CollectionPolyTest
    {
        private void Verify(Parent[] parms,EnumInvokeVersion invokeVersion) 
        { 
            Utility util = new Utility();
            foreach (Parent parm in parms)
            {
                parm.Method1(util);
                Assert.AreEqual(invokeVersion,util.InvokeVersion);
                util.InvokeVersion = EnumInvokeVersion.UNDEFINED;
            }
        }

        [Test]
        public void TestRealParent()
        {
            Parent[] realParents = new Parent[] { new Parent(), new Parent() };
            Verify(realParents,EnumInvokeVersion.PARENT_CALLED);
        }

        /// <summary>
        /// !!!!!!!!!!!!!!!!!! this method also shows that pass "Child[]" to a method
        /// !!!!!!!!!!!!!!!!!! which needs "Parent[]", can pass compilation
        /// </summary>
        [Test]
        public void TestChildren()
        {
            Child[] children = new Child[] { new Child(), new Child() };
            Verify(children,EnumInvokeVersion.CHILD_CALLED);
        }

        /// <summary>
        /// overriding is always "dynamic binding"
        /// </summary>
        [Test]
        public void TestParentRefChild()
        {
            Parent[] realParents = new Parent[] { new Child(), new Child() };
            Verify(realParents, EnumInvokeVersion.CHILD_CALLED);
        }
    }
}