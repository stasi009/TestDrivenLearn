
using System;
using System.Collections.Generic;
using System.Linq;

using NUnit.Framework;

namespace LinqTest
{
    [TestFixture]
    sealed class TestSelect
    {
        [Test]
        public void TestIndex()
        {
            string[] names = { "Tom", "Dick", "Harry" };
            var query = names.Select((s, i) => string.Format("[{0}]-{1}", i.ToString(), s.ToLower()));
            CollectionAssert.AreEqual(new[] { "[0]-tom", "[1]-dick", "[2]-harry" }, query);
        }

        /// <summary>
        /// below codes should be better to use "join" to solve
        /// below codes is only for demonstration purpose, not best practice
        /// </summary>
        [Test]
        public void TestBuildHierarchy()
        {
            Tuple<int, string>[] classes = { new Tuple<int, string>(1, "power system"), new Tuple<int, string>(2, "computer") };
            Tuple<int, string>[] students = { Tuple.Create(1, "Tom"), Tuple.Create(2, "Tom"), Tuple.Create(1, "Mary") };

            var query = from cl in classes
                        select new
                        {
                            ClassId = cl.Item1,
                            ClassName = cl.Item2,
                            Students = from st in students
                                       where st.Item1 == cl.Item1
                                       select st.Item2
                        };
            var classInfos = query.ToArray();

            Assert.AreEqual(1, classInfos[0].ClassId);
            Assert.AreEqual("power system", classInfos[0].ClassName);
            CollectionAssert.AreEqual(new[] { "Tom", "Mary" }, classInfos[0].Students);

            Assert.AreEqual(2, classInfos[1].ClassId);
            Assert.AreEqual("computer", classInfos[1].ClassName);
            CollectionAssert.AreEqual(new[] { "Tom" }, classInfos[1].Students);
        }

        [Test]
        public void TestAnonymousType()
        {
            string[] inputs = { "aPPLE", "BlUeBeRrY", "cHeRry" };
            string[] expectedUpper = { "APPLE", "BLUEBERRY", "CHERRY" };
            string[] expectedLower = { "apple", "blueberry", "cherry" };

            var results = from s in inputs
                          select new { Upper = s.ToUpper(), Lower = s.ToLower() };
            int index = 0;
            foreach (var result in results)
            {
                Assert.AreEqual(expectedUpper[index], result.Upper);
                Assert.AreEqual(expectedLower[index], result.Lower);
                ++index;
            }
        }
    }

    [TestFixture]
    sealed class TestSelectMany
    {
        private readonly string[] m_fullNames = { "Anne Williams", "John Fred Smith" };
        private readonly string[] m_expected = new[] { "Anne", "Williams", "John", "Fred", "Smith" };

        [Test]
        public void Demo1()
        {
            var queryMany = m_fullNames.SelectMany(s => s.Split());
            CollectionAssert.AreEqual(m_expected, queryMany);

            // ==================== achieve the same result with "Select"
            IEnumerable<string[]> querySingle = m_fullNames.Select(s => s.Split());
            IList<string> wordlist = new List<string>();
            foreach (string[] strarray in querySingle)
            {
                foreach (string str in strarray)
                {
                    wordlist.Add(str);
                }
            }
            CollectionAssert.AreEqual(m_expected, wordlist);
        }

        [Test]
        public void TestUsingQuerySyntax()
        {
            var query = from fullname in m_fullNames
                        from word in fullname.Split()
                        select word;
            CollectionAssert.AreEqual(m_expected, query);
        }

        [Test]
        public void TestQuerySyntaxOuterRangeVariable()
        {
            var query = from fullname in m_fullNames
                        from word in fullname.Split()
                        select string.Format("[{0}] from [{1}]", word, fullname);
            CollectionAssert.AreEqual(new[]
                {
                    "[Anne] from [Anne Williams]",
                    "[Williams] from [Anne Williams]",
                    "[John] from [John Fred Smith]",
                    "[Fred] from [John Fred Smith]",
                    "[Smith] from [John Fred Smith]"
                }, query);
        }

        [Test]
        public void TestJoinDemo1()
        {
            int[] numbers = { 1, 2, 3 };
            char[] letters = { 'x', 'y' };

            var query = from l in letters
                        from n in numbers
                        select string.Format("{0}-{1}", l, n);
            CollectionAssert.AreEqual(
                new[] 
                { 
                    "x-1", "x-2","x-3", "y-1", "y-2","y-3" 
                }, query);
        }

        [Test]
        public void TestJoinDemo2()
        {
            string[] players = { "Tom", "Jay", "Mary" };

            var query = from s1 in players
                        from s2 in players
                        where s1.CompareTo(s2) > 0
                        select s1 + " vs. " + s2;
            CollectionAssert.AreEqual(new[] { "Tom vs. Jay", "Tom vs. Mary", "Mary vs. Jay" }, query);
        }

