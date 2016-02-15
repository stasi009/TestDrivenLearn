using System;
using System.IO;
using System.Xml.Serialization;

namespace MVVM.DevSelector
{
    static class Helper
    {
        public const string FileCandidates = "candidates_devices.xml";
        public const string FileSelected = "selected_devices.xml";

        private static readonly XmlSerializer MSerializer;

        static Helper()
        {
            XmlRootAttribute root = new XmlRootAttribute("Devices");
            MSerializer = new XmlSerializer(typeof(Device[]), root);
        }

        public static void Serialize(string fileName, Device[] devices)
        {
            using (FileStream fs = File.Create(fileName))
            {
                MSerializer.Serialize(fs, devices);
            }
        }

        public static Device[] Deserialize(string fileName)
        {
            try
            {
                using (FileStream fs = File.OpenRead(fileName))
                {
                    return (Device[])MSerializer.Deserialize(fs);
                }//using
            }
            catch (FileNotFoundException ex)
            {
                return new Device[0];
            }
        }
    }// Helper
}
