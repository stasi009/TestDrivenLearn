using System;
using System.Windows;
using System.Windows.Media;
using Microsoft.Research.DynamicDataDisplay;
using Microsoft.Research.DynamicDataDisplay.Charts;
using Microsoft.Research.DynamicDataDisplay.Common;
using Microsoft.Research.DynamicDataDisplay.DataSources;

namespace DemoD3.RealtimeCurve
{
    internal sealed class Point
    {
        public DateTime Time { get; private set; }
        public double Value { get; private set; }

        public Point(DateTime time, double value)
        {
            this.Time = time;
            this.Value = value;
        }
    }

    sealed class Curve
    {
        // --------------------------------------------- //
        #region "member fields"

        private readonly string _name;
        private readonly Color _color;
        private readonly RingArray<Point> _points;
        private readonly Func<long, double> _calculator;
        private LineGraph _linegraph;

        #endregion

        // --------------------------------------------- //
        #region "constructor"

        public Curve(string name, Color color, Func<long, double> calculator)
        {
            _name = name;
            _color = color;
            _points = new RingArray<Point>(300);
            _calculator = calculator;
        }

        #endregion

        // --------------------------------------------- //
        #region "public API"

        public string Name
        {
            get { return _name; }
        }

        public Color Color
        {
            get { return _color; }
        }

        public bool IsVisible
        {
            get { return _linegraph.Visibility == Visibility.Visible; }
            set
            {
                var visibility = value ? Visibility.Visible : Visibility.Collapsed;
                _linegraph.Visibility = visibility;
                _linegraph.Description.LegendItem.Visibility = visibility;
            }
        }

        public double Calculate(long index)
        {
            return _calculator(index);
        }

        public void AddPoint(Point pnt)
        {
            _points.Add(pnt);
        }

        public void AddToPlotter(ChartPlotter plotter)
        {
            var ds = new EnumerableDataSource<Point>(_points);
            ds.SetXMapping(p => ((HorizontalDateTimeAxis)plotter.HorizontalAxis).ConvertToDouble(p.Time));
            ds.SetYMapping(p => p.Value);
            _linegraph = plotter.AddLineGraph(ds, _color, 2, _name);
        }

        #endregion
    }
}
