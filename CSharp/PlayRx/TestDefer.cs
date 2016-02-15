using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;

namespace PlayRx
{
    /// <summary>
    /// demonstrate the usage of "Defer", which accept a factory of IObservable and return an IObservable
    /// but the returned IObservable is just a token, not a concrete one
    /// everytime the returned IObservable is subscribed, the factory method will be invoked once again'
    /// the Observer will subscribe to the newest Observable returned by the factory
    /// so this Defer method is always used to get the newest update by re-subscribing
    /// </summary>
    static class TestDefer
    {
        #region "constants"

        private const string PrintFormat = "{0,-20}{1,-28}";

        #endregion

        #region "inner classes"

        sealed class Product
        {
            private readonly string m_name;
            private int m_count;

            public Product(string name, int count0)
            {
                m_name = name;
                m_count = count0;
            }

            public void Change(int number)
            {
                m_count += number;
                if (m_count < 0)
                    m_count = 0;
            }

            public override string ToString()
            {
                return string.Format(PrintFormat, m_name, m_count);
            }
        }

        sealed class Inventory
        {
            private readonly IList<Product> m_products;
            private readonly Random m_random;

            public Inventory(int count0)
            {
                m_random = new Random();

                m_products = (from index in Enumerable.Range(1, count0)
                              select new Product(string.Format("Product{0}", index), m_random.Next(1000)))
                              .ToList();
            }

            public void Change()
            {
                foreach (Product product in m_products)
                {
                    product.Change(m_random.Next(-100, 100));
                }
            }

            public IObservable<Product> GetLatestInventory()
            {
                return m_products.ToObservable();
            }

            public void Add(Product product)
            {
                m_products.Add(product);
            }
        }

        #endregion

        #region "methods"

        private static void Print(this IObservable<Product> products, string info)
        {
            Console.WriteLine();
            Console.WriteLine(info);

            Console.WriteLine(PrintFormat, "Name", "Current Count");
            Console.WriteLine("======================================================================");

            products.Subscribe(Console.WriteLine);
        }

        private static void TestUsingDefer()
        {
            Inventory inventory = new Inventory(6);

            IObservable<Product> latestProducts = Observable.Defer(() =>
                                                                       {
                                                                           inventory.Change();
                                                                           return inventory.GetLatestInventory();
                                                                       });
            latestProducts.Print("changed once, ......");

            Helper.Pause();

            latestProducts.Print("changed twice, ......");
        }

        /// <summary>
        /// chekanote: this method demonstrate two features:
        /// 1. we don't need to use 'defer', in this demo, because "ToObservable" returns a Cold Observable
        /// so everytime being subscribed, the background enumerable will be iterated from start once again, so 
        /// there are always newest items returned back
        /// 2. a completed observer will automatically disposed (unsubscribed) from the observable
        /// so in this demo, when the second time printing, it won't print the same item twice
        /// </summary>
        private static void TestBasicDeferFeature()
        {
            Inventory inventory = new Inventory(4);

            // chekanote: because this products is a cold one, it will always re-execute when subscribed
            // so always return the latest values
            IObservable<Product> products = inventory.GetLatestInventory();
            products.Print("before changing, ......");

            inventory.Change();
            inventory.Add(new Product("***NewProduct1***", 123));
            inventory.Add(new Product("***NewProduct2***", 456));

            products.Print("after changing, ......");
        }

        private static void TestWithRepeat()
        {
            int newIndex = 0;
            Inventory inventory = new Inventory(1);

            IObservable<Product> latestProducts = Observable.Defer(() =>
            {
                ++newIndex;
                inventory.Add(new Product(string.Format("## NewProduct{0}", newIndex), 100 + newIndex));
                return inventory.GetLatestInventory();
            });

            // chekanote: repeat subscribing for 3 times, so the factory will be invoked for three times
            // the same observer will be subscribed three times, however
            // the side effect (such as, printing the title) will only occur once
            latestProducts.Repeat(3).Print("");
        }

        public static void TestMain()
        {
            // TestUsingDefer();
            // TestBasicDeferFeature();
            TestWithRepeat();
        }

        #endregion
    }// TestDefer
}
