
using System;
using System.Collections.Generic;
using System.Linq;

using NUnit.Framework;

namespace LinqTest
{
    /// <summary>
    /// test the exeuction feature of LINQ expression
    /// such as deferred execution or immediate execution
    /// </summary>
    [TestFixture]
    public sealed class TestExecuteFeature
    {
        /// <summary>
        /// below codes shows that LINQ expression is deferred executed, only executed when the expression is being iterated
        /// and side effect will only take place when that query is deferred executed
        /// </summary>
        [Test]
        public void TestDeferredExecution()
        {
            IEnumerable<int> range = Enumerable.Range(1, 3);

            // below codes has side effect, so only for demonstration purpose
            // not best practice, not recommended
            int counter = 0;
            IEnumerable<int> results = from num in range
                                       select ++counter;

            int[] expectedResults = { 1, 2, 3 };
            int[] expectedCounters = { 1, 2, 3 };

            int index = 0;
            foreach (int result in results)
            {
                Assert.AreEqual(expectedResults[index], result);
                Assert.AreEqual(expectedCounters[index], counter);
                ++index;
            }
        }

        [Test]
        public void TestImmediateExecution()
        {
            const int Total = 3;
            IEnumerable<int> range = Enumerable.Range(1, Total);

            int counter = 0;
            int[] results = (from num in range
                             select ++counter).ToArray();

            int[] expectedResults = { 1, 2, 3 };
            int index = 0;
            foreach (int result in results)
            {
                Assert.AreEqual(expectedResults[index], result);
                Assert.AreEqual(Total, counter);
                ++index;
            }
        }

        [Test]
        public void TestReuseExpression()
        {
            int[] inputs = { 1, 2, 3 };
            IEnumerable<int> results = from num in inputs
                                       where num % 2 == 0
                                       select num;
            CollectionAssert.AreEqual(new[] { 2 }, results);

            // chekawarn: pay much attention that the input collection of the LINQ expression is not captured variable
            // expression syntax is just equal to fluent syntax, the compiler will transform the expression syntax 
            // into fluent syntax, then in the fluent syntax, the later LINQ operation is always bound to the original input collection
            // even though later that the name of the original input is re-direct to another collection
            // but the binding is already fixed when the LINQ expression is declared
            // THAT IS TO SAY, the previous LINQ expression is equal to "results = inputs.Where(num => num%2 == 0);"
            // then we can see clearly that when inputs is re-directed to other variables, the results should never be affected
            int[] originalInputs = inputs;
            inputs = new int[] { 4, 2, 3 };
            CollectionAssert.AreEqual(new[] { 2 }, results);// same result as the previous query

            // chekanote: if you want to show the deferred execution feature, you should change the content of the original input
            // rather than redirect the input name to other collection
            originalInputs[2] = 10;
            CollectionAssert.AreEqual(new[] { 2, 10 }, results);
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TestReuseExpression2()
        {
            IEnumerable<int> inputs = null;
            IEnumerable<int> results = from num in inputs
                                       select num;
            inputs = new int[0];
            int[] neverGotArray = results.ToArray();
        }

        [Test]
        public void TestCaptureVariable()
        {
            int[] oriNumbers = { 3, 4, 5 };

            int factor = 5;
            var multipleResults = from n in oriNumbers select n * factor;

            // factor is captured, always use the newest value when executing the LINQ express
            factor = 8;
            CollectionAssert.AreEqual(new int[] { 24, 32, 40 }, multipleResults);
        }

        [Test]
        public void TestCaptureVarInLoop()
        {
            IEnumerable<char> charset = "Not what you might expect";

            // ------------------------------------------ only remove 'u'
            foreach (char vowel in "aeiou")
                charset = charset.Where(c => c != vowel);

            // !!! weired, once upon a time, the line below pass the test
            // !!! but now, it failed, only the current line works
            // !!! according to current test result, actually no difference between the two
            // CollectionAssert.AreEqual("Not what yo might expect", charset);
            CollectionAssert.AreEqual("Nt wht y mght xpct", charset);

            // ------------------------------------------ remove all vowel
            foreach (char vowel in "aeiou")
            {
                char temp = vowel;
                charset = charset.Where(c => c != temp);
            }
            CollectionAssert.AreEqual("Nt wht y mght xpct", charset);
        }
    }
}