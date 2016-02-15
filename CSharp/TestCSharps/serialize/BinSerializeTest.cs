
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using NUnit.Framework;

namespace CSharpBasicTest.Serialize
{
    [Serializable]
    sealed class ObjWithSerializeAttribute
    {
        public int m_intNum;
        public float m_floatNum;
        public double m_doubleNum;
        public string m_strNum;
        public IList<int> m_listNumbers;// we can see here "binary serializer" support generic types

        private static Random m_rand = new Random();

        public ObjWithSerializeAttribute()
        {
            m_intNum = m_rand.Next();
            m_floatNum = (float)(m_rand.NextDouble() * 20);
            m_doubleNum = m_rand.NextDouble() * 100;
            m_strNum = ProduceRandomString(20);

            m_listNumbers = new List<int>();
            for (int index = 0; index < 30; ++index)
            {
                m_listNumbers.Add(m_rand.Next());
            }
        }

        private string ProduceRandomString(int size)
        {
            char[] charArray = new char[size];
            Random random = new Random();
            for (int i = 0; i < size; i++)
            {
                charArray[i] = Convert.ToChar(Convert.ToInt32(Math.Floor(26 * random.NextDouble() + 65)));
            }
            return new string(charArray);
        }
    }

    [Serializable]
    sealed class Person
    {
        private string m_name;
        private int m_age;

        public Person(string name, int age)
        {
            m_name = name;
            m_age = age;
        }

        public override bool Equals(object other)
        {
            if (other == null)
                return false;

            if (ReferenceEquals(this, other))
                return true;

            if (this.GetType() != other.GetType())
                return false;

            Person otherPerson = (Person)other;
            return (m_name.Equals(otherPerson.m_name) && m_age == otherPerson.m_age);
        }

        public override int GetHashCode()
        {
            string strid = string.Format("{0}-{1}", m_name, m_age);
            return strid.GetHashCode();
        }
    }

    /// <summary>
    /// must both add "Serializable" attribute and implement "ISerializable"
    /// only implement "ISerializable" without "Serializable" attribute will throw a runtiime exception
    /// </summary>
    [Serializable]
    sealed class Team : ISerializable
    {
        private string m_name;
        private IList<Person> m_players;

        private bool m_isGetObjDataInvoked = false;
        private bool m_isSerialConstructorInvoked = false;

        public Team(string name)
        {
            m_name = name;
            m_players = new List<Person>();
        }

        public Team(SerializationInfo si, StreamingContext sc)
        {
            m_name = si.GetString("name");
            m_players = (IList<Person>)si.GetValue("players", typeof(IList<Person>));
            m_isSerialConstructorInvoked = true;
        }

        public bool IsGetObjDataInvoked
        {
            get { return m_isGetObjDataInvoked; }
        }

        public bool IsSerialConstructorInvoked
        {
            get { return m_isSerialConstructorInvoked; }
        }

        public void AddPlayer(Person player)
        {
            m_players.Add(player);
        }

        public void GetObjectData(SerializationInfo si, StreamingContext sc)
        {
            si.AddValue("name", m_name);
            si.AddValue("players", m_players);
            m_isGetObjDataInvoked = true;
        }

        private bool ArePlayersEqual(IList<Person> players1, IList<Person> players2)
        {
            int count1 = players1.Count;
            if (count1 != players2.Count)
                return false;

            for (int index = 0; index < count1; ++index)
            {
                if (!players1[index].Equals(players2[index]))
                    return false;
            }

            return true;
        }

        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;

            if (ReferenceEquals(this, obj))
                return true;

            if (this.GetType() != obj.GetType())
                return false;

            Team otherteam = (Team)obj;
            return (m_name == otherteam.m_name && ArePlayersEqual(m_players, otherteam.m_players));
        }

        public override int GetHashCode()
        {
            return m_name.GetHashCode();
        }
    }

    [TestFixture]
    public sealed class BinSerializeTest
    {
        [Test]
        public void TestUsingAttribute()
        {
            //----------------------------------- serialize
            ObjWithSerializeAttribute srcobj = new ObjWithSerializeAttribute();
            MemoryStream memstream = new MemoryStream();
            IFormatter serializer = new BinaryFormatter();
            serializer.Serialize(memstream, srcobj);

            //----------------------------------- deserialize
            memstream.Position = 0;
            ObjWithSerializeAttribute cpyobj = (ObjWithSerializeAttribute)serializer.Deserialize(memstream);

            //----------------------------------- assert equal
            Assert.AreNotSame(srcobj, cpyobj);

            Assert.AreEqual(srcobj.m_intNum, cpyobj.m_intNum);

            //!!!!!!!!!!!!!!!!!!!!!!!!!! PAY ATTENTION THAT:
            //!!!!!!!!!!!!!!!!!!!!!!!!!! after serialization and deserialization, two float number, two double number
            //!!!!!!!!!!!!!!!!!!!!!!!!!! can even check their equality with "==", not checking their difference within a small range
            Assert.AreEqual(srcobj.m_floatNum, cpyobj.m_floatNum);
            Assert.IsTrue(srcobj.m_floatNum == cpyobj.m_floatNum);

            Assert.AreEqual(srcobj.m_doubleNum, cpyobj.m_doubleNum);
            Assert.IsTrue(srcobj.m_floatNum == cpyobj.m_floatNum);

            Assert.AreEqual(srcobj.m_strNum, cpyobj.m_strNum);

            CollectionAssert.AreEqual(srcobj.m_listNumbers, cpyobj.m_listNumbers);
        }

        [Test]
        public void TestUsingInterface()
        {
            //---------------------------------- prepare team data
            Team srcteam = new Team("test");
            for (int index = 1; index <= 10; ++index)
            {
                srcteam.AddPlayer(new Person(string.Format("player{0}", index), index));
            }
            Assert.IsFalse(srcteam.IsSerialConstructorInvoked);
            Assert.IsFalse(srcteam.IsGetObjDataInvoked);

            //---------------------------------- serialize
            MemoryStream memstream = new MemoryStream();
            IFormatter serializer = new BinaryFormatter();
            serializer.Serialize(memstream, srcteam);
            Assert.IsTrue(srcteam.IsGetObjDataInvoked);

            //---------------------------------- deserialze
            memstream.Position = 0;
            Team cpyteam = (Team)serializer.Deserialize(memstream);
            Assert.IsTrue(cpyteam.IsSerialConstructorInvoked);

            //---------------------------------- check
            Assert.AreNotSame(srcteam, cpyteam);
            Assert.AreEqual(srcteam, cpyteam);
        }
    }
}