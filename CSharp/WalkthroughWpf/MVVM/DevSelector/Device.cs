using System;
using System.ComponentModel;

namespace MVVM.DevSelector
{
    public enum DevType
    {
        Bus,
        Line,
        Generator,
        Load
    }

    public sealed class Device : INotifyPropertyChanged
    {
        #region "properties"

        private int m_startBus;
        public int StartBus
        {
            get { return m_startBus; }
            set
            {
                if (m_startBus != value)
                {
                    m_startBus = value;
                    NotifyPropertyChanged("StartBus");
                }
            }
        }

        private int m_endBus;
        public int EndBus
        {
            get { return m_endBus; }
            set
            {
                if (m_endBus != value)
                {
                    m_endBus = value;
                    NotifyPropertyChanged("EndBus");
                }
            }
        }

        private DevType m_devtype;
        public DevType DeviceType
        {
            get { return m_devtype; }
            set
            {
                if (m_devtype != value)
                {
                    m_devtype = value;
                    NotifyPropertyChanged("DeviceType");
                }
            }
        }

        #endregion

        #region "implement INotifyPropertyChanged"

        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion
    }
}
