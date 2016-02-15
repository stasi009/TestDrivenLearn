using System;
using System.Linq;
using System.Xml;
using System.Xml.Linq;
using NUnit.Framework;

namespace LinqTest.XML
{
    [TestFixture]
    sealed class TestXmlQuery
    {
        private XElement m_bench;

        [SetUp]
        public void Setup()
        {
            string xml = @"
                <bench owner=""cheka"" id=""1"">
                    <toolbox>
                        <handtool>Hammer</handtool>
                        <handtool>Rasp</handtool>
                    </toolbox>
                    <toolbox>
                        <handtool>Saw</handtool>
                        <powergun>Nailgun</powergun>
                    </toolbox>
                    <others>
                        <tag>for test</tag>
                        <tag>to learn</tag>
                    </others>
                    <!--be careful with the nailgun-->
                </bench>";
            m_bench = XElement.Parse(xml);
        }

        [Test]
        public void TestNodes()
        {
            // -------------------- node types
            var nodes = m_bench.Nodes().ToArray();
            Assert.AreEqual(4, nodes.Length);

            var ndtypes = from nd in nodes
                          select nd.NodeType;
            CollectionAssert.AreEqual(new[] { XmlNodeType.Element, XmlNodeType.Element, XmlNodeType.Element, XmlNodeType.Comment }, ndtypes);

            // -------------------- names for elements
            var names = from nd in nodes
                        where nd.NodeType == XmlNodeType.Element
                        let element = (XElement)nd
                        select element.Name.ToString();
            CollectionAssert.AreEqual(new[] { "toolbox", "toolbox", "others" }, names);
        }

        [Test]
        public void TestNodeType()
        {
            var summary = XElement.Parse("<summary>An XAttribute is <bold>not</bold> an XNode</summary>");
            var ndtypes = from nd in summary.Nodes()
                         select nd.NodeType;
            CollectionAssert.AreEqual(new XmlNodeType[] { XmlNodeType.Text, XmlNodeType.Element, XmlNodeType.Text }, ndtypes);
        }

        [Test]
        public void TestElements()
        {
            Assert.AreEqual(2, m_bench.Elements("toolbox").Count());

            var names = (from element in m_bench.Elements()
                         select element.Name.ToString()).ToArray();
            CollectionAssert.AreEqual(new[] { "toolbox", "toolbox", "others" }, names);

            // --------------- get elements by OfType
            var names2 = from element in m_bench.Nodes().OfType<XElement>()
                         select element.Name.ToString();
            CollectionAssert.AreEqual(names, names2);
        }

        [Test]
        public void TestSelectMany()
        {
            var handtools = m_bench.Elements("toolbox").Elements("handtool").Select(t => t.Value);
            CollectionAssert.AreEqual(new[] { "Hammer", "Rasp", "Saw" }, handtools);
        }

        [Test]
        public void TestSingle()
        {
            // ----------- the first matched
            var first = m_bench.Element("toolbox").Element("handtool");
            Assert.AreEqual("Hammer", first.Value);

            var others = m_bench.Element("others");
            Assert.AreEqual("for testto learn", (string)others.Value);

            var nonExist = m_bench.Element("none");
            Assert.IsNull(nonExist);
        }

        [Test]
        public void TestDescendants()
        {
            // ------------ by using elements
            // it will return none, because the requested are not direct children
            var none = m_bench.Elements("handtool");
            Assert.AreEqual(0, none.Count());

            // ------------ by using descendants
            // it will search the whole hierarhcy recursively
            var matched = m_bench.Descendants("handtool").Select(t => t.Value);
            CollectionAssert.AreEqual(new[] { "Hammer", "Rasp", "Saw" }, matched);
        }

        [Test]
        public void TestParent()
        {
            bool result = m_bench.Elements().All(e => e.Parent.Equals(m_bench));
            Assert.IsTrue(result);
        }

        [Test]
        public void TestAttributes()
        {
            // -------------------- attributes
            var attributes = m_bench.Attributes().ToArray();
            CollectionAssert.AreEqual(new[] { "owner", "id" }, attributes.Select(a => a.Name.LocalName));
            CollectionAssert.AreEqual(new[] { "cheka", "1" }, attributes.Select(a => a.Value));

            var existed = m_bench.Attributes("owner");
            Assert.AreEqual(1, existed.Count());

            // the difference between "Attributes" and "Attribute"
            // when there is no such attributes
            // "Attributes" return empty sequence
            // "Attribute" return empty XAttribute
            var empty = m_bench.Attributes("xxxxxxxxxx");
            Assert.AreEqual(0, empty.Count());

            var nil = m_bench.Attribute("xxxxxxxxxx");
            Assert.IsNull(nil);

            // -------------------- attribute
            var owner = m_bench.Attribute("owner");
            Assert.AreEqual("cheka", owner.Value);
        }
    }// TestXmlQuery
}
