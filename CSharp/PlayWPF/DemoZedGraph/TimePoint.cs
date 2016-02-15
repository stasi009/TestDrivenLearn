using System;

namespace DemoZedGraph
{
    public sealed class TimePoint
    {
        public DateTime Time { get; private set; }
        public double Value { get; private set; }
        public string Name { get; private set; }

        public TimePoint(string name, DateTime time, double value)
        {
            this.Name = name;
            this.Time = time;
            this.Value = value;
        }
    }
}
