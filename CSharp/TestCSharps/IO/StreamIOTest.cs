
using System;
using System.IO;

using NUnit.Framework;

namespace CSharpBasicTest
{
    [TestFixture]
    public sealed class StreamIOTest
    {
        //*********************************************************//
        #region [ testcase ]

        [Test]
        public void TestAccessRight()
        {
            string fileName = "file4AccessTest.dat";

            using (FileStream onlyWriteStream = File.OpenWrite(fileName))
            {
                Assert.IsTrue(onlyWriteStream.CanWrite);
                Assert.IsFalse(onlyWriteStream.CanRead);
            }

            using (FileStream onlyReadStream = File.OpenRead(fileName))
            {
                Assert.IsTrue(onlyReadStream.CanRead);
                Assert.IsFalse(onlyReadStream.CanWrite);
            }
        }

        [Test]
        public void TestFileIOStream()
        {
            //------------------- prepare ----------------------------------//
            byte[] writeBytes = new byte[1024];

            Random rand = new Random();
            rand.NextBytes(writeBytes);

            byte[] readBytes = new byte[1024];
            string filename = "testfilestreamio.dat";

            //------------------- write ----------------------------------//
            using (FileStream wfs = File.Create(filename))
            {
                Assert.IsTrue(wfs.CanWrite);
                // chekanote: file stream opened by "Create" other than "OpenWrite" can be both read and written
                Assert.IsTrue(wfs.CanRead);

                wfs.Write(writeBytes, 0, writeBytes.Length);
            }

            //------------------- read ----------------------------------//
            int readLength;
            using (FileStream rfs = File.OpenRead(filename))
            {
                Assert.IsTrue(rfs.CanRead);
                Assert.IsFalse(rfs.CanWrite);

                readLength = rfs.Read(readBytes, 0, readBytes.Length);

                Assert.AreEqual(-1, rfs.ReadByte());// check that it has reached the end of the file
            }

            //------------------- check ----------------------------------//
            Assert.AreEqual(writeBytes.Length, readLength);
            CollectionAssert.AreEqual(writeBytes, readBytes);
        }

        /// <summary>
        /// test write and read all bytes
        /// </summary>
        [Test]
        public void TestAllBytesRW()
        {
            byte[] writeBytes = new byte[1024];
            new Random().NextBytes(writeBytes);

            string fileName = "test_rw_allbytes.dat";
            File.WriteAllBytes(fileName, writeBytes);

            byte[] readBytes = File.ReadAllBytes(fileName);

            CollectionAssert.AreEqual(writeBytes, readBytes);
        }

        #endregion
    }

    [TestFixture]
    sealed class MemoryStreamTest
    {
        private static Random m_rand = new Random();

        /// <summary>
        /// test each time "ToArray" is invoked, it will return a totally new copy
        /// not an internal cached one
        /// </summary>
        [Test]
        public void TestToArrayCopy()
        {
            byte[] srcBytes = new byte[1024];
            m_rand.NextBytes(srcBytes);

            MemoryStream stream = new MemoryStream();
            stream.Write(srcBytes, 0, srcBytes.Length);
            stream.Close();

            // even meory stream is close, ToArray still works
            byte[] cpyBytes1 = stream.ToArray();
            byte[] cpyBytes2 = stream.ToArray();

            // check
            CollectionAssert.AreEqual(srcBytes, cpyBytes1);
            CollectionAssert.AreEqual(srcBytes, cpyBytes2);
            Assert.AreNotSame(cpyBytes1, cpyBytes2);
        }

        [Test]
        public void TestToArrayOverride()
        {
            byte[] longBytes = new byte[1024];
            m_rand.NextBytes(longBytes);

            byte[] shortBytes = new byte[512];
            m_rand.NextBytes(shortBytes);

            // ------------------------- first write the long bytes
            MemoryStream stream = new MemoryStream();
            stream.Write(longBytes, 0, longBytes.Length);
            CollectionAssert.AreEqual(longBytes, stream.ToArray());

            // ------------------------- second write short bytes
            stream.Position = 0;
            stream.Write(shortBytes, 0, shortBytes.Length);

            // !!!!! returns a combined array, the first half has been overwritten by shortBytes
            // !!!!! but the last half remains
            Assert.AreEqual(longBytes.Length, stream.ToArray().Length);

            stream.Close();
        }

        [Test]
        public void TestSetLengthToOverwrite()
        {
            byte[] longBytes = new byte[1024];
            m_rand.NextBytes(longBytes);

            byte[] shortBytes = new byte[512];
            m_rand.NextBytes(shortBytes);

            // ------------ write long buffer
            MemoryStream memstream = new MemoryStream();
            memstream.Write(longBytes, 0, longBytes.Length);

            // ------------ write short buffer, since short, so cannot overwrite all previous content
            // ------------ results in that the stream is a combined one
            memstream.Position = 0;
            memstream.Write(shortBytes, 0, shortBytes.Length);

            Assert.AreEqual(shortBytes.Length, memstream.Position);
            Assert.AreEqual(longBytes.Length, memstream.Length);

            // ------------ set stream's length to be zero, then truncate stream 
            // ------------ then following write will only fill new content, other than a combined one
            memstream.SetLength(0);
            memstream.Position = 0;
            memstream.Write(shortBytes, 0, shortBytes.Length);

            Assert.AreEqual(shortBytes.Length, memstream.Position);
            Assert.AreEqual(shortBytes.Length, memstream.Length);
        }
    }
}
