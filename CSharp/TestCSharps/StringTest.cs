
using System;
using System.Text;
using NUnit.Framework;

namespace CSharpBasicTest
{
    [TestFixture]
    sealed class StringTest
    {
        /// <summary>
        /// check that "string" is reference type, not value type
        /// </summary>
        [Test]
        public void TestStringReferenceType()
        {
            string s1 = "hello cheka from WSU";
            string s2 = s1;// just assign reference, not copy occurs
            Assert.IsTrue(object.ReferenceEquals(s1, s2));
        }

        [Test]
        public void TestEqual()
        {
            string s1 = "cheka";
            string s2 = "CHEKA";
            Assert.IsFalse(s1 == s2);
            Assert.IsFalse(s1.Equals(s2));
            Assert.IsTrue(s1.Equals(s2, StringComparison.OrdinalIgnoreCase));
        }

        /// <summary>
        /// !!!!!!!!!!!!!!!!!!!!!! It overloads the == operator !!!!!!!!!!!!!!!!!!!!!!
        /// When the == operator is used to compare two strings, the Equals method is called, 
        /// which checks for the equality of the contents of the strings rather than the references themselves. 
        /// For instance, "hello".Substring(0, 4)=="hell" is true, even though the references on the two sides of the operator are different 
        /// (they refer to two different string objects, which both contain the same character sequence). 
        /// Note that operator overloading only works here if both sides of the operator are string expressions 
        /// at compile time - operators aren't applied polymorphically. If either side of the operator is of type object 
        /// as far as the compiler is concerned, the normal == operator will be applied, and simple reference equality will be tested. 
        /// </summary>
        [Test]
        public void TestOverrideEqualOperator()
        {
            string s1 = "chekachekachekachekachekachekachekachekachekachekachekachekachekachekachekachekachekachekachekachekacheka";
            string s3 = new string(s1.ToCharArray());

            Assert.AreNotSame(s1, s3);
            Assert.AreEqual(s1, s3);
            Assert.IsTrue(s1 == s3);
        }

        [Test]
        public void TestEncodingGetBytes()
        {
            byte[] bytearray1 = Encoding.ASCII.GetBytes("hello");
            byte[] bytearray2 = Encoding.ASCII.GetBytes("hello");
            Assert.AreNotSame(bytearray1, bytearray2);
            CollectionAssert.AreEqual(bytearray1, bytearray2);
        }

        [Test]
        public void TestStringCache()
        {
            string s1 = "cheka";
            string s2 = "cheka";
            // string doesn't have a copy constructor to enable us to construct a string from another string
            // that's not a big problem, we can just use "=" to "copy" a string, remember string is always immutable
            string s3 = new string(s1.ToCharArray());

            Assert.AreSame(s1, s2);
            Assert.IsTrue(object.ReferenceEquals(s1, s2));

            Assert.AreNotSame(s1, s3);
            Assert.AreEqual(s1, s3);
        }

        [Test]
        public void TestToBytes()
        {
            string oriString = "hello cheka from WSU in C#";

            byte[] buffer = Encoding.Default.GetBytes(oriString);

            string cpyString = Encoding.Default.GetString(buffer);

            Assert.AreEqual(oriString, cpyString);
        }

        [Test]
        public void TestBytesToString()
        {
            string oriString = "hello cheka from WSU in C#";

            byte[] validBuffer = Encoding.ASCII.GetBytes(oriString);

            // --------------- make a larger buffer with meaning less content
            const int Extra = 100;
            byte[] buffer = new byte[validBuffer.Length + Extra];
            Buffer.BlockCopy(validBuffer, 0, buffer, Extra, validBuffer.Length);

            string cpyString = Encoding.ASCII.GetString(buffer, Extra, validBuffer.Length);
            Assert.AreEqual(oriString, cpyString);
        }

        [Test]
        public void TestFormat()
        {
            string msg = "hello cheka";
            int num = 100;

            string formattedString = string.Format("{0}:{1}", num, msg);
            string addedString = num.ToString() + ":" + msg;

            Assert.AreEqual(formattedString, addedString);
        }

        [Test]
        public void TestSearch()
        {
            Assert.IsTrue(("quick brown fox".Contains("brown")));
            Assert.IsTrue(("quick brown fox".EndsWith("fox")));

            Assert.AreEqual(2, "abcde".IndexOf("cd"));
            Assert.AreEqual(2, "abcde".IndexOf("CD", StringComparison.CurrentCultureIgnoreCase));

            Assert.AreEqual(2, "ab,cd ef".IndexOfAny(new char[] { ' ', ',' }));
            Assert.AreEqual(3, "pas5w0rd".IndexOfAny("0123456789".ToCharArray()));

            // test the case when "IndexOf" fails to find the expected character
            // it shows that "IndexOf" uses "-1" to indicate that no expected substring is found
            Assert.AreEqual(-1, "abcde".IndexOf("z"));
        }

