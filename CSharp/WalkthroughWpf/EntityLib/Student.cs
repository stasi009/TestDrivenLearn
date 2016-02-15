using System.ComponentModel;

namespace EntityLib
{
    public sealed class Student : INotifyPropertyChanged
    {
        // *************************************************************** //
        #region [ event definition ]

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion

        // *************************************************************** //
        #region [ constructor ]
        #endregion

        // *************************************************************** //
        #region [ properties ]

        private int m_id;
        public int Id
        {
            get { return m_id; }
            set
            {
                if (m_id != value)
                {
                    m_id = value;
                    NotifyPropertyChanged("Id");
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

        private Gender m_gender;
        public Gender Gender
        {
            get { return m_gender; }
            set
            {
                if (m_gender != value)
                {
                    m_gender = value;
                    NotifyPropertyChanged("Gender");
                }
            }
        }

        private bool m_qualified;
        public bool IsQualified
        {
            get { return m_qualified; }
            set
            {
                if (m_qualified != value)
                {
                    m_qualified = value;
                    NotifyPropertyChanged("IsQualified");
                }
            }
        }

        private string m_notes;
        public string Notes
        {
            get { return m_notes; }
            set
            {
                if (m_notes == null || !m_notes.Equals(value))
                {
                    m_notes = value;
                    NotifyPropertyChanged("Notes");
                }
            }
        }

        #endregion

        // *************************************************************** //
        #region [ private helpers ]

        private void NotifyPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion

    }
}
