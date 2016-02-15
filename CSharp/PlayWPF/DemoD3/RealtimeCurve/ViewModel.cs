using System;
using System.Collections.Generic;
using System.Globalization;
using System.Reactive.Linq;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using Microsoft.Research.DynamicDataDisplay;

namespace DemoD3.RealtimeCurve
{
    internal sealed class CanCalculate2TxtConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            bool canCalculate = (bool)value;
            return canCalculate ? "Start" : "Stop";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    sealed class ViewModel : ViewModelBase
    {
        // *********************************************** //
        #region "member fields"

        private readonly Curve[] _curves;
        private readonly RelayCommand _startStopCmd;
        private IDisposable _calculationHandle;

        #endregion

        // *********************************************** //
        public ViewModel(ChartPlotter plotter)
        {
            _curves = new Curve[]
                          {
                              new Curve("sin",Colors.Red,index=>Math.Sin(0.1 * index)), 
                              new Curve("cos",Colors.Blue,index=>2*Math.Cos(0.3 * index)), 
                          };

            foreach (var curve in _curves)
            {
                curve.AddToPlotter(plotter);
            }

            CanCalculate = true;

            _startStopCmd = new RelayCommand(StartStopCalculate);
        }

        // *********************************************** //
        public IEnumerable<Curve> Curves
        {
            get { return _curves; }
        }

        private bool _canCalculate;
        public bool CanCalculate
        {
            get { return _canCalculate; }
            set
            {
                if (_canCalculate == value) return;
                _canCalculate = value;
                RaisePropertyChanged("CanCalculate");
            }
        }

        public ICommand StartStopCalculateCmd
        {
            get { return _startStopCmd; }
        }

        // *********************************************** //
        private void StartStopCalculate()
        {
            if (CanCalculate)
            {
                var timerstream = Observable.Interval(TimeSpan.FromMilliseconds(100)).Timestamp().Publish();

                foreach (var temp in Curves)
                {
                    Curve curve = temp;
                    timerstream
                        .Select(tindex =>
                            new Point(tindex.Timestamp.LocalDateTime, curve.Calculate(tindex.Value)))
                        .ObserveOnDispatcher()
                        .Subscribe(curve.AddPoint);
                }

                _calculationHandle = timerstream.Connect();
                CanCalculate = false;
            }
            else
            {
                _calculationHandle.Dispose();
                CanCalculate = true;
            }
        }

    }// MainViewModel
}
