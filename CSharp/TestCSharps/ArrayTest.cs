using System;
using System.Collections;
using System.Collections.Generic;

using NUnit.Framework;

namespace CSharpBasicTest
{
    /// <summary>
    /// this testcase is used to test the syntax of array
    /// </summary>
    [TestFixture]
    public class ArrayTest
    {
        class Element : IComparable<Element>
        {
            private int m_number;

            public Element(int number)
            {
                m_number = number;
            }

            public int CompareTo(Element other)
            {
                // even private member variable can be accessed within the same type
                return m_number - other.m_number;
            }
        }

        Element m_minimum;
        Element m_middle;
        Element m_maximum;
        Element[] m_arrOriginal;

        /// <summary>
        /// test the default value when intializing an array
        /// </summary>
        [Test]
        public static void TestInitialize()
        {
            const int ARR_LEN = 5;

            #region [when element is value type]

            int[] intArray = new int[ARR_LEN];
            Assert.That(intArray, Is.All.EqualTo(0));
            Assert.AreEqual(ARR_LEN, intArray.Length);

            char[] charArray = new char[ARR_LEN];
            Assert.AreEqual(ARR_LEN, charArray.Length);
            Assert.That(charArray, Is.All.EqualTo('\0'));

            charArray = new char[] { 'a', 'e', 'i' };
            Assert.AreEqual(3, charArray.Length);
            Assert.AreEqual('a', charArray[0]);
            Assert.AreEqual('e', charArray[1]);
            Assert.AreEqual('i', charArray[2]);

            #endregion

            #region [when element is reference type]

            Element[] elemArray = new Element[ARR_LEN];
            Assert.That(elemArray, Has.All.Null);
            Assert.That(elemArray, Has.None.Not.Null);

            #endregion
        }

        [Test]
        public void TestRectangleArray()
        {
            int numRow = 3;
            int numCol = 4;
            int[,] rectangleArray = new int[numRow, numCol];
            Assert.AreEqual(2, rectangleArray.Rank);
            Assert.AreEqual(numRow, rectangleArray.GetLength(0));
            Assert.AreEqual(numCol, rectangleArray.GetLength(1));
            // for rectangle array, it behaves like a matrix,the "Length" property return the count
            // of all the numbers in this matrix, all dimensions included
            Assert.AreEqual(numRow * numCol, rectangleArray.Length);

            rectangleArray = new int[,] { { 0, 1, 2 }, { 4, 5, 6 } };
            Assert.AreEqual(2, rectangleArray.GetLength(0));// number of rows
            Assert.AreEqual(3, rectangleArray.GetLength(1)); // number of columns
            // check the element in the array
            Assert.AreEqual(1, rectangleArray[0, 1]);
            Assert.AreEqual(4, rectangleArray[1, 0]);
            Assert.AreEqual(6, rectangleArray[1, 2]);
        }

        [Test]
        public void TestCompareRectArray()
        {
            int[,] matrix1 = new int[,] { { 0, 1, 2 }, { 3, 4, 5 } };

            int numRows = 2;
            int numCols = 3;
            int value = 0;
            int[,] matrix2 = new int[numRows, numCols];
            for (int row = 0; row < numRows; ++row)
            {
                for (int col = 0; col < numCols; col++)
                {
                    matrix2[row, col] = value;
                    ++value;
                }
            }

            CollectionAssert.AreEqual(matrix1, matrix2);

            matrix2[0, 0] += 1;
            CollectionAssert.AreNotEqual(matrix1, matrix2);
        }

        [Test]
        public static void TestJaggedArray()
        {
            int[][] jagArray = new int[3][];
            Assert.AreEqual(1, jagArray.Rank);// jag array is not a matrix, its rank is only one
            Assert.AreEqual(3, jagArray.GetLength(0));
            // for jagged array, GetLength(0) is same with "Length" property
            // !!!!! pay attention that this will not happen in rectangle array
            Assert.AreEqual(jagArray.Length, jagArray.GetLength(0));
            Assert.That(jagArray, Has.All.Null);

            jagArray[0] = new int[2];
            Assert.IsInstanceOf(typeof(int[]), jagArray[0]);
            Assert.AreEqual(2, jagArray[0].Length);
            Assert.That(jagArray[0], Has.All.EqualTo(0));

            jagArray[2] = new int[] { 4, 7, 8, 9 };
            Assert.AreEqual(4, jagArray[2].Length);
            Assert.AreEqual(4, jagArray[2][0]);
            Assert.AreEqual(8, jagArray[2][2]);
        }

