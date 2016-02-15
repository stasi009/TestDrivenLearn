using System.IO;
using System.Linq;
using System.Xml;
using System.Xml.Serialization;
using NUnit.Framework;

namespace CSharpBasicTest.serialize
{
    [TestFixture]
    public sealed class XmlSerializeTest
    {
        #region "inner classes"

        public enum DevType
        {
            Bus,
            Line,
            Generator,
            Load
        }

        // chekanote: it can only serialize those "public" types
        // so we have to enlarge the "Access Privilege"
        // if we make this class non-public, it will throw exception in XmlSerializer's constructor
        public sealed class Device
        {
            public int StartBus { get; set; }

            public int EndBus { get; set; }

            public DevType DeviceType { get; set; }

            public override bool Equals(object obj)
            {
                Device otherdev = (Device)obj;
                return this.StartBus == otherdev.StartBus && this.EndBus == otherdev.EndBus &&
                       this.DeviceType == otherdev.DeviceType;
            }

            public override int GetHashCode()
            {
                return base.GetHashCode();
            }
        }

        #endregion

        #region "app methods"

        [Test]
        public static void SerializeSingle()
        {
            Device original = new Device { DeviceType = DevType.Bus, StartBus = 2, EndBus = 3 };
            XmlSerializer serializer = new XmlSerializer(typeof(Device));

            // --------------------- serialize
            StringWriter writer = new StringWriter();
            serializer.Serialize(new XmlTextWriter(writer), original);

            // --------------------- deserialize
            StringReader reader = new StringReader(writer.ToString());
            Device copy = (Device)serializer.Deserialize(reader);

            // --------------------- check
            Assert.AreEqual(2, copy.StartBus);
            Assert.AreEqual(3, copy.EndBus);
            Assert.AreEqual(DevType.Bus, copy.DeviceType);
            Assert.AreEqual(original, copy);
        }

        [Test]
        public static void SerializeMultiple()
        {
            Device[] original = new Device[]
                                   {
                                       new Device {DeviceType = DevType.Bus, StartBus = 1, EndBus = 2},
                                       new Device { DeviceType = DevType.Line, StartBus = 3, EndBus = 4 }
                                   };
            XmlRootAttribute root = new XmlRootAttribute("Devices");
            XmlSerializer serializer = new XmlSerializer(typeof(Device[]), root);

            // --------------------- serialize
            StringWriter writer = new StringWriter();
            serializer.Serialize(new XmlTextWriter(writer), original);

            // --------------------- deserialize
            StringReader reader = new StringReader(writer.ToString());
            Device[] copy = (Device[])serializer.Deserialize(reader);

            // --------------------- check
            Assert.AreEqual(2, copy.Length);
            CollectionAssert.AreEqual(original, copy);
            CollectionAssert.AreEqual(new DevType[] { DevType.Bus, DevType.Line }, copy.Select(dev => dev.DeviceType));
            CollectionAssert.AreEqual(new int[] { 1, 3 }, copy.Select(dev => dev.StartBus));
            CollectionAssert.AreEqual(new int[] { 2, 4 }, copy.Select(dev => dev.EndBus));
        }

        public static void TestMain()
        {
            // SerializeSingle();
            SerializeMultiple();
        }

        #endregion
    }
}