        [Test]
        public void TestSplitSingleSeperator()
        {
            string s1 = ",ONE,,TWO,,,THREE,,";
            char[] charSeparators = new char[] { ',' };

            //******************************************* include empty entries
            string[] result = s1.Split(charSeparators, StringSplitOptions.None);
            CollectionAssert.AreEqual(new string[]{
                "",
            "ONE",
            "",
            "TWO",
            "",
            "",
            "THREE",
            "",
            ""}, result);

            //******************************************* exclude empty entries
            result = s1.Split(charSeparators, StringSplitOptions.RemoveEmptyEntries);
            CollectionAssert.AreEqual(new string[] { "ONE", "TWO", "THREE" }, result);

        }

        [Test]
        public void TestSplitMultipleSeperator()
        {
            string s1 = "42,  12, 19";
            char[] seperators = new char[] { ',' };
            Assert.AreEqual(new string[] { "42", "  12", " 19" }, s1.Split(seperators));

            // If two delimiters are adjacent, or a delimiter is found at the beginning or end of this instance, 
            // the corresponding array element contains Empty.
            seperators = new char[] { ',', ' ' };
            Assert.AreEqual(new string[] { "42", "", "", "12", "", "19" }, s1.Split(seperators));

            // exclude the empty entries
            Assert.AreEqual(new string[] { "42", "12", "19" }, s1.Split(seperators, StringSplitOptions.RemoveEmptyEntries));
        }

        [Test]
        public void TestSplitNumberSegments()
        {
            char[] SplitChars = new char[] { ' ' };

            string cmd = "program argument1 argument2";

            // ------------------- split with no count limit
            string[] segNoLimit = cmd.Split(SplitChars);
            CollectionAssert.AreEqual(new string[] { "program", "argument1", "argument2" }, segNoLimit);

            // ------------------- split with count limit
            string[] segWithLimit = cmd.Split(SplitChars, 2);
            CollectionAssert.AreEqual(new string[] { "program", "argument1 argument2" }, segWithLimit);

            // ------------------- count limit exceeds maximum value
            CollectionAssert.AreEqual(segNoLimit, cmd.Split(SplitChars, 100));
        }

        [Test]
        public void TestTrim()
        {
            string oriline = "  abc \t\r\n ";
            int orilength = oriline.Length;

            string trimline = oriline.Trim();

            // verify that the original string is not changed
            Assert.AreEqual(orilength, oriline.Length);
            Assert.AreEqual(3, trimline.Length);

            Assert.AreEqual("0", " 0                           ".Trim());
        }

        [Test]
        public void TestPad()
        {
            // pad a string that all ready excceds the desired length
            string s1 = "cheka";
            s1 = s1.PadRight(5, '*');
            Assert.AreEqual("cheka", s1);// no change

            s1 = "ch";
            s1 = s1.PadRight(4, ' ');
            Assert.AreEqual("ch  ", s1);
        }

        [Test]
        public void TestEmptyOrWhilespace()
        {
            string empty = "";
            Assert.IsTrue(string.IsNullOrEmpty(empty));

            string whitespace = "        ";
            Assert.IsFalse(string.IsNullOrEmpty(whitespace));
            Assert.IsTrue(string.IsNullOrWhiteSpace(whitespace));
        }

        /// <summary>
        /// using '@' to keep escape characters unchanged
        /// </summary>
        [Test]
        public void TestVerbatim()
        {
            // chekanote: below codes cannot pass the compilation, because 'unrecognized escape characters'
            // if you want to specify a path, you must use '@'
            // string path = "D:\study\programming\CSharp\CSharpBasicTest";
            string path = @"D:\study\programming\CSharp\CSharpBasicTest";

            // chekanote: string containing multiple lines must also use '@'
            //string multiLines = "line1
            //line2
            //line3";
            string multiLines = @"line1
            line2
            line3";
        }
    }

    [TestFixture]
    sealed class StringBuilderTest
    {
        /// <summary>
        /// Clear only appers in .NET 4.0
        /// before this version, we can only use "sb.Length = 0" to clear the existing content
        /// </summary>
        [Test]
        public void TestClear()
        {
            StringBuilder sb = new StringBuilder("hello cheka");
            sb.Clear();
            Assert.AreEqual(0, sb.Length);
        }
    }
}