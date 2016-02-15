using System.Drawing;
using ZedGraph;

namespace DemoZedGraph.StaticCurves
{
    public sealed class Curve
    {
        public string Name { get; set; }
        public Color Color { get; set; }
        public SymbolType SymbolType { get; set; }
    }

    public sealed class Points
    {
        public double[] Xdatas { get; private set; }
        public double[] Ydatas { get; private set; }
        public string Name { get; private set; }

        public Points(string name, int size)
        {
            Name = name;
            Xdatas = new double[size];
            Ydatas = new double[size];
        }

        public void Set(int index, double x, double y)
        {
            Xdatas[index] = x;
            Ydatas[index] = y;
        }

    }
}
