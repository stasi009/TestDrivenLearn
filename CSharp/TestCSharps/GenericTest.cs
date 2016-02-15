
using System;

using NUnit.Framework;

namespace CSharpBasicTest
{
    [TestFixture]
    class GenericTest
    {
        /// <remark>
        /// 1. if we want to use a specific member method of a Generic Type
        /// we must add Type Constraint by using "where" keyword
        /// otherwise it cannot pass the compilation
        /// 2. the generic method can be a instance method
        /// no constraint that it must be a static method
        /// </remark>
        T Max<T> (T a, T b) where T: IComparable<T>
        {
            return (a.CompareTo(b) > 0) ? a : b;
        }

        /// <summary>
        /// there is no need to supply the type parameters to a generic method
        /// because that the complier can implicitly infer the type from the given parameter
        /// </summary>
        [Test]
        public void TestImplicitCast()
        {
            Assert.AreEqual(6,Max(6,-1));
            Assert.AreEqual("yield",Max("abc","yield"));
        }
    }
}