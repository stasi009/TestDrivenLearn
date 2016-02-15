
using System;
using System.Collections.Generic;
using System.Linq;

using NUnit.Framework;

namespace LinqTest
{
    [TestFixture]
    sealed class TestGeneration
    {
        [Test]
        public void TestRange()
        {
            CollectionAssert.AreEqual(new[] { 2, 3, 4 }, Enumerable.Range(2, 3));
        }

        /// <summary>
        /// the static "Generation" method of Enumerable is just create a state machine
        /// the parameter of this state machine has been fixed when it is first time created
        /// such as start point and total number
        /// these parameter are copied into the constructor of the state machine,NOT captured variable
        /// so modification of outside variables will not affect the parameter of the state machine, even though it hasn't been executed
        /// so modification of outside variables will not affect the result of state machine
        /// </summary>
        [Test]
        public void TestDelayRange()
        {
            int start = 4;
            int total = 2;
            IEnumerable<int> deferred = Enumerable.Range(start, total);

            start = 1;
            total = 3;
            IList<int> concrete = deferred.ToList();

            Assert.AreEqual(2, concrete.Count);
            CollectionAssert.AreEqual(new[] { 4, 5 }, concrete);
            CollectionAssert.AreNotEqual(new[] { 1, 2, 3 }, concrete);
        }

        [Test]
        public void TestRepeat()
        {
            CollectionAssert.AreEqual(new[] { 'x', 'x' }, Enumerable.Repeat('x', 2));
        }

        [Test]
        public void TestDelayRepeat()
        {
            char letter = 'x';
            int count = 2;
            IEnumerable<char> deferred = Enumerable.Repeat(letter, count);

            letter = 'y';
            count = 3;
            char[] letters = deferred.ToArray();

            CollectionAssert.AreEqual(new[] { 'x', 'x' }, letters);
            CollectionAssert.AreNotEqual(new[] { 'y', 'y','y' }, letters);
        }
    }
}