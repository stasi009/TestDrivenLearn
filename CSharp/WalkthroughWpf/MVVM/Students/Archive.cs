using System;
using System.IO;
using System.Xml.Serialization;
using EntityLib;

namespace MVVM.Students
{
    sealed class Archive
    {
        private readonly string m_fileName;
        private readonly XmlSerializer m_serializer;

        public Archive(string fileName)
        {
            m_fileName = fileName;

            XmlRootAttribute root = new XmlRootAttribute("Students");
            m_serializer = new XmlSerializer(typeof(Student[]), root);
        }

        public void Save(Student[] students)
        {
            using (FileStream fs = File.Create(m_fileName))
            {
                m_serializer.Serialize(fs, students);
            }
        }

        public Student[] Load()
        {
            try
            {
                using (FileStream fs = File.OpenRead(m_fileName))
                {
                    return (Student[])m_serializer.Deserialize(fs);
                }
            }
            catch (FileNotFoundException ex)
            {
                return null;
            }
        }
    }// Archive
}
