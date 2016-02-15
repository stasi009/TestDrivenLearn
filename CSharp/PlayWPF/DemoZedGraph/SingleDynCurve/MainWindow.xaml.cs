using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows;
using ZedGraph;

namespace DemoZedGraph.SingleDynCurve
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, IView
    {
        private IDictionary<string, IPointListEdit> _mapName2Points;
        private ViewModel _viewModel;

        public MainWindow()
        {
            InitializeComponent();
            _mapName2Points = new Dictionary<string, IPointListEdit>();
            _viewModel = new ViewModel(this);
            this.DataContext = _viewModel;
        }

        public void Initialize(DateTime min, DateTime max, double majorStep, IEnumerable<string> names)
        {
            GraphPane pane = zedGraphControl.GraphPane;
            pane.Title.Text = "Realtime Curve Demo";
            pane.Chart.Fill = new Fill(Color.LemonChiffon);
            pane.YAxis.Title.Text = "Value";
            pane.YAxis.MajorGrid.IsVisible = true;

            pane.XAxis.Title.Text = "Time";
            pane.XAxis.Type = AxisType.Date;
            pane.XAxis.Scale.Format = "mm:ss";
            // pane.XAxis.Scale.FontSpec.Size /= 2;

            pane.XAxis.Scale.MinAuto = false;
            pane.XAxis.Scale.MinGrace = 0;
            pane.XAxis.Scale.MaxAuto = false;
            pane.XAxis.Scale.MaxGrace = 0;

            pane.XAxis.Scale.Min = min.ToOADate();
            pane.XAxis.Scale.Max = max.ToOADate();
            pane.XAxis.Scale.MajorStep = majorStep;
            pane.XAxis.Scale.MajorUnit = DateUnit.Second;
            pane.XAxis.Scale.MinorStep = 2;
            pane.XAxis.Scale.MinorUnit = DateUnit.Second;
            pane.XAxis.MajorGrid.IsVisible = true;

            int index = 0;
            foreach (string name in names)
            {
                ++index;
                var points = new RollingPointPairList(600);//todo: need revised
                var line = pane.AddCurve(name, points, Helpers.GetColor(index), SymbolType.Triangle);
                line.Symbol.Size /= 2;
                _mapName2Points.Add(name, points);
            }
        }

        public void Draw(IEnumerable<TimePoint> points, bool updateScale, DateTime newMin, DateTime newMax)
        {
            foreach (TimePoint tmpnt in points)
            {
                var pntCollection = _mapName2Points[tmpnt.Name];
                double time = tmpnt.Time.ToOADate();
                pntCollection.Add(time, tmpnt.Value);
            }

            if (updateScale)
            {
                var scale = zedGraphControl.GraphPane.XAxis.Scale;
                scale.Max = newMax.ToOADate();
                scale.Min = newMin.ToOADate();
            }

            zedGraphControl.AxisChange();
            zedGraphControl.Invalidate();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            _viewModel.OnLoad();
        }
    }
}
