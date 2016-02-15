
using System;

using NUnit.Framework;

namespace CSharpBasicTest
{
    /// <summary>
    /// check the syntax of delegate
    /// </summary>
    [TestFixture]
    public class DelegateTest
    {
        /// <summary>
        /// 'delegate' is like 'class', which is a type definition
        /// can not be defined within a function
        /// </summary>
        delegate int Transform(int x);
        delegate T GenericTransform<T>(T x);

        class Multipler
        {
            private int m_multipler;
            private ushort m_index = 0;
            private int[] m_outputArray;

            public Multipler(int multipler) { m_multipler = multipler; }

            public Multipler(int[] outputArray,ushort index, int multipler)
            {
                m_outputArray = outputArray;
                m_index = index;
                m_multipler = multipler;
            }

            public int Function(int x) { return m_multipler * x; }

            public int SaveInArray(int x)
            {
                m_outputArray[m_index] = m_multipler * x;
                return m_outputArray[m_index];
            }
        }

        [Test]
        public void TestSingleDelegate()
        {
            Transform transformer = DoubleNumber;
            Assert.AreEqual(10,transformer(5));

            transformer = SquareNumber;
            Assert.AreEqual(36,transformer(6));

            Multipler m1 = new Multipler(100);
            transformer = m1.Function;
            Assert.AreEqual(700,transformer(7));
        }

        [Test]
        public void TestDelegateArray()
        {
            Transform[] arrayTransformer = new Transform[4];
            arrayTransformer[0] = DoubleNumber;
            arrayTransformer[1] = SquareNumber;

            Multipler m1 = new Multipler(8);
            Multipler m2 = new Multipler(100);

            arrayTransformer[2] = m1.Function;
            arrayTransformer[3] = m2.Function;

            int input = 9;
            int[] expectedOutput = { 18, 81, 72, 900 };
            int[] realOutput = new int[4];

            for (int index = 0; index < 4; ++index)
                realOutput[index] = arrayTransformer[index](input);

            CollectionAssert.AreEqual(expectedOutput,realOutput);
        }

        [Test]
        public void TestMulticastDelegate()
        {
            int[] realOutput = new int[3];

            Transform transformer = null;
            transformer += new Multipler(realOutput,0,-1).SaveInArray;
            transformer += new Multipler(realOutput,1, 2).SaveInArray;
            transformer += new Multipler(realOutput,2, 6).SaveInArray;

            // the result value is the result of the last method to be invoked
            Assert.AreEqual(36,transformer(6));

            int[] expectedOutput = {-6,12,36};
            CollectionAssert.AreEqual(expectedOutput,realOutput);
        }

        [Test]
        public void TestGenericDelegate()
        {
            GenericTransform<int> t1 = DoubleNumber;
            Assert.AreEqual(10,t1(5));

            GenericTransform<float> t2 = TrippleNumber;
            Assert.AreEqual(18.0f,t2(6.0f),1e-6);

            GenericTransform<Helper> t3 = DealwithHelper;
            Helper h = new Helper(8);
            h = t3(h);
            Assert.AreEqual(40,h.Number);
        }

        private int DoubleNumber(int x) { return x + x; }
        private int SquareNumber(int x) { return x * x; }
        private float TrippleNumber(float x) { return x * 3; }

        class Helper
        {
            private int m_number;
            public Helper(int number) { m_number = number; }

            public int Number
            {
                get { return m_number; }
                set { m_number = value; }
            }
        }
        private Helper DealwithHelper(Helper x)
        {
            x.Number = x.Number * 5;
            return x;
        }
    }

    [TestFixture]
    public class EventTest
    {
        class PriceChangedEventArgs : EventArgs
        {
            int m_originalPrice;
            int m_newPrice;

            public PriceChangedEventArgs(int originalPrice, int newPrice)
            {
                m_originalPrice = originalPrice;
                m_newPrice = newPrice;
            }

            public int OriginalPrice { get { return m_originalPrice; } }
            public int NewPrice { get { return m_newPrice; } }
        }

        class Stock
        {
            private string m_name;
            private int m_price;

