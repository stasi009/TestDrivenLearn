using System;
using GalaSoft.MvvmLight.Command;

namespace DemoZedGraph.DateXAxis
{
    sealed class Presenter
    {
        // ******************************************** //
        #region "member fields"

        private int _seed;
        private readonly IView _view;
        public RelayCommand NextCmd { get; private set; }

        #endregion

        // ******************************************** //
        #region "constructor"

        public Presenter(IView view)
        {
            _view = view;
            _seed = 0;

            NextCmd = new RelayCommand(() =>
            {
                ++_seed;
                _view.DrawNext(DateTime.Now, Math.Sin((double)_seed * Math.PI / 15.0));
            });
        }

        #endregion

        // ******************************************** //
        #region "public API"

        public void Initialize()
        {
            _seed = 0;
            _view.Init();
        }

        #endregion
    }
}