        [Test]
        public void TestFilterBeforeJoin()
        {
            // filter before inner loop to reduce total loop count
            IEnumerable<string> results = from fullname in m_fullNames
                                          where fullname.StartsWith("A")
                                          from word in fullname.Split()
                                          select word;
            CollectionAssert.AreEqual(new string[] { "Anne", "Williams" }, results);
        }

        [Test]
        public void TestSelectManyIndexed()
        {
            IEnumerable<string> results = m_fullNames.SelectMany((fullname, index) => from word in fullname.Split()
                                                                                      select word + index.ToString());
            CollectionAssert.AreEqual(new string[] { "Anne0", "Williams0", "John1", "Fred1", "Smith1" }, results);
        }
    }

    [TestFixture]
    sealed class TestMultipleProject
    {
        private int[] m_input;
        private int[] m_expected;

        [SetUp]
        public void Setup()
        {
            m_input = new int[] { 1, 2, 3, 4, 5 };
            m_expected = new int[] { 9, 16, 25 };
        }

        [Test]
        public void TestConcatenatedQuery()
        {
            IEnumerable<int> squared = from orinum in m_input
                                       select orinum * orinum;
            IEnumerable<int> final = from squnum in squared
                                     where squnum > 8
                                     select squnum;
            CollectionAssert.AreEqual(m_expected, final);
        }

        /// <summary>
        /// "let" will introduce a new temporary value
        /// and when using that temporary value, value in outer scope is also invalid
        /// </summary>
        [Test]
        public void TestLetKeyword()
        {
            IEnumerable<int> final = from orinum in m_input
                                     let squnum = orinum * orinum
                                     where squnum > 8
                                     select squnum;
            CollectionAssert.AreEqual(m_expected, final);
        }

        /// <summary>
        /// "into" also introduce a new value
        /// but that new value is in a totally new isolated scope, value, identifier in previous scope are not valid any more
        /// !!!!!!! Actually, since using "into" will introduces an isolated scope, which will not be shared with previous scope
        /// !!!!!!! using "select...into..." is much similiar with "concatenated query"
        /// </summary>
        [Test]
        public void TestIntoKeyword()
        {
            IEnumerable<int> final = from orinum in m_input
                                     select orinum * orinum
                                         into squnum
                                         where squnum > 8
                                         select squnum;
            CollectionAssert.AreEqual(m_expected, final);
        }

        [Test]
        public void TestShareFeatureByLet()
        {
            IEnumerable<Tuple<int, int>> results = from orinum in m_input
                                                   let squnum = orinum * orinum
                                                   where squnum > 10
                                                   select Tuple.Create(orinum, squnum);// both "orinum" and "squnum" are valid here
            CollectionAssert.AreEqual(new[]
                                          {
                                              Tuple.Create(4,16),
                                              Tuple.Create(5,25)
                                          }, results);

            // !!!!!!!! select...into... totally isolated, will not let you access the previous identifier
            /*
            var impossible = from orinum in m_input
                             select orinum * orinum
                                 into squnum
                                 where squnum > 10
                                 select Tuple.Create(orinum, squnum);// !!! "orinum" here cannot be solved
             * */
        }

        /// <summary>
        /// if using fluent syntax, it becomes easy
        /// but should pay attention to the invocation order
        /// </summary>
        [Test]
        public void TestUseFluentSyntax()
        {
            IEnumerable<int> correctResults = m_input.Select(num => num * num).Where(squNum => squNum > 8);
            CollectionAssert.AreEqual(m_expected, correctResults);

            IEnumerable<int> wrongResults = m_input.Where(num => num > 8).Select(num => num * num);
            Assert.IsFalse(wrongResults.Any());
        }
    }

    [TestFixture]
    sealed class TestMultipleProject2
    {
        private readonly string[] m_names = { "Tom", "Dick", "Harry", "Mary", "Jay" };
        private readonly string[] m_expected = new[] { "Dick", "Harry", "Mary" };

        [Test]
        public void Demo1()
        {
            var intermQuery = from s in m_names
                              select new
                              {
                                  OriName = s,
                                  NovowelName = s.Replace("a", "").Replace("e", "").Replace("i", "").Replace("o", "").Replace("u", "")
                              };
            var result = from record in intermQuery
                         where record.NovowelName.Length > 2
                         select record.OriName;
            CollectionAssert.AreEqual(m_expected, result);
        }

        [Test]
        public void Demo2()
        {
            var result = from s in m_names
                         select new
                         {
                             OriName = s,
                             NovowelName = s.Replace("a", "").Replace("e", "").Replace("i", "").Replace("o", "").Replace("u", "")
                         }
                             into temp
                             where temp.NovowelName.Length > 2
                             select temp.OriName;
            CollectionAssert.AreEqual(m_expected, result);
        }

        [Test]
        public void TestLet()
        {
            var result = from s in m_names
                         let vowelless = s.Replace("a", "").Replace("e", "").Replace("i", "").Replace("o", "").Replace("u", "")
                         where vowelless.Length > 2
                         orderby vowelless
                         select s;// the feature is that "s" is still in scope
            CollectionAssert.AreEqual(m_expected, result);
        }
    }
}