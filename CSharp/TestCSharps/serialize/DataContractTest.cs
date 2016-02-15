
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.Xml;

using NUnit.Framework;

namespace CSharpBasicTest.Serialize
{
    [DataContract]
    public sealed class LocationTime
    {
        [DataMember]
        public string Location { get; set; }

        [DataMember]
        public string Time { get; set; }

        #region "override methods"

        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;

            if (object.ReferenceEquals(this, obj))
                return true;

            if (obj is LocationTime)
            {
                LocationTime other = (LocationTime)obj;
                return this.Location.Equals(other.Location) && (this.Time.Equals(other.Time));
            }
            else
                return false;
        }

        public override int GetHashCode()
        {
            return (string.Format("{0}.{1}", this.Location, this.Time)).GetHashCode();
        }

        #endregion
    }

    [DataContract]
    public sealed class Course
    {
        [DataMember]
        public int Id { get; set; }

        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public LocationTime LocationTime { get; set; }

        #region "override methods"

        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;

            if (object.ReferenceEquals(this, obj))
                return true;

            if (!(obj is Course))
                return false;

            Course otherCourse = (Course)obj;
            return (this.Id == otherCourse.Id)
                && (this.Name.Equals(otherCourse.Name))
                && (this.LocationTime.Equals(otherCourse.LocationTime));
        }

        public override int GetHashCode()
        {
            string idstring = string.Format("{0}.{1}", this.Name, this.Id.ToString());
            return idstring.GetHashCode();
        }

        #endregion
    }

    [DataContract]
    public sealed class StudyPlan
    {
        [DataMember]
        public string Semester { get; set; }

        private IList<Course> m_course = new List<Course>();

        /// <summary>
        /// chekanote: if you want to deserialize, "set" is indispensable
        /// because "initializer"(private IList<Course> m_course = new List<Course>();) will be neglect
        /// during deserializing process
        /// so if "set" isn't provided, then property "Course" will only return a null reference
        /// </summary>
        [DataMember]
        public IList<Course> Courses
        {
            get { return m_course; }
            set { m_course = value; }
        }

        #region "override methods"

        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;

            if (object.ReferenceEquals(this, obj))
                return true;

            if (obj is StudyPlan)
            {
                StudyPlan otherplan = (StudyPlan)obj;

                if ((this.Semester.Equals(otherplan.Semester))
                    && (m_course.Count == otherplan.m_course.Count))
                {
                    for (int index = 0; index < m_course.Count; ++index)
                    {
                        if (!m_course[index].Equals(otherplan.m_course[index]))
                            return false;
                    }// for, iterate over each element course
                    return true;
                }
                else
                    return false;
            }
            else
                return false;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        #endregion
    }

    [TestFixture]
    public sealed class DataContractSingleObjectTest
    {
        /// <summary>
        /// serialize without any indent
        /// </summary>
        private static Course StreamDeserializer(DataContractSerializer serializer, Course oriCourse)
        {
            const string FileName = "RawSingleDataContract.xml";

            // ---------------- serialize
            // !!!!!!!!! pay attention that when serialize, we are not using Writer, but stream
            using (Stream outputStream = File.Create(FileName))
            {
                serializer.WriteObject(outputStream, oriCourse);
            }

            // ---------------- deserialize
            // !!!!!!!!! pay attention that when deserialize, we are not using Reader, but also stream
            Course cpyCourse = null;
            using (Stream inputStream = File.OpenRead(FileName))
            {
                cpyCourse = (Course)serializer.ReadObject(inputStream);
            }

            return cpyCourse;
        }

        public void CheckSingleObject(Func<DataContractSerializer, Course, Course> copyFunction)
        {
            DataContractSerializer serializer = new DataContractSerializer(typeof(Course));

            Course oriCourse = new Course
            {
                Id = 1,
                Name = "Python Programming",
                LocationTime = new LocationTime { Location = "Sloan", Time = "Mon, Wed, 12.15" }
            };
            Course cpyCourse = copyFunction(serializer, oriCourse);

            // ---------------- check equal
            Assert.AreEqual(oriCourse, cpyCourse);

            // check it is deep clone
            Assert.AreNotSame(oriCourse, cpyCourse);
            Assert.AreNotSame(oriCourse.LocationTime, cpyCourse.LocationTime);
        }

        [Test]
        public void TestSingleObjectStream()
        {
            CheckSingleObject(StreamDeserializer);
        }

        public static Course XmlDeserializer(DataContractSerializer serializer, Course oriCourse)
        {
            const string FileName = "IndentSingleDataContract.xml";

            XmlWriterSettings settings = new XmlWriterSettings { Indent = true };

            using (XmlWriter writer = XmlWriter.Create(FileName, settings))
                serializer.WriteObject(writer, oriCourse);

            Course cpyCourse = null;
            using (XmlReader reader = XmlReader.Create(FileName))
                cpyCourse = (Course)serializer.ReadObject(reader);

            return cpyCourse;
        }

        public static Course XmlDictDeserializer(DataContractSerializer serializer, Course oriCourse)
        {
            const string FileName = "XmlBinarySingleObject.xml";

            // ---------------- serialize
            // !!!!!!!!! pay attention that when serialize, we are not using Writer, but stream
            using (XmlDictionaryWriter xdictwriter = XmlDictionaryWriter.CreateBinaryWriter(File.Create(FileName)))
            {
                serializer.WriteObject(xdictwriter, oriCourse);
            }

            // ---------------- deserialize
            // !!!!!!!!! pay attention that when deserialize, we are not using Reader, but also stream
            Course cpyCourse = null;
            using (XmlDictionaryReader xdictreader = XmlDictionaryReader.CreateBinaryReader(File.OpenRead(FileName), XmlDictionaryReaderQuotas.Max))
            {
                cpyCourse = (Course)serializer.ReadObject(xdictreader);
            }

            return cpyCourse;
        }

        [Test]
        public void TestSingleObjectXml()
        {
            CheckSingleObject(XmlDeserializer);
        }

        /// <summary>
        /// from this testcase, we can know that "xml+binary"
        /// is just the envelope is still XML, but content is not text, but binary
        /// </summary>
        [Test]
        public void TestSingleObjectXmlDictionary()
        {
            CheckSingleObject(XmlDictDeserializer);
        }
    }

    [TestFixture]
    public sealed class DataContractMultiObjectTest
    {
        [Test]
        public void TestFileIO()
        {
            StudyPlan oriPlan = new StudyPlan { Semester = "2010 Fall" };
            oriPlan.Courses.Add(new Course
            {
                Id = 1,
                Name = "Python",
                LocationTime = new LocationTime { Location = "sloan", Time = "Mon 12.10" }
            });
            oriPlan.Courses.Add(new Course
            {
                Id = 2,
                Name = "Distributed System",
                LocationTime = new LocationTime { Location = "EME", Time = "Mon 16.15" }
            });

            const string FileName = "MultiObjectFile.xml";
            DataContractSerializer serializer = new DataContractSerializer(typeof(StudyPlan));

            // ---------------- serialize
            using (XmlWriter writer = XmlWriter.Create(FileName, new XmlWriterSettings { Indent = true }))
            {
                serializer.WriteObject(writer, oriPlan);
            }

            // ---------------- deserialize
            StudyPlan cpyPlan = null;
            using (XmlReader reader = XmlReader.Create(FileName))
            {
                cpyPlan = (StudyPlan)serializer.ReadObject(reader);
            }

            // ---------------- check
            Assert.AreEqual(oriPlan, cpyPlan);
        }
    }
}