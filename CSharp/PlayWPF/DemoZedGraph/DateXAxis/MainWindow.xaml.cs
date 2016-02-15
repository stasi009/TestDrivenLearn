using System;
using System.Drawing;
using System.Windows;
using ZedGraph;

namespace DemoZedGraph.DateXAxis
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, IView
    {
        private PointPairList _points;
        private Presenter _presenter;

        public MainWindow()
        {
            InitializeComponent();
            _presenter = new Presenter(this);
            this.DataContext = _presenter;
        }

        public void Init()
        {
            GraphPane pane = zedGraphControl.GraphPane;
            pane.Title.Text = "Use Date As Axis";
            pane.YAxis.Title.Text = "Value";
            pane.Chart.Fill = new Fill(Color.LemonChiffon);
            pane.XAxis.Title.Text = "Time";
            pane.XAxis.Scale.FontSpec.Size /= 2;

            pane.XAxis.Type = AxisType.Date;
            pane.XAxis.Scale.Format = "mm:ss";
            pane.XAxis.Scale.MajorStep = 1;
            pane.XAxis.Scale.MajorUnit = DateUnit.Second;

            _points = new PointPairList();
            var line = pane.AddCurve("line", _points, Color.Crimson, SymbolType.Square);
            line.Symbol.Size /= 2;
        }

        public void DrawNext(DateTime x, double y)
        {
            _points.Add(x.ToOADate(), y);
            zedGraphControl.AxisChange();
            zedGraphControl.Invalidate();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            _presenter.Initialize();
        }
    }
}
