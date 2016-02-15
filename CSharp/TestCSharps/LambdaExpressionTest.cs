
using System;

using NUnit.Framework;

namespace CSharpBasicTest
{
    [TestFixture]
    sealed class LambdaExpressionTest
    {
        [Test]
        public void TestSimpleLE()
        {
            Func<int, int> sqr = x => x * x;
            Assert.AreEqual(16, sqr(4));

            Func<int, int> doubleFunc = x => { return x * 2; };
            Assert.AreEqual(56, doubleFunc(28));

            Func<string, string, int> sumLenFunc = (string s1, string s2) => s1.Length + s2.Length;
            Assert.AreEqual(8, sumLenFunc("cheka", "wsu"));
        }

        /// <summary>
        /// !!!!!!!!!! from this testcase, 
        /// we also can see that captured variables are evaluated when invoked, not when lambda expression
        /// are defined (like in LINQ, so called "Defered Execution" feature)
        /// !!!!!!!!!! we should pay much attention to this problem, because in multi-thread context, this maybe cause race condition
        /// !!!!!!!!!! if that lambda expression is executed in another thread, when that thread really runs, the value of that captured
        /// !!!!!!!!!! variables have already been changed
        /// !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!! another problem should be mentioned is when the captured is a reference
        /// !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!! since the reference is always evaluated when invoking, not capturing
        /// !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!! so the background mechanism is not as passing reference parameter into functions
        /// !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!! then if the paramter and real reference, if one of them change, will not affect the other
        /// !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!! Pay Attention that, the case is not reference passing, but the captured variable
        /// !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!! is only one, always only one, whether inside closure or outside
        /// </summary>
        [Test]
        public void TestOuterVariableAccess()
        {
            int x = 0;
            Action SelfIncrease = (() => ++x);

            SelfIncrease();
            Assert.AreEqual(1, x);

            SelfIncrease();
            Assert.AreEqual(2, x);
        }

        /// <summary>
        /// below test shows the "Defer Evaluation" feature of lambda expression
        /// because captured variables are evaluated when lambda expression is executed, not when declared
        /// so each lambda expression will always access the most new, recent, updated value when being invoked
        /// (captured variable are encapsulated as public member field when appearing in closure, so change in the closure
        /// is directly changed on the outer variable)
        /// </summary>
        [Test]
        public void TestCaptureVarDeferEvaluateFeature()
        {
            int counter = 0;

            const int TOTAL = 5;
            Func<int>[] funcs = new Func<int>[TOTAL];
            for (int index = 0; index < TOTAL; ++index)
            {
                funcs[index] = () => { ++counter; return counter; };
            }

            int[] results = new int[TOTAL];
            for (int index = 0; index < TOTAL; ++index)
                results[index] = funcs[index]();

            // ---------------------------- check result
            for (int index = 1; index <= TOTAL; ++index)
            {
                Assert.AreEqual(index, results[index - 1]);
            }
            Assert.AreEqual(TOTAL, counter);
        }

        Func<int> SelfIncrement
        {
            get
            {
                int seed = 0;
                return () => { ++seed; return seed; };
            }
        }

        [Test]
        public void TestVariableLifeExtension()
        {
            Func<int> selfIncrementFunc = this.SelfIncrement;

            Assert.AreEqual(1, selfIncrementFunc());
            Assert.AreEqual(2, selfIncrementFunc());

            // --------------- the seed is bound to a single lambda expression, not shared globally
            Func<int> selfIncrementFunc2 = this.SelfIncrement;
            Assert.AreEqual(1, selfIncrementFunc2());
        }

        [Test]
        public void TestSameIterateVariables()
        {
            const int Length = 3;

            Func<int>[] sameVarFuncs = new Func<int>[Length];
            for (int i = 0; i < Length; ++i)
            {
                sameVarFuncs[i] = () => { return i; };
            }

            foreach (Func<int> afunc in sameVarFuncs)
                Assert.AreEqual(Length, afunc());
        }

        [Test]
        public void TestDifferentIterateVariables()
        {
            const int Length = 3;

            Func<int>[] diffVarFuncs = new Func<int>[Length];
            for (int i = 0; i < Length; ++i)
            {
                int temp = i;
                diffVarFuncs[i] = () => { return temp; };
            }

            for (int i = 0; i < Length; ++i)
                Assert.AreEqual(i, diffVarFuncs[i]());
        }
    }
}