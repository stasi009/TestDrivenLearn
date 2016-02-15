using System;
using System.Xml.Linq;
using System.Linq;
using NUnit.Framework;

namespace LinqTest.XML
{
    [TestFixture]
    sealed class TestXmlValues
    {
        [Test]
        public void TestGetValueByCast()
        {
            var e = new XElement("tag",
                88,
                new XAttribute("attr", "something"));

            Assert.AreEqual(88, (int)e);
            Assert.AreEqual("something", (string)e.Attribute("attr"));

            // ----------- nullable values
            int? nonexisted = (int?)e.Attribute("none");
            Assert.IsFalse(nonexisted.HasValue);
        }

        [Test]
        public void TestGetNonExisted()
        {
            var element = new XElement("tag", 88);

            Assert.Throws<ArgumentNullException>(() => { int nonExisted = (int)element.Attribute("none"); });

            int? empty = (int?)element.Attribute("none");
            Assert.IsFalse(empty.HasValue);
            Assert.IsFalse(empty == 0);

            // --------------------- deal with nullable value in an unified way
            var records = XElement.Parse(
                              @"<data>
                                  <customer id='1' name='Mary' credit='100' />
                                  <employer id='9' name='John' credit='150' />
                                  <customer id='3' name='Anne' />
                                </data>");
            var ids = from p in records.Elements()
                      where (int?)p.Attribute("credit") > 120
                      select (int)p.Attribute("id");
            CollectionAssert.AreEqual(new[] { 9 }, ids);
        }

        [Test]
        public void TestMixedValues()
        {
            XElement summary = new XElement("summary",
                      "An XAttribute is ",
                      new XElement("bold", "not"),
                      " an XNode");
            Assert.AreEqual("<summary>An XAttribute is <bold>not</bold> an XNode</summary>", summary.ToString(SaveOptions.DisableFormatting));

            // -------------- it value are just "a concatenation of each child’s value"
            Assert.AreEqual("An XAttribute is not an XNode", (string)summary);
        }

        [Test]
        public void TestAddText()
        {
            // ------------------ add normal texts
            var byNormalText = new XElement("e1", "hello", "world");
            Assert.IsFalse(byNormalText.Elements().Any());
            Assert.AreEqual(1, byNormalText.Nodes().Count());
            Assert.AreEqual("helloworld", (string)byNormalText);

            // ------------------ add by XText
            var byXText = new XElement("e1",
                new XText("hello"),
                new XText("world"));
            Assert.IsFalse(byXText.Elements().Any());
            Assert.AreEqual(2, byXText.Nodes().Count());
            Assert.AreEqual("helloworld", (string)byXText);
        }
    }// TestXmlValues
}
