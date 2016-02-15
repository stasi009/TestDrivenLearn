
using System;

using NUnit.Framework;

namespace CSharpBasicTest
{
    /// <summary>
    /// create a test which overrides "Equals" for test purpose
    /// 
    /// Equality customization is much more applicable to structs than to classes. With a class, 
    /// there's no potential for improving performance because the default reference-type comparison 
    /// is already highly efficient. Instead, there is potential to confuse the consumer of the class, 
    /// who will in most cases expect reference-type equality semantics
    /// 
    /// In general, you should avoid messing with the equality semantics of classes. 
    /// For instance, if you have a Customer class with fields such as ID, FirstName, LastName, and so on, 
    /// it would be inappropriate to change the equality semantics such that two Customer instances 
    /// with the same ID or name reported as being equal
    /// </summary>
    class RefTypeEqualOverride : IEquatable<RefTypeEqualOverride>
    {
        private int m_x;

        public RefTypeEqualOverride(int x)
        {
            m_x = x;
        }

        // override the method in object
        public override bool Equals(object right)
        {
            // check null
            if (right == null)
                return false;

            if (object.ReferenceEquals(this, right))
                return true;

            RefTypeEqualOverride other = right as RefTypeEqualOverride;
            if (other == null)
            {
                return false;
            }
            else
                return Equals(other);
        }

        // override the method defined in IEquatable
        public bool Equals(RefTypeEqualOverride right)
        {
            if (right == null)
                return false;

            if (object.ReferenceEquals(this, right))
                return true;

            return m_x == right.m_x;
        }

        /// <summary>
        /// 1. It must return the same value if called repeatedly on the same object.
        /// 2. It must return the same value on two objects for which Equals returns true 
        /// (hence, GetHashCode and Equals are overridden together).
        /// 3. It must not throw exceptions
        /// </summary>
        public override int GetHashCode()
        {
            return m_x.GetHashCode();
        }

        /// <remark>
        /// !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
        /// in both version of "Equals", we test whether the parameter is null at the beginning of the method
        /// then we CAN NOT override operator "=="
        /// because when test null using "right == null", it will call the overriden "==" operator
        /// then it will again call Equals, which result in an ENDLESS LOOP
        /// !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
        /// </remark>
        /*
        public static bool operator == (RefTypeEqualOverride obj1,RefTypeEqualOverride obj2)
        {
            return obj1.Equals(obj2);
        }

        public static bool operator != (RefTypeEqualOverride obj1,RefTypeEqualOverride obj2)
        {
            return !(obj1.Equals(obj2));
        }
        */
    }

    class RefTypeNoEqualOverride
    {
        private int m_y;

        public RefTypeNoEqualOverride(int y)
        {
            m_y = y;
        }
    }


    /// <summary>
    /// test various test method in C#
    /// </summary>
    [TestFixture]
    public class EqualTest
    {
        [Test]
        public void TestValueType()
        {
            #region [check equality with operator ==]

            // operators, and so are statically resolved (in fact, they are implemented as 
            // static functions). So, when you use == or !=, C# makes a compile-time decision 
            // as to which type will perform the comparison, and no virtual behavior comes into play
            int x = 5;
            int y = 5;
            Assert.IsTrue(x == y);

            // !!! it seems that C# has not use the same boxing-cache mechanism as in Java or Python
            // !!! which is cache a specific range of numbers to save memory and runtime overhead
            // !!! when boxing value types, the CLR just allocate a totally new object for each value number
            object objx = 1;
            object objy = 1;
            Assert.IsFalse(objx == objy);

            #endregion

            #region [check equality with the instance method "Equals" of object]

            // Equals is resolved at runtime—according to the object's actual type. 
            // In this case, it calls Int32's Equals method, which applies value equality 
            // to the operands, returning true.

            // since it is resolved at runtime, hence, 
            // Equals is suitable for equating two objects in a type-agnostic fashion

            Assert.AreNotSame(objx, objy);
            Assert.IsTrue(objx.Equals(objy));

            #endregion

            #region [check equality with the static method "Equals" of object]

            // it calls the virtual Equals method on objx
            // so it still belongs to "runtime resolution"
            Assert.IsTrue(object.Equals(objx, objy));

            #endregion

            #region [check reference equality with "ReferenceEquals"]

            Assert.IsFalse(object.ReferenceEquals(objx, objy));

            #endregion

            #region [check equality in NUnit]

            // NUnit makes use of the Equals override on the expected object. 
            // If you neglect to override Equals, you can expect failures non-identical objects. 
            // In particular, overriding operator== without overriding Equals has no effect

            Assert.AreEqual(objx, objy);
            Assert.AreNotSame(objx, objy);

            #endregion
        }

        /// <summary>
        /// check equality on reference types which has not override "Equals" or operator "=="
        /// </summary>
        [Test]
        public void TestRefTypeNoOverride()
        {
            int value = 100;
            RefTypeNoEqualOverride typedObj = new RefTypeNoEqualOverride(value);
            object generalObj = typedObj;

            RefTypeNoEqualOverride typedObjSameVal = new RefTypeNoEqualOverride(value);
            object generalObjSameVal = typedObjSameVal;

            object generalObjSameRef = generalObj;

            #region [using specific type, based on base-version "Equals" of object]
            Assert.IsFalse(typedObj.Equals(typedObjSameVal));
            Assert.IsFalse(object.Equals(typedObj, typedObjSameVal));
            #endregion

            #region [check on general type, based on runtime type resolution]
            // based on reference equality
            Assert.IsTrue(generalObj.Equals(generalObjSameRef));
            Assert.IsTrue(object.Equals(generalObj, generalObjSameRef));

            Assert.IsFalse(generalObj.Equals(generalObjSameVal));
            Assert.IsFalse(object.Equals(generalObj, generalObjSameVal));
            #endregion

            #region [check under NUnit]
            Assert.AreNotSame(generalObj, generalObjSameVal);
            Assert.AreSame(generalObj, generalObjSameRef);

            Assert.AreEqual(generalObj, generalObjSameRef);
            Assert.AreNotEqual(generalObj, generalObjSameVal);// since not override "Equals"
            #endregion
        }

        /// <summary>
        /// check equality on reference types which has overriden "Equals" and operator "=="
        /// </summary>
        [Test]
        public void TestRefTypeOverride()
        {
            int number = 123;
            RefTypeEqualOverride typeobj = new RefTypeEqualOverride(number);
            object obj = typeobj;

            RefTypeEqualOverride typeSameVal = new RefTypeEqualOverride(number);
            object objSameVal = typeSameVal;

            #region [check using "Equals"]
            Assert.IsFalse(object.ReferenceEquals(obj, objSameVal));
            Assert.IsTrue(obj.Equals(objSameVal));
            Assert.IsTrue(object.Equals(obj, objSameVal));// based on the instance method "Equals"
            #endregion

            #region [check using "=="]
            // we have not overriden operator ==, then use just the orignal version defined in object
            // which check the reference equality, so the result must be false
            Assert.IsFalse(typeobj == typeSameVal);
            #endregion

            #region [check under NUnit]
            Assert.AreNotSame(obj, objSameVal);
            Assert.AreEqual(obj, objSameVal);
            #endregion
        }
    }
}