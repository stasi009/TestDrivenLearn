
using System;
using NUnit.Framework;

namespace CSharpBasicTest
{
    [TestFixture]
    public sealed class ExceptionTest
    {
        private bool m_isFinallyInvoked;

        [SetUp]
        public void Setup()
        {
            m_isFinallyInvoked = false;
        }

        private int SetFinallyInvoked()
        {
            try
            {
                m_isFinallyInvoked = false;
                return 0;
            }
            finally
            {
                m_isFinallyInvoked = true;
            }
        }

        [Test]
        public void TestFinally()
        {
            int returnValue = SetFinallyInvoked();
            Assert.IsTrue(m_isFinallyInvoked);
        }

        [Test]
        [ExpectedException(typeof(InvalidCastException))]
        public void TestInvalidCastException()
        {
            object intValue = 5;
            string strValue = (string)intValue;
        }
    }
}