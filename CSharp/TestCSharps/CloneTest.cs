
using System;
using System.Collections;

using NUnit.Framework;

namespace CSharpBasicTest
{
    [TestFixture]
    sealed class ShallowCloneTest
    {
        [Test]
        public void TestArrayList()
        {
            ArrayList oriList = new ArrayList { "cheka", "KGB", "Stasi" };
            int oriCount = oriList.Count;
            ArrayList cpyList = (ArrayList)oriList.Clone();

            Assert.AreNotSame(cpyList, oriList);
            Assert.AreEqual(oriCount, cpyList.Count);

            // just a shallow copy for each element
            for (int index = 0; index < oriList.Count; ++index)
            {
                Assert.AreSame(cpyList[index], oriList[index]);
            }

            // but the two list it self pointing to different place
            // so remove or add one will not affect the other
            cpyList.RemoveAt(cpyList.Count - 1);
            Assert.AreEqual(oriCount - 1, cpyList.Count);
            Assert.AreEqual(oriCount, oriList.Count);
        }

        // ************************************************** //
        #region [ derived class clone ]

        private class CloneBase : ICloneable
        {
            public object Clone()
            {
                return this.MemberwiseClone();
            }
        }

        private sealed class IdObject : CloneBase
        {
            private int m_id;

            public int Id { get { return m_id; } set { m_id = value; } }

            public override bool Equals(object obj)
            {
                // TODO: make it more professional later
                return m_id == ((IdObject)obj).m_id;
            }

            public override int GetHashCode()
            {
                // TODO: make it more professional later
                return base.GetHashCode();
            }
        }

        private sealed class NameObject : CloneBase
        {
            private string m_name;

            public string Name { get { return m_name; } set { m_name = value; } }

            public override bool Equals(object obj)
            {
                // TODO: make it more professional later
                return m_name.Equals(((NameObject)obj).m_name);
            }

            public override int GetHashCode()
            {
                // TODO: make it more professional later
                return base.GetHashCode();
            }
        }

        private object CheckDerivedCopy(CloneBase oriObject)
        {
            object cpyObject = oriObject.Clone();

            Assert.AreNotSame(cpyObject,oriObject);
            Assert.AreEqual(cpyObject,oriObject);

            return cpyObject;
        }

        [Test]
        public void TestDerivedCopy()
        {
            IdObject idobj1 = new IdObject { Id = 1};
            IdObject idobj2 = (IdObject)CheckDerivedCopy(idobj1);
            idobj2.Id = idobj2.Id + 1;
            Assert.AreNotEqual(idobj2,idobj1);

            NameObject nameObj1 = new NameObject { Name = "cheka"};
            NameObject nameObj2 = (NameObject)CheckDerivedCopy(nameObj1);
            nameObj2.Name = "NewName";
            Assert.AreNotEqual(nameObj2,nameObj1);
        }

        #endregion
    }
}