            /// <remark>
            /// The compiler converts the keyword "event" to the following:
            /// 1. A private delegate field
            /// 2. A public pair of event accessor functions, whose implementations forward the += and -= operations to the private delegate field
            /// </remak>
            public event EventHandler<PriceChangedEventArgs> PriceChanged;

            public Stock(string name, int price)
            {
                m_name = name;
                m_price = price;
            }

            public int Price
            {
                get { return m_price; }
                set
                {
                    if (m_price == value)
                        return;
                    else
                    {
                        if (PriceChanged != null)
                            PriceChanged(this, new PriceChangedEventArgs(m_price, value));
                        m_price = value;
                    }

                }
            }// property "Price"
        }// class "Stock"

        private int m_oldPrice;
        private int m_newPrice;
        private Stock m_stock;
        private bool m_bEvtInvoked;

        [SetUp]
        public void Setup()
        {
            m_oldPrice = 78;
            m_newPrice = 99;
            m_bEvtInvoked = false;

            m_stock = new Stock("fortest",m_oldPrice);
            m_stock.PriceChanged += OnPriceChangeSetFlag;
            m_stock.PriceChanged += OnPriceChangeCheckValue;
        }

        [Test]
        public void TestNoEventInvoked()
        {
            m_stock.Price = m_oldPrice;
            Assert.IsFalse(m_bEvtInvoked);
        }

        private void CheckAfterPriceChanged()
        {
            Assert.IsTrue(m_bEvtInvoked);
            Assert.AreEqual(m_newPrice, m_stock.Price);
        }

        [Test]
        public void TestPriceChangedEvent()
        {
            m_stock.Price = m_newPrice;
            CheckAfterPriceChanged();
        }

        private void OnPriceChangeSetFlag(object source, PriceChangedEventArgs args)
        {
            m_bEvtInvoked = true;
        }

        private void OnPriceChangeCheckValue(object source, PriceChangedEventArgs args)
        {
            Assert.AreSame(m_stock,source);
            Assert.AreEqual(m_stock.Price,args.OriginalPrice);// has not been changed yet
            Assert.AreEqual(m_newPrice,args.NewPrice);
        }

        //------------------------------------------------------//
        #region [ test event wrapper ]

        /// <summary>
        /// test the "add" and "remove" keyword for event handler property
        /// </summary>
        class StockWrapper
        {
            //*********************************************//
            #region [ wrapper event handler ]

            public static bool m_wrapperFired = false;
            public static void WrapperEvtHandler(object source, PriceChangedEventArgs evtargs)
            {
                m_wrapperFired = true;
            }

            #endregion

            private Stock m_stock;

            public StockWrapper(Stock aStock)
            {
                m_stock = aStock;
            }

            public int Price
            {
                get { return m_stock.Price; }
                set { m_stock.Price = value; }
            }

            public event EventHandler<PriceChangedEventArgs> PriceChanged
            {
                add { m_stock.PriceChanged += value; }
                remove { m_stock.PriceChanged -= value; }
            }
        }

        [Test]
        public void TestWrapperEventHandler()
        {
            StockWrapper.m_wrapperFired = false;

            StockWrapper wrapper = new StockWrapper(m_stock);
            wrapper.PriceChanged += StockWrapper.WrapperEvtHandler;
            wrapper.Price = m_newPrice;

            CheckAfterPriceChanged();
            Assert.IsTrue(StockWrapper.m_wrapperFired);
        }

        #endregion
    }

    [TestFixture]
    public class AnonymousMethodTest
    {
        [Test]
        public void TestAnonymousMethod()
        {
            Func<int,int> IntOperation = delegate(int x) { return x * x; };
            Assert.AreEqual(9,IntOperation(3));

            IntOperation = delegate(int x) { return x * 3; };
            Assert.AreEqual(18,IntOperation(6));
        }

        [Test]
        public void TestOuterVariableAccess()
        {
            int x = 5;
            int y = 6;

            Action action = delegate() { x += y; };

            action();
            Assert.AreEqual(11,x);
        }
    }
}