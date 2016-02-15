using System.ComponentModel;
using System.Windows.Input;

namespace MVVM.Birthday
{
    sealed class PersonViewModel : INotifyPropertyChanged
    {
        #region [ member variables ]

        public event PropertyChangedEventHandler PropertyChanged;

        private readonly Person m_person;

        #endregion

        #region [ constructor ]

        public PersonViewModel()
        {
            m_person = new Person();
        }

        #endregion

        #region [ properties ]

        public string Name
        {
            get { return m_person.Name; }
            set
            {
                if (m_person.Name == null || !m_person.Name.Equals(value))
                {
                    m_person.Name = value;
                    NotifyPropertyChanged("Name");
                }
            }
        }

        public int Age
        {
            get { return m_person.Age; }
            set
            {
                if (m_person.Age != value)
                {
                    m_person.Age = value;
                    NotifyPropertyChanged("Age");
                }
            }
        }

        public ICommand AgeAddCommand
        {
            get { return new AddAgeCommand(this); }
        }

        #endregion

        #region [ private helpers ]

        private void NotifyPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion
    }
}