        [Test]
        public void TestArrayEnumerable()
        {
            int[] intArray = new int[4];
            // check that array also implements "IEnumerable" interface
            Assert.IsInstanceOf(typeof(IEnumerable), intArray);

            // integer array is the more specific type
            // so it can not be assigned from a general type 'IEnumerable'
            Assert.IsNotAssignableFrom(typeof(IEnumerable), intArray);

            foreach (int x in intArray)
                Assert.AreEqual(0, x);
        }

        [Test]
        [ExpectedException(typeof(IndexOutOfRangeException))]
        public void TestIndexOutRangeException()
        {
            int[] intArray = new int[5];
            intArray[100] = 89;
        }

        [SetUp]
        public void Setup()
        {
            m_minimum = new Element(1);
            m_middle = new Element(2);
            m_maximum = new Element(3);
            // type[] array = {instance1, instance2, instance3}==>sthis kind of initialization can only occurs when "define and initialize" the array
            // type[] array = null;
            // array = {instance1,instance2,instance3}==> can not pass the compilation
            m_arrOriginal = new[] { m_middle, m_maximum, m_minimum };
        }

        [Test]
        public void TestSortByComparable()
        {
            Assert.That(m_arrOriginal, Is.Not.Ordered);

            // ording based on "IComparable" interface
            // if the element doesn't implement the "IComparable" interface, then a runtime exception will be thrown
            Array.Sort(m_arrOriginal);
            Element[] expectedArray = { m_minimum, m_middle, m_maximum };
            CollectionAssert.AreEqual(expectedArray, m_arrOriginal);
            CollectionAssert.IsOrdered(m_arrOriginal);
        }

        // the abstract class "Comparer<Element>" has been derived from both 
        // IComparer and IComparer<Element>
        class ReverseComparer : Comparer<Element>
        {
            public override int Compare(Element x, Element y)
            {
                return y.CompareTo(x);
            }
        }

        [Test]
        public void TestSortByComparer()
        {
            ReverseComparer comparer = new ReverseComparer();
            Array.Sort(m_arrOriginal, comparer);
            CollectionAssert.IsOrdered(m_arrOriginal, comparer);

            Element[] expectedArray = { m_maximum, m_middle, m_minimum };
            CollectionAssert.AreEqual(expectedArray, m_arrOriginal);
        }

        [Test]
        public void TestBinarySearch()
        {
            Array.Sort(m_arrOriginal);
            Assert.AreEqual(1, Array.BinarySearch(m_arrOriginal, m_middle));
            Assert.AreEqual(0, Array.BinarySearch(m_arrOriginal, m_minimum));
        }

        /// <summary>
        /// for integer, as a value type, array checks equality based on each bit
        /// !!!!!!!!!!!!!!!!!!!!!! it seems that C# has not provide any equivalent function like the static "Array.equals" in Java
        /// </summary>
        [Test]
        public void TestEquals()
        {
            Random rand = new Random();

            int total = rand.Next(1, 100);
            int[] array1 = new int[total];
            int[] array2 = new int[total];

            int val;
            for (int index = 0; index < total; ++index)
            {
                val = rand.Next(-200, 200);
                array1[index] = val;
                array2[index] = val;
            }

            Assert.AreNotSame(array1, array2);
            CollectionAssert.AreEqual(array1, array2);

            // !!!!!!!!!!!!!!!!!!!!! System.Array doesn't override Object's Equals
            Assert.IsFalse(array2.Equals(array1));
        }

        [Test]
        public void TestCopy()
        {
            int[] srcArray = new int[32];
            Random rand = new Random();
            for (int index = 0; index < srcArray.Length; ++index)
                srcArray[index] = rand.Next();

            int[] cpyArray = new int[srcArray.Length];
            CollectionAssert.AreNotEqual(srcArray, cpyArray);

            Array.Copy(srcArray, cpyArray, srcArray.Length);
            CollectionAssert.AreEqual(srcArray, cpyArray);
        }
    }
}
