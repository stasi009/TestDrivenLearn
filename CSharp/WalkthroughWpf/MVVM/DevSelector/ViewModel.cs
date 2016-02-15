using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace MVVM.DevSelector
{
    sealed class ViewModel
    {
        // *************************************************************** //
        #region [ member fields ]

        private readonly ObservableCollection<Device> m_candidates;
        private readonly ObservableCollection<Device> m_selected;

        #endregion

        // *************************************************************** //
        #region [ constructor ]

        public ViewModel()
        {
            m_candidates = new ObservableCollection<Device>(Helper.Deserialize(Helper.FileCandidates));
            m_selected = new ObservableCollection<Device>(Helper.Deserialize(Helper.FileSelected));
        }

        #endregion

        // *************************************************************** //
        #region [ destructor ]
        #endregion

        // *************************************************************** //
        #region [ public APIs ]

        public ObservableCollection<Device> CandidateDevices
        {
            get { return m_candidates; }
        }

        public ObservableCollection<Device> SelectedDevices
        {
            get { return m_selected; }
        }

        #endregion

        // *************************************************************** //
        #region [ private helpers ]
        #endregion
    }
}
