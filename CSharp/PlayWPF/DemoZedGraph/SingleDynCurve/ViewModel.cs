using System;
using System.Reactive.Linq;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;

namespace DemoZedGraph.SingleDynCurve
{
    sealed class ViewModel : ViewModelBase
    {
        // ------------------------------------------------ //
        #region "member fields"

        private readonly IView _view;
        private IDisposable _subscription;
        private DateTime _scaleMax;
        private double _majorStep;

        #endregion

        // ------------------------------------------------ //
        #region "bindable properties"

        private float _period;
        public float Period
        {
            get { return _period; }
            set
            {
                if (_period == value) return;
                _period = value;
                RaisePropertyChanged("Period");
            }
        }

        private float _length;
        public float Length
        {
            get { return _length; }
            set
            {
                if (_length == value) return;
                _length = value;
                RaisePropertyChanged("Length");
            }
        }

        public RelayCommand StartCmd { get; private set; }
        public RelayCommand StopCmd { get; private set; }

        #endregion

        // ------------------------------------------------ //
        #region "constructor"

        public ViewModel(IView view)
        {
            _view = view;

            StartCmd = new RelayCommand(Start, () => _subscription == null);
            StopCmd = new RelayCommand(Stop, () => _subscription != null);
        }

        #endregion

        // ------------------------------------------------ //
        #region "public API"

        public void OnLoad()
        {
            this.Period = 1;
            this.Length = 1;

            DateTime min = DateTime.Now;
            _scaleMax = min.AddMinutes(this.Length);

            _majorStep = 10;
            _view.Initialize(min, _scaleMax, _majorStep, new[] { "sin", "cos" });
        }

        #endregion

        // ------------------------------------------------ //
        #region "private helpers"

        private void Start()
        {
            _subscription =
                Observable.Interval(TimeSpan.FromSeconds(this.Period))
                .Timestamp()
                .Select(tindex =>
                            {
                                DateTime time = tindex.Timestamp.LocalDateTime;
                                long index = tindex.Value;
                                return new[]
                                           {
                                               new TimePoint("sin", time, Math.Sin(index)),
                                               new TimePoint("cos", time, Math.Cos(index)),
                                           };
                            })
                .ObserveOnDispatcher()
                .Subscribe(points =>
                               {
                                   DateTime time = points[0].Time.AddSeconds(_majorStep);
                                   if (time >= _scaleMax)
                                   {
                                       _scaleMax = time;
                                       var newMin = _scaleMax.AddMinutes(-this.Length);
                                       _view.Draw(points, true, newMin, _scaleMax);
                                   }
                                   else
                                   {
                                       _view.Draw(points, false, time, time);
                                   }
                               });
        }

        private void Stop()
        {
            _subscription.Dispose();
            _subscription = null;
        }

        #endregion

    }
}
