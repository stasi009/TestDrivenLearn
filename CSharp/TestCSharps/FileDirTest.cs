
using System;
using System.IO;

using NUnit.Framework;

namespace CSharpBasicTest
{
    [TestFixture]
    sealed class FileDirTest
    {
        [Test]
        [Ignore]
        public void TestCurrentDirectory()
        {
            // ------------- original current directory
            Assert.AreEqual(@"D:\Study\programming\TestDrivenLearn\CSharp\bin\debug", Directory.GetCurrentDirectory());

            // ------------- check a file doesn't exist under current directory
            Assert.IsFalse(File.Exists("TestDaemonConfigs.xml"));
            Assert.IsTrue(File.Exists(@"Test/TestDaemonConfigs.xml"));

            // ------------- change current directory by using relative path
            Directory.SetCurrentDirectory("Test");

            // ------------- then file can be found
            Assert.IsTrue(File.Exists("TestDaemonConfigs.xml"));
        }
    }

    [TestFixture]
    sealed class PathTest
    {
        /// <summary>
        /// given a path, split that path into "directory" and "file"
        /// </summary>
        [Test]
        public void TestSplitDirFile()
        {
            string path = @"D:\study\programming\CSharp\bin\debug\CSharpBasicTest.dll";
            Assert.AreEqual(@"D:\study\programming\CSharp\bin\debug", Path.GetDirectoryName(path));
            Assert.AreEqual("CSharpBasicTest.dll", Path.GetFileName(path));
        }
    }
}