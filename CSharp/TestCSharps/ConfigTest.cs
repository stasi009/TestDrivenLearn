
using System;
using System.Configuration;

using NUnit.Framework;

namespace CSharpBasicTest
{
    /// <summary>
    /// test application configuration function in C#
    /// </summary>
    [TestFixture]
    public class AppConfigTest
    {
        /// <remark>
        /// 1. during building process, App.config will be copied into the destination directory for storing the build result, and be re-named to "(projectName).exe.config" or "(projectName).dll.config"
        /// 2. when App.Config is modified, then it willl result in a re-build. During the re-build process, App.Config will be copied and re-named again. Pay attention that, re-build will not always copy and re-name App.Config. If the modification happens in any files other than App.Config, it will re-build, but the configuration file in "bin" directory will not be update
        /// 3. during runtime, the application will read the configuration file under "bin" directory, not the original App.Config, which means that if you modified the xxx.exe.config or xxx.dll.config, it will effect when the application starts next time. (but if this xxx.exe.config or xxx.dll.config is modified, NUnit will not re-load the assembly and its runtime configuration(xxx.exe.config or xxx.dll.config) automatically, we must re-load test manually to check the effect after modification)
        /// </remark>
        [Test]
        public void TestRetrieve()
        {
            // both "ConfigurationSettings" and "ConfigurationManager" are defined in the namespace
            // of "System.Configuration", but the former one is implemented in System.dll, but the later
            // one is implemented in System.configuration.dll
            // this is a example that logic scope (namespace) are different from physical scope(dll)
            Assert.AreEqual("Hello Cheka From C#", ConfigurationManager.AppSettings["Message"]);
        }

        /// <summary>
        /// this testcase shows that when want to retrieve a value from the configuration with non-existed key
        /// then no exception will throw, but only return a null string
        /// </summary>
        [Test]
        public void TestRetrieveNonExisted()
        {
            Assert.AreEqual(null, ConfigurationManager.AppSettings["NonExisted"]);
        }
    }
}