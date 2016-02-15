using System;
using System.Linq;
using System.Xml.Linq;
using NUnit.Framework;

namespace LinqTest.XML
{
    [TestFixture]
    sealed class TestXmlUpdate
    {
        /// <summary>
        /// SetValue can accept Object argument, so it can be passed in 
        /// any kind of type
        /// </summary>
        [Test]
        public void TestSetValue()
        {
            var address = new XElement("address",
                                       new XElement("street", "1st street"),
                                       new XElement("city", "2nd city"));
            var street = address.Element("street");
            street.SetValue(1000);

            Assert.AreEqual("<address><street>1000</street><city>2nd city</city></address>", address.ToString(SaveOptions.DisableFormatting));

            // -------------- set value on parent will replace all its children elements
            address.SetValue("replaced");
            Assert.AreEqual("<address>replaced</address>", address.ToString(SaveOptions.DisableFormatting));
        }

        /// <summary>
        /// Assigning the Value property, can only accept string type
        /// </summary>
        [Test]
        public void TestAssignValue()
        {
            var address = new XElement("address",
                                       new XElement("street", "1st street"),
                                       new XElement("city", "2nd city"));
            var street = address.Element("street");
            street.Value = "88";

            Assert.AreEqual("<address><street>88</street><city>2nd city</city></address>", address.ToString(SaveOptions.DisableFormatting));

            // -------------- set value on parent will replace all its children elements
            address.Value = "replaced";
            Assert.AreEqual("<address>replaced</address>", address.ToString(SaveOptions.DisableFormatting));
        }

        [Test]
        public void TestSetElementValue()
        {
            var settings = new XElement("settings");

            // ------------- first time, it just add
            settings.SetElementValue("timeout", 30);
            Assert.AreEqual("<settings><timeout>30</timeout></settings>", settings.ToString(SaveOptions.DisableFormatting));

            // ------------- second time, it will replace
            settings.SetElementValue("timeout", "n/a");
            Assert.AreEqual("<settings><timeout>n/a</timeout></settings>", settings.ToString(SaveOptions.DisableFormatting));
        }

        [Test]
        public void TestSetAttributeValue()
        {
            var settings = new XElement("settings");

            // ------------- first time, it just add
            settings.SetAttributeValue("timeout", 30);
            Assert.AreEqual(@"<settings timeout=""30"" />", settings.ToString(SaveOptions.DisableFormatting));

            // ------------- second time, it will replace
            settings.SetAttributeValue("timeout", "n/a");
            Assert.AreEqual(@"<settings timeout=""n/a"" />", settings.ToString(SaveOptions.DisableFormatting));
        }

        [Test]
        public void TestAddBeforeAfterSelf()
        {
            var items = new XElement("items",
                                     new XElement("one"),
                                     new XElement("three"));
            items.FirstNode.AddAfterSelf(new XElement("two"));
            Assert.AreEqual("<items><one /><two /><three /></items>", items.ToString(SaveOptions.DisableFormatting));
        }

        [Test]
        public void TestRemoveSequence()
        {
            string xml = @"<contacts>
                            <customer name='Mary'/>
                            <customer name='Chris' archived='true'/>
                            <supplier name='Susan'>
                                <phone archived='true'>012345678<!--confidential--></phone>
                            </supplier>
                            </contacts>";
            XElement contacts1 = XElement.Parse(xml);

            contacts1.Elements("customer").Remove();

            Assert.AreEqual(1, contacts1.Elements().Count());
            Assert.AreEqual("supplier", ((XElement)contacts1.FirstNode).Name.LocalName);

            // ------------ filter and then remove
            XElement contacts2 = XElement.Parse(xml);
            var chris = from e in contacts2.Elements("customer")
                        where e.Attribute("name").Value.Equals("Chris")
                        select e;
            Assert.AreEqual(1, chris.Count());

            contacts2.Elements().Where(e => ((bool?)e.Attribute("archived") == true)).Remove();

            Assert.AreEqual(2, contacts2.Elements().Count());
            Assert.AreEqual(0, chris.Count());
        }

        [Test]
        public void TestRmvSeqFromDescendants()
        {
            string xml = @"<contacts>
                            <customer name='Mary'/>
                            <customer name='Chris' archived='true'/>
                            <supplier name='Susan'>
                                <phone archived='true'>012345678<!--confidential--></phone>
                            </supplier>
                            </contacts>";
            XElement contacts = XElement.Parse(xml);

            contacts.Descendants().Where(e => (bool?)e.Attribute("archived") == true).Remove();

            Assert.AreEqual(@"<contacts><customer name=""Mary"" /><supplier name=""Susan"" /></contacts>", contacts.ToString(SaveOptions.DisableFormatting));
        }

        [Test]
        public void TestAddEnumerable()
        {
            int[] values = { 66, 99 };
            var numbers = new XElement("numbers",
                from val in values
                select new XElement("num", val));
            Assert.AreEqual("<numbers><num>66</num><num>99</num></numbers>", numbers.ToString(SaveOptions.DisableFormatting));
        }
    }// TestXmlUpdate
}
