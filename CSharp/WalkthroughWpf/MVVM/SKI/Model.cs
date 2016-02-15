
using System;
using System.ComponentModel;

namespace MVVM.SKI
{
    sealed class Event : INotifyPropertyChanged
    {
        #region "properties"

        private int m_eventId;
        public int EventId
        {
            get { return m_eventId; }
            set
            {
                if (m_eventId != value)
                {
                    m_eventId = value;
                    NotifyPropertyChanged("EventId");
                }
            }
        }

        private string m_name;
        public string Name
        {
            get { return m_name; }
            set
            {
                if (m_name == null || !m_name.Equals(value))
                {
                    m_name = value;
                    NotifyPropertyChanged("Name");
                }
            }
        }

        private DateTime m_time;
        public DateTime Time
        {
            get { return m_time; }
            set
            {
                if (m_time != value)
                {
                    m_time = value;
                    NotifyPropertyChanged("Time");
                }
            }
        }

        #endregion

        #region "notify"

        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion
    }

    sealed class Competitor : INotifyPropertyChanged
    {
        #region "properties"

        private int m_competitorId;
        public int CompetitorId
        {
            get { return m_competitorId; }
            set
            {
                if (m_competitorId != value)
                {
                    m_competitorId = value;
                    NotifyPropertyChanged("CompetitorId");
                }
            }
        }

        private int m_eventId;
        public int EventId
        {
            get { return m_eventId; }
            set
            {
                if (m_eventId != value)
                {
                    m_eventId = value;
                    NotifyPropertyChanged("EventId");
                }
            }
        }

        private string m_name;
        public string Name
        {
            get { return m_name; }
            set
            {
                if (m_name == null || !m_name.Equals(value))
                {
                    m_name = value;
                    NotifyPropertyChanged("Name");
                }
            }
        }

        private int m_birthYear;
        public int YearOfBirth
        {
            get { return m_birthYear; }
            set
            {
                if (m_birthYear != value)
                {
                    m_birthYear = value;
                    NotifyPropertyChanged("YearOfBirth");
                }
            }
        }

        #endregion

        #region "notify"

        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion
    }
}
