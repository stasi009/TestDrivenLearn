
using System;
using System.Collections.Generic;
using System.Linq;

using NUnit.Framework;

namespace LinqTest
{
    [TestFixture]
    sealed class TestJoin
    {
        // ************************************* //
        #region [ setup ]

        private IDictionary<int, string> m_customers;
        private IList<Tuple<int, string>> m_orders;
        private string[] m_expected;

        [SetUp]
        public void Setup()
        {
            m_customers = new Dictionary<int, string>() 
            {
                {1,"Henry"},
                {2,"Tom"}
            };

            m_orders = new List<Tuple<int, string>> 
            {
                Tuple.Create(1,"computer"),
                Tuple.Create(2,"TV"),
                Tuple.Create(1,"refrigerator"),
                Tuple.Create(3,"laptop"),
            };

            m_expected = new string[]                
            {                    
                "Henry buy computer",
                "Henry buy refrigerator",
                "Tom buy TV"
            };
        }

        #endregion

        // ************************************* //
        // If an element from left has no matching right, that left will not appear in the results
        [Test]
        public void TestFastJoin()
        {
            var query = from customer in m_customers
                        join aorder in m_orders
                        on customer.Key equals aorder.Item1
                        select string.Format("{0} buy {1}", customer.Value, aorder.Item2);
            CollectionAssert.AreEqual(m_expected, query);
        }

        [Test]
        public void TestSlowSelect()
        {
            var slowQuery = from customer in m_customers
                            from aorder in m_orders
                            where customer.Key == aorder.Item1
                            select string.Format("{0} buy {1}", customer.Value, aorder.Item2);
            CollectionAssert.AreEqual(m_expected, slowQuery);
        }

        [Test]
        public void TestUsingFluentSyntax()
        {
            var query = m_customers.Join
                (
                m_orders,
                customer => customer.Key,
                aorder => aorder.Item1,
                (customer, aorder) => string.Format("{0} buy {1}", customer.Value, aorder.Item2)
                );
            CollectionAssert.AreEqual(m_expected, query);
        }
    }

    [TestFixture]
    sealed class TestGroupJoin
    {
        // ************************************* //
        #region [ setup ]

        private IDictionary<int, string> m_customers;
        private IList<Tuple<int, string>> m_orders;
        private IDictionary<string, IList<string>> m_expected;

        [SetUp]
        public void Setup()
        {
            m_customers = new Dictionary<int, string>() 
            {
                {1,"Henry"},
                {2,"Tom"},
                {3,"empty"}// show that groupby is a left outer join, which will include empty
            };

            m_orders = new List<Tuple<int, string>> 
            {
                Tuple.Create(1,"computer"),
                Tuple.Create(2,"TV"),
                Tuple.Create(1,"refrigerator")
            };

            // ------------------------- form the expected result
            m_expected = new Dictionary<string, IList<string>>
            {
                {"Henry", new List<string> {"computer", "refrigerator"}},
                {"Tom", new List<string> {"TV"}}
            };
        }

        #endregion

        // ************************************* //
        // If no elements from the right source sequence are found to match an element in the left source, 
        // the join clause will produce an empty array for that item
        [Test]
        public void TestDivideIntoGroup()
        {
            var query = from acustomer in m_customers
                        join aorder in m_orders
                        on acustomer.Key equals aorder.Item1
                        into ordersOfCustomer
                        where ordersOfCustomer.Any() // empty will be included in the final query result, since GroupJoin is a left outer join
                        select new KeyValuePair<string, IEnumerable<string>>(acustomer.Value, ordersOfCustomer.Select(aorder => aorder.Item2));

            foreach (KeyValuePair<string, IEnumerable<string>> kv in query)
            {
                IList<string> expected = m_expected[kv.Key];
                CollectionAssert.AreEqual(expected, kv.Value);
            }
        }// for TestDivideIntoGroup

        [Test]
        public void TestFlatWithJoinGroup()
        {
            var query = from acustomer in m_customers
                        join aorder in m_orders on acustomer.Key equals aorder.Item1
                        into ordersOfCustomer
                        from singleOrderOfCustomer in ordersOfCustomer.DefaultIfEmpty()// transform into "SelectMany"
                        select Tuple.Create
                        (
                            acustomer.Value,
                            (singleOrderOfCustomer != null) ? singleOrderOfCustomer.Item2 : string.Empty
                        );
            CollectionAssert.AreEquivalent(new Tuple<string, string>[]
            {
                Tuple.Create("Henry","computer"),
                Tuple.Create("Henry","refrigerator"),
                Tuple.Create("Tom","TV"),
                Tuple.Create("empty",string.Empty)
            }, query);
        }
    }

    [TestFixture]
    sealed class TestLookup
    {
        // ************************************* //
        #region [ setup ]

        private IDictionary<int, string> m_customers;
        private ILookup<int, string> m_orders;

        [SetUp]
        public void Setup()
        {
            m_customers = new Dictionary<int, string>() 
            {
                {1,"Henry"},
                {2,"Tom"},
                {3,"empty"}// show that groupby is a left outer join, which will include empty
            };

            m_orders = new List<Tuple<int, string>> 
            {
                Tuple.Create(1,"computer"),
                Tuple.Create(2,"TV"),
                Tuple.Create(1,"refrigerator")
            }.ToLookup(t => t.Item1, t => t.Item2);
        }

        #endregion

        // ************************************* //
        /// <summary>
        /// from this test we can see another difference between Dictionary and Lookup
        /// that is when non-existing key is met, Lookup will return a empty collection 
        /// while Dictionary will throw an exception
        /// </summary>
        [Test]
        public void TestSimulatedInnerJoin()
        {
            var query = from acustomer in m_customers
                        from productName in m_orders[acustomer.Key]
                        select new Tuple<string, string>(acustomer.Value, productName);
            CollectionAssert.AreEqual(
                new Tuple<string, string>[]
                {
                    Tuple.Create("Henry","computer"),
                    Tuple.Create("Henry","refrigerator"),
                    Tuple.Create("Tom","TV")
                }, query);
        }

        [Test]
        public void TestSimulatedOuterJoin()
        {
            var query = from acustomer in m_customers
                        from productName in m_orders[acustomer.Key].DefaultIfEmpty()
                        select new Tuple<string, string>(acustomer.Value, productName != null ? productName : string.Empty);
            CollectionAssert.AreEqual(
                new Tuple<string, string>[]
                {
                    Tuple.Create("Henry","computer"),
                    Tuple.Create("Henry","refrigerator"),
                    Tuple.Create("Tom","TV"),
                    Tuple.Create("empty",string.Empty)
                }, query);
        }
    }
}
