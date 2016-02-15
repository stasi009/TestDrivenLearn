using System;
using System.Text;
using System.Linq;
using System.Xml.Linq;

using NUnit.Framework;

namespace LinqTest.XML
{
    [TestFixture]
    sealed class XmlSample
    {
        [Test]
        public void Sample()
        {
            XElement config = XElement.Parse(
                @"<configuration>    
                    <client enabled='true'>
                        <timeout>30</timeout>
                    </client>
                    <server enabled='false'>
                        <timeout>60</timeout>
                    </server>
                </configuration>");

            Assert.AreEqual(2, config.Elements().Count());

            // ---------- read
            XElement client = config.Element("client");
            bool enabled = (bool)client.Attribute("enabled");
            Assert.IsTrue(enabled);

            int timeout = (int)client.Element("timeout");
            Assert.AreEqual(30, timeout);

            // ---------- write
            XElement server = config.Element("server");
            timeout = (int)server.Element("timeout");
            server.Element("timeout").SetValue(timeout * 3);

            server.Add(new XElement("host", "localhost"));

            Console.WriteLine(config);
        }

        private void ManualConstruct()
        {
            var lastName = new XElement("lastname", "stasi");
            lastName.Add(new XComment("strong"));

            var customer = new XElement("customer");
            customer.Add(new XAttribute("id", 123));
            // customer.Add(new XText("agency"));
            customer.Add(new XElement("firstname", "cheka"));
            customer.Add(lastName);

            Console.WriteLine(customer.ToString(SaveOptions.None));
        }

        private void FunctionalConstruction()
        {
            var customer = new XElement("customer",
                                        new XAttribute("id", 123),
                                        new XElement("firstname", "cheka"),
                                        new XElement("lastname", "stasi",
                                            new XComment("powerful")));
            Console.WriteLine(customer);
        }

        [Test]
        public void TestNameValue()
        {
            var address = new XElement("address",
                                       new XElement("street", "1st street"),
                                       new XElement("city", "2nd city"),
                                       new XComment("comments"));
            Assert.AreEqual("address", address.Name.ToString());
            Assert.AreEqual("1st street2nd city", address.Value);

            // chekanote: element can be explicitly cast to string, the result of the cast
            // equals Element's value
            var valueByCast = (string)address;
            Assert.AreEqual(address.Value, valueByCast);
        }

        [Test]
        public void AutoDeepCloning()
        {
            var address = new XElement("address",
                                       new XElement("street", "1st street"),
                                       new XElement("city", "2nd city"));
            var customer1 = new XElement("customer", address);
            var customer2 = new XElement("customer", address);

            // ------------- demonstrate that Address in customer1
            // ------------- and customer2 are totally seperate copy
            customer1.Element("address").Element("street").SetValue("3rd avenue");
            string street2 = (string)customer2.Element("address").Element("street");
            Assert.AreEqual("1st street", street2);
        }

        [Test]
        public void TestName()
        {
            var element = new XElement("address", "none");

            // to show that Name property is actually XName type
            // but it can still compare with string directly
            Assert.IsFalse(element.Name == "id");
            Assert.IsTrue(element.Name == "address");
            Assert.IsTrue(element.Name is XName);
        }

        public static void TestMain()
        {
            var sample = new XmlSample();

            // sample.Sample();
            // sample.ManualConstruct();
            sample.FunctionalConstruction();
        }
    } // XmlSample
}
