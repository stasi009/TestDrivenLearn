using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Linq;

namespace MVVM.SKI
{
    /// <summary>
    /// pay respect to the video:
    /// http://www.youtube.com/watch?v=g53__vPihFY&feature=related
    /// which is a good demo for MVVM
    /// </summary>
    sealed class ViewModel : INotifyPropertyChanged
    {
        private readonly IDAL m_dal;

        public ViewModel()
        {
            m_dal = new MockMemoryDAL();
            this.Events = m_dal.GetEvents();

            m_saveCommand = new SaveCommand(this);
        }

        #region "events"

        private IEnumerable<Event> m_events;
        public IEnumerable<Event> Events
        {
            get { return m_events; }
            set
            {
                if (m_events != value)
                {
                    m_events = value;
                    NotifyPropertyChanged("Events");

                    this.CurrentEvent = m_events.First();
                }
            }
        }

        private Event m_currentEvent;
        public Event CurrentEvent
        {
            get { return m_currentEvent; }
            set
            {
                if (m_currentEvent != value)
                {
                    m_currentEvent = value;
                    NotifyPropertyChanged("CurrentEvent");

                    this.CurrentCompetitors = m_dal.GetCompetitors(m_currentEvent.EventId);
                }
            }
        }

        #endregion

        #region "competitors"

        private IEnumerable<Competitor> m_currentCompetitors;
        public IEnumerable<Competitor> CurrentCompetitors
        {
            get { return m_currentCompetitors; }
            set
            {
                if (m_currentCompetitors != value)
                {
                    m_currentCompetitors = value;
                    NotifyPropertyChanged("CurrentCompetitors");
                }
            }
        }

        private Competitor m_selectedCompetitor;
        public Competitor SelectedCompetitor
        {
            get { return m_selectedCompetitor; }
            set
            {
                if (m_selectedCompetitor != value)
                {
                    m_selectedCompetitor = value;
                    NotifyPropertyChanged("SelectedCompetitor");
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

        #region "commands"
        private readonly SaveCommand m_saveCommand;
        public SaveCommand SaveCommand
        {
            get { return m_saveCommand; }
        }
        #endregion

        public void SaveSelectedCompetitor()
        {
            m_dal.SaveCompetitor(m_selectedCompetitor);
        }
    }
}
