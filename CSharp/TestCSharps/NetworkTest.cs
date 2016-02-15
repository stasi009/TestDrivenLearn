
using System;
using System.Diagnostics;
using System.Net;
using System.Collections.Generic;

using NUnit.Framework;

namespace CSharpBasicTest
{
    [TestFixture]
    class IPEndPointTest
    {
        static IPEndPoint CreateEndPoint(string address, int port)
        {
            return new IPEndPoint(IPAddress.Parse(address), port);
        }

        [Test]
        public void TestIPEndPointEquality()
        {
            IPEndPoint endpoint1 = CreateEndPoint("192.10.10.99", 6001);
            IPEndPoint endpoint2 = CreateEndPoint("192.10.10.99", 6001);

            Assert.AreNotSame(endpoint1, endpoint2);
            Assert.IsFalse(object.ReferenceEquals(endpoint1, endpoint2));

            Assert.IsTrue(endpoint1.Equals(endpoint2));
            Assert.AreEqual(endpoint1, endpoint2);
        }

        /// <summary>
        /// since IPEndPoint has overriden "Equals" method in Object
        /// then this method test some operation in the list, which are based equality checking
        /// </summary>
        [Test]
        public void TestListOperation()
        {
            List<IPEndPoint> endpointList = new List<IPEndPoint>();
            IPEndPoint endpoint1 = CreateEndPoint("192.10.10.98", 1);
            IPEndPoint endpoint2 = CreateEndPoint("192.10.10.99", 2);
            IPEndPoint endpoint3 = CreateEndPoint("192.10.10.100", 3);
            endpointList.Add(endpoint1);
            endpointList.Add(endpoint2);
            endpointList.Add(endpoint3);

            IPEndPoint wanted = CreateEndPoint("192.10.10.99", 2);
            int position = endpointList.IndexOf(wanted);
            Assert.AreEqual(1, position);
            Assert.AreNotSame(wanted, endpointList[position]);
            Assert.IsTrue(endpointList[position].Equals(wanted));

            endpointList.Remove(wanted);
            CollectionAssert.AreEqual(new[] { endpoint1, endpoint3 }, endpointList);
        }

        [Test]
        public void TestIPEndpointAsKey()
        {
            IDictionary<IPEndPoint, object> ipendpointDict = new Dictionary<IPEndPoint, object>();
            IPEndPoint oriKey = CreateEndPoint("192.10.10.99", 1);
            object value = new object();
            ipendpointDict[oriKey] = value;

            IPEndPoint cpyKey = CreateEndPoint("192.10.10.99", 1);
            Assert.AreNotSame(oriKey, cpyKey);

            Assert.IsTrue(ipendpointDict.ContainsKey(cpyKey));
            Assert.AreSame(value, ipendpointDict[cpyKey]);
        }

        [Test]
        public void TestToString()
        {
            string addr = "192.10.10.99";
            int port = 2010;
            IPEndPoint endpoint = CreateEndPoint(addr, port);

            Assert.AreEqual(addr, endpoint.Address.ToString());
            Assert.AreEqual(port, endpoint.Port);
        }

        [Test]
        public void TestShallowClone()
        {
            IPEndPoint srcEndpnt = CreateEndPoint("192.10.10.99", 6001);
            IPEndPoint cpyEndpnt = new IPEndPoint(srcEndpnt.Address, srcEndpnt.Port);

            Assert.AreNotSame(srcEndpnt, cpyEndpnt);
            Assert.AreEqual(srcEndpnt, cpyEndpnt);

            // ------------ they reference the same IPAddress
            Assert.AreSame(srcEndpnt.Address, cpyEndpnt.Address);
            Assert.IsTrue(object.ReferenceEquals(srcEndpnt.Address,cpyEndpnt.Address));
        }

        [Test]
        public void TestDeepClone()
        {
            IPEndPoint srcEndpnt = CreateEndPoint("192.10.10.99", 6001);

            var cpyAddress = new IPAddress(srcEndpnt.Address.GetAddressBytes());
            IPEndPoint cpyEndpnt = new IPEndPoint(cpyAddress,srcEndpnt.Port);

            Assert.AreNotSame(srcEndpnt.Address,cpyEndpnt.Address);
        }

        [Test]
        public void TestEndian()
        {
            Assert.IsTrue(BitConverter.IsLittleEndian);
        }
    }
}