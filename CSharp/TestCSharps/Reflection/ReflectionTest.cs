
using System;
using System.Reflection;
using System.Collections.Generic;

using NUnit.Framework;

namespace CSharpBasicTest
{
    sealed class Student
    {
        private int m_id;
        private string m_name;
        private float m_score;

        public Student() { }

        public Student(int id,string name,float score)
        {
            m_id = id;
            m_name = name;
            m_score = score;
        }

        public int ID { get { return m_id; } set { m_id = value; } }
        public string Name { get { return m_name; } set { m_name = value; } }
        public float Score { get { return m_score; } set { m_score = value; } }
    }

    [TestFixture]
    public sealed class DynPropertyTest
    {
        private Type m_studType;
        private PropertyInfo m_idProperty;
        private PropertyInfo m_nameProperty;
        private PropertyInfo m_scoreProperty;

        [SetUp]
        public void Setup()
        {
            m_studType = typeof(Student);
            m_idProperty = m_studType.GetProperty("ID");
            m_nameProperty = m_studType.GetProperty("Name");
            m_scoreProperty = m_studType.GetProperty("Score");
        }

        [Test]
        public void TestParameterConstructor()
        {
            // to use this method to create an instance, the class must have a non-parameter constructor
            Student studobj = (Student)Activator.CreateInstance(m_studType);

            int id = 9;
            string name = "cheka";
            float score = 88.88f;
            studobj = (Student)Activator.CreateInstance(m_studType,id,name,score);

            Assert.AreEqual(id,studobj.ID);
            Assert.AreEqual(name,studobj.Name);
            Assert.AreEqual(score,studobj.Score,1e-6);
        }

        /// <summary>
        /// Returns all the public properties of the current Type.
        /// not include the properties in its base class, only current type
        /// </summary>
        [Test]
        public void TestGetProperties()
        {
            PropertyInfo[] properties = m_studType.GetProperties();
            Assert.AreEqual(3,properties.Length);

            PropertyInfo[] expectedProp = new PropertyInfo[] { m_idProperty, m_nameProperty, m_scoreProperty};
            CollectionAssert.AreEquivalent(expectedProp,properties);
        }

        [Test]
        public void TestPropertyType()
        {
            Type intType = typeof(int);
            Type stringType = typeof(string);
            Type floatType = typeof(float);

            Assert.AreEqual(intType,m_idProperty.PropertyType);
            Assert.IsTrue(intType == m_idProperty.PropertyType);

            Assert.AreEqual(stringType,m_nameProperty.PropertyType) ;
            Assert.IsTrue(stringType == m_nameProperty.PropertyType);

            Assert.AreEqual(floatType,m_scoreProperty.PropertyType);
            Assert.IsTrue(floatType == m_scoreProperty.PropertyType);
        }

        [Test]
        public void TestPropertyName()
        {
            Assert.AreEqual("ID",m_idProperty.Name);
            Assert.AreEqual("Name",m_nameProperty.Name);
            Assert.AreEqual("Score",m_scoreProperty.Name);
        }

        [Test]
        public void TestSetGetValue()
        {
            int id = 9;
            string name = "cheka";
            float score = 99.9f;

            object obj = Activator.CreateInstance(m_studType);
            m_idProperty.SetValue(obj,id,null);
            m_nameProperty.SetValue(obj,name,null);
            m_scoreProperty.SetValue(obj,score,null);

            // no need to do explicit cast, for "object.Equals" is totally dynamic binding
            Assert.AreEqual(id,m_idProperty.GetValue(obj,null));
            Assert.AreEqual(name,m_nameProperty.GetValue(obj,null));
            Assert.AreEqual(score,(float)m_scoreProperty.GetValue(obj,null),1e-6);
        }

        [Test]
        public void TestGetValueType()
        {
            Student stud = new Student(1,"cheka",88.8f);

            object obj = m_idProperty.GetValue(stud,null);
            Assert.IsTrue(obj is int);

            obj = m_nameProperty.GetValue(stud,null);
            Assert.IsTrue(obj is string);

            obj = m_scoreProperty.GetValue(stud,null);
            Assert.IsTrue(obj is float);
        }

        [Test]
        [ExpectedException(typeof(ArgumentException))]
        public void TestException()
        {
            object obj = Activator.CreateInstance(m_studType);
            m_idProperty.SetValue(obj,"bad argument",null);
        }

        [Test]
        public void TestClassInformation()
        {
            Assert.AreEqual("Student",m_studType.Name);
            Assert.AreEqual("CSharpBasicTest",m_studType.Namespace);
        }
    }

    [TestFixture]
    public sealed class GenericTypeTest
    {
        [Test]
        public void TestGenericArguments()
        {
            IList<string> strlist = new List<string>();

            Type type = strlist.GetType();
            Assert.IsTrue(type.IsGenericType);

            Assert.AreEqual(typeof(string),type.GetGenericArguments()[0]);
        }
    }
}