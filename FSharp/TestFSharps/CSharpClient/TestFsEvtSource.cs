using System;
using System.Collections.Generic;

using NUnit.Framework;

using TestFSharps;

namespace CSharpClient
{
    [TestFixture]
    sealed class TestFsEvtSource
    {
        [Test]
        public void Demo()
        {
            IList<int> sink = new List<int>();

            FsEventSource source = new FsEventSource("stasi");
            source.NumChangeEvent += (sender, num) =>
            {
                // sender is null (because we didn't specify it at the event source)
                Assert.IsNull(sender);
                sink.Add(num);
            };

            source.Num = 1;

            source.Name = "cheka";
            source.Num = 121;
            source.Num = 88;

            CollectionAssert.AreEqual(new int[] {1,121,88 }, sink);
        }// Demo
    }
}
