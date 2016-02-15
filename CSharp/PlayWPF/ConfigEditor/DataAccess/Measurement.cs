namespace ConfigEditor.DataAccess
{
    public sealed class Measurement
    {
        public string Key { get; set; }
        public string SignalReference { get; set; }
        public string Device { get; set; }
        public string SignalType { get; set; }
        public string PhasorType { get; set; }
    }
}
