using System;
using System.Collections.Generic;
using System.IO;

namespace ConfigEditor.DataAccess
{
    public sealed class MeasRepository
    {
        // ************************************** //
        #region "member fields"

        private IList<Measurement> _measurements;

        #endregion

        // ************************************** //
        #region "constructor"

        public MeasRepository(string filePath)
        {
            _measurements = new List<Measurement>();

            using (StreamReader reader = File.OpenText(filePath))
            {
                string line = reader.ReadLine();
                while (!reader.EndOfStream)
                {
                    line = reader.ReadLine();
                    var measurement = Parse(line);
                    _measurements.Add(measurement);
                }
            }//using
        }

        #endregion

        // ************************************** //
        #region "public API"

        public IEnumerable<Measurement> Measurements
        {
            get { return _measurements; }
        }

        #endregion

        // ************************************** //
        #region "private helpers"

        private static Measurement Parse(string line)
        {
            string[] segments = line.Split(',');
            if (segments.Length != 5)
            {
                throw new InvalidOperationException("wrong format");
            }
            return new Measurement
                       {
                           Key = segments[0],
                           SignalReference = segments[1],
                           Device = segments[2],
                           SignalType = segments[3],
                           PhasorType = segments[4]
                       };
        }

        #endregion
    }
}
