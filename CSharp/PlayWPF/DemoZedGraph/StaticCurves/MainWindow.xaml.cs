using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows;
using GalaSoft.MvvmLight.Command;
using ZedGraph;

namespace DemoZedGraph.StaticCurves
{
    interface IStaticCurves
    {
        void Init(IEnumerable<Curve> curves);
        void Draw(IEnumerable<Points> points);
    }

    internal sealed class StaticCurvesPresenter
    {
        private const int Size = 36;

        private IStaticCurves _view;
        private int _index;

        public StaticCurvesPresenter(IStaticCurves view)
        {
            _view = view;
            _index = 0;
            StartCmd = new RelayCommand(this.Draw);
        }

        public RelayCommand StartCmd { get; private set; }

        public void OnLoaded()
        {
            Curve[] curves = new[]
            {
                new Curve{Name="Porsche",Color = Color.Red,SymbolType = SymbolType.Diamond}, 
                new Curve{Name = "Piper",Color = Color.Blue,SymbolType = SymbolType.Circle}, 
            };
            _view.Init(curves);
        }

        public void Draw()
        {
            Points[] points = new Points[]
                                  {
                                      new Points("Porsche",Size),
                                      new Points("Piper",Size), 
                                  };

            for (int i = 0; i < Size; i++)
            {
                _index++;
                double x = (double)_index + 5;
                double y1 = 1.5 + Math.Sin((double)_index * 0.2);
                double y2 = 3.0 * (1.5 + Math.Sin((double)_index * 0.2));

                points[0].Set(i, x, y1);
                points[1].Set(i, x, y2);
            }

            _view.Draw(points);
        }
    }

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, IStaticCurves
    {
        private StaticCurvesPresenter _presenter;
        private IDictionary<string, PointPairList> _pointLists;

        public MainWindow()
        {
            InitializeComponent();
            _presenter = new StaticCurvesPresenter(this);
            _pointLists = new Dictionary<string, PointPairList>();
            this.DataContext = _presenter;
        }

        public void Init(IEnumerable<Curve> curves)
        {
            GraphPane pane = zedGraphControl.GraphPane;
            pane.Title.Text = "ZedGraph Static Curves";
            pane.Chart.Fill = new Fill(Color.LemonChiffon);
            pane.XAxis.Title.Text = "X Axis";
            pane.YAxis.Title.Text = "Y Axis";

            foreach (var curve in curves)
            {
                AddCurve(curve);
            }
        }

        public void Draw(IEnumerable<Points> points)
        {
            foreach (Points line in points)
            {
                var pointlist = _pointLists[line.Name];
                pointlist.Add(line.Xdatas, line.Ydatas);
            }
            zedGraphControl.AxisChange();
            zedGraphControl.Invalidate();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            _presenter.OnLoaded();
        }

        private LineItem AddCurve(Curve curve)
        {
            var pointlist = new PointPairList();
            _pointLists.Add(curve.Name, pointlist);

            var line = zedGraphControl.GraphPane.AddCurve(curve.Name, pointlist, curve.Color, curve.SymbolType);
            line.Symbol.Size = 2;
            line.Symbol.Fill = new Fill(curve.Color);

            return line;
        }
    }
}
