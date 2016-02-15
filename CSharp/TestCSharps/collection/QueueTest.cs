
using System;
using System.Collections.Generic;
using System.Diagnostics;
using NUnit.Framework;

namespace CSharpBasicTest.collection
{
    [TestFixture]
    sealed class QueueTest
    {
        [Test]
        public void TestDequeueAndPeek()
        {
            // you cannot use a "initializer" syntax to initialize a queue, because it doesn't have "Add" method
            // Queue<int> queue = new Queue<int> { 1, 2 };

            Queue<int> queue = new Queue<int>();
            queue.Enqueue(1);
            queue.Enqueue(2);

            // "Dequeue" remove the head from the queue
            Assert.AreEqual(1, queue.Dequeue());
            Assert.AreEqual(1, queue.Count);

            // "Peek" doesn't remove the head, the item is still in the queue
            Assert.AreEqual(2, queue.Peek());
            Assert.AreEqual(1, queue.Count);
            Assert.AreEqual(2, queue.Peek());
        }
    }
}
