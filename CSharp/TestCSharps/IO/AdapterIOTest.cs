
using System;
using System.IO;
using System.Text;
using System.Collections.Generic;

using NUnit.Framework;

namespace CSharpBasicTest
{
    [TestFixture]
    public sealed class AdapterIOTest
    {
        public interface IReaderWriterFactory
        {
            TextReader NewReader(string filename);
            TextWriter NewWriter(string filename);
        }

        /// <summary>
        /// create the reader and writer by the static method of File class
        /// </summary>
        class FileStaticMethodFactory : IReaderWriterFactory
        {
            public TextReader NewReader(string filename) { return File.OpenText(filename); }
            public TextWriter NewWriter(string filename) { return File.CreateText(filename); }
        }

        /// <summary>
        /// create the reader and writer by the constructor of the adapter, which wraps the stream directly
        /// </summary>
        class StreamAdapterConstructFactory : IReaderWriterFactory
        {
            public TextReader NewReader(string filename) { return new StreamReader(File.OpenRead(filename)); }
            public TextWriter NewWriter(string filename) { return new StreamWriter(File.Create(filename)); }
        }

        public void WriteReadFile(IReaderWriterFactory factory, string filename)
        {
            string msg1 = "Hello Cheka";
            string msg2 = "Hello WSU from C#";

            using (TextWriter writer = factory.NewWriter(filename))
            {
                writer.WriteLine(msg1);
                writer.WriteLine(msg2);
            }

            using (TextReader reader = factory.NewReader(filename))
            {
                string readmsg = reader.ReadLine();
                Assert.AreEqual(msg1, readmsg);

                readmsg = reader.ReadLine();
                Assert.AreEqual(msg2, readmsg);
            }
        }

        [Test]
        public void TestFileRwFileStaticMethod()
        {
            WriteReadFile(new FileStaticMethodFactory(), "testfilerw_filestaticmethod.txt");
        }

        [Test]
        public void TestFileRwAdapterConstructor()
        {
            WriteReadFile(new StreamAdapterConstructFactory(), "testfilerw_adapterconstruct.txt");
        }

        /// <summary>
        /// just like the DataInputStream and DataOutputStream in Java
        /// !!!!!!!!!!!!!!!!!!!! ONE BIG DIFFERENCE BETWEEN C# STREAM AND JAVA STREAM IS THAT
        /// !!!!!!!!!!!!!!!!!!!! in java, the output stream and input stream are seperated, the stream can
        /// !!!!!!!!!!!!!!!!!!!! only read or write, never both
        /// !!!!!!!!!!!!!!!!!!!! but in C#, the same stream can be used for both read and write purpose
        /// !!!!!!!!!!!!!!!!!!!! until in the adapter level, C# begins to distinguish reader and writer
        /// </summary>
        [Test]
        public void TestBinaryRW()
        {
            Random rand = new Random();
            MemoryStream memStream = new MemoryStream();

            int srcInt = rand.Next();
            float srcSingleValue = (float)rand.NextDouble();
            double srcDoubleValue = rand.NextDouble();
            string srcString = "hello Cheka from WSU";

            //---------------------------- write
            BinaryWriter bwriter = new BinaryWriter(memStream);
            bwriter.Write(srcInt);// 4 bytes
            bwriter.Write(srcSingleValue);
            bwriter.Write(srcDoubleValue);
            bwriter.Write(srcString);

            bwriter.Flush();// even for memory stream, better no close. no bug, just maybe confuse other people

            //---------------------------- read
            // !!!!!!!!!!!!!!!!!!!!!!!!!!!! to position to the beginning of the stream, we can 
            // !!!!!!!!!!!!!!!!!!!!!!!!!!!! use either lines of codes below
            memStream.Seek(0, SeekOrigin.Begin);
            // memStream.Position = 0;

            BinaryReader breader = new BinaryReader(memStream);
            Assert.AreEqual(srcInt, breader.ReadInt32());
            Assert.AreEqual(srcSingleValue, breader.ReadSingle(), 1e-6);
            Assert.AreEqual(srcDoubleValue, breader.ReadDouble(), 1e-6);
            Assert.AreEqual(srcString, breader.ReadString());

            Assert.Throws<EndOfStreamException>(delegate { breader.ReadByte(); });
        }

        /// <summary>
        /// write multiple strings into binary writer
        /// and read multipe strings from binary reader
        /// test that these multiple strings can be correctly demarked
        /// !!!!!!!!!!!!!!!!!! actually BinaryWriter will prefix text length for BinaryReader
        /// </summary>
        [Test]
        public void TestBinaryMultiStringRW()
        {
            string[] oriStrings = new string[]
            { 
                "Hello C# from Cheka in WSU", 
                "Cheka, Stasi, KGB",
                "China, USA, C#, F#"
            };

            using (MemoryStream memStream = new MemoryStream())
            {
                // ------------------------------ write
                // !!!!!!!!!!!!!!!!!!!!! cannot close binary writer after writing
                // !!!!!!!!!!!!!!!!!!!!! which will close the internal stream, preventing it to be read 
                // !!!!!!!!!!!!!!!!!!!!! this feature has been proved by failure test
                BinaryWriter bwriter = new BinaryWriter(memStream);
                foreach (string str in oriStrings)
                    bwriter.Write(str);

                memStream.Position = 0;

                // ------------------------------ read
                BinaryReader breader = new BinaryReader(memStream);
                for (int index = 0; index < oriStrings.Length; ++index)
                {
                    string cpyString = breader.ReadString();
                    Assert.AreEqual(oriStrings[index], cpyString);
                }

                Assert.Throws<EndOfStreamException>(() => { breader.ReadByte(); });
            }// mutiple dispose is just ok
        }

        [Test]
        public void TestStringWriterReader()
        {
            string[] srcMsgs = new string[] { "line 1", "line 2" };

            //******************************** write
            StringWriter writer = new StringWriter();
            foreach (string msg in srcMsgs)
                writer.WriteLine(msg);

            //******************************** read
            StringReader reader = new StringReader(writer.ToString());

            IList<string> cpyMsgs = new List<string>();
            string readstring = null;
            while ((readstring = reader.ReadLine()) != null)
            {
                cpyMsgs.Add(readstring);
            }

            //******************************** check
            CollectionAssert.AreEqual(srcMsgs, cpyMsgs);
        }
    }
}