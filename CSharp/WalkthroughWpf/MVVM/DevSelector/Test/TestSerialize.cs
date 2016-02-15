
using System;
using System.Diagnostics;

using NUnit.Framework;

namespace MVVM.DevSelector.Test
{
    /// <summary>
    /// chekanote: actually, there is no test methods in this class
    /// since I didn't find a easy way to launch non-UI test methods in WPF
    /// so I just put some attributes on the methods, which then can be started by unittest
    /// </summary>
    [TestFixture]
    class TestSerialize
    {
        private static Device[] MakeDevices()
        {
            return new Device[]
            {
                new Device {DeviceType = DevType.Bus,StartBus = 1,EndBus = 1},
                new Device{ DeviceType = DevType.Line,StartBus = 1,EndBus = 2}
            };
        }

        [Test]
        public void DumpIntoFile()
        {
            Helper.Serialize(Helper.FileCandidates, MakeDevices());
        }

        [Test]
        public void TestReadFromFile()
        {
            Device[] devices = Helper.Deserialize(Helper.FileSelected);

            if (devices != null)
            {

                int counter = 0;
                foreach (Device dev in devices)
                {
                    ++counter;
                    Debug.WriteLine("[{0}]: {1},{2}-{3}", counter, dev.DeviceType, dev.StartBus, dev.EndBus);
                }
            }
            else
                Debug.WriteLine("!!! empty !!!");
        }
    }
}
