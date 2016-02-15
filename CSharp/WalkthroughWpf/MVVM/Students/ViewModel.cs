
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows.Data;
using System.Windows.Input;
using EntityLib;

namespace MVVM.Students
{
    /// <summary>
    /// chekanote: drop the original solution which bind event handler to CollectionView's CurrentChanged event
    /// because I find that when cooperating with DataGrid, that DataGrid always have one extra row
    /// which represents the new,future row which will be added into the collection later
    /// when click on that extra, new, future row, CollectionView's CurrentChanged will not fire, and current
    /// item inside the CollectionView remain the same, which make the "Remove" button still enabled
    /// so I have to use the event handler of the DataGrid's SelectionChanged event
    /// when attaching the "DataGrid.SelectionChanged", clicking on that extra, new, future row
    /// the background item representing that row cannot be cast into "Student" class, which can use as a sign
    /// to differentiate from other ordinary rows, which can walk around CollectionView's limitation
    /// </summary>
    sealed class ViewModel : INotifyPropertyChanged
    {
        // *************************************************************** //
        #region [ member fields ]

        public event PropertyChangedEventHandler PropertyChanged;

        private readonly string[] m_genderFilterOptions;
        private readonly Archive m_archive;

        private readonly ObservableCollection<Student> m_students;
        private readonly ICollectionView m_view;

        private readonly SaveCommand m_saveCommand;
        private readonly RemoveCommand m_removeCommand;

        #endregion

        // *************************************************************** //
        #region [ constructor ]

        public ViewModel()
        {
            m_genderFilterOptions = new string[] { "All", "Male", "Female" };

            m_archive = new Archive("students.xml");

            IEnumerable<Student> initStudents = m_archive.Load();
            m_students = initStudents == null ?
                new ObservableCollection<Student>() : new ObservableCollection<Student>(initStudents);

            m_view = CollectionViewSource.GetDefaultView(m_students);

            m_saveCommand = new SaveCommand(this);
            m_removeCommand = new RemoveCommand(this);

            this.CurrentSelectGender = "All";
        }

        #endregion

        // *************************************************************** //
        #region [ properties ]

        public string[] GenderFilterOptions
        {
            get { return m_genderFilterOptions; }
        }

        private string m_currentSelectGender;
        public string CurrentSelectGender
        {
            get { return m_currentSelectGender; }
            set
            {
                if (m_currentSelectGender == null || !m_currentSelectGender.Equals(value))
                {
                    m_currentSelectGender = value;
                    NotifyPropertyChanged("CurrentSelectGender");

                    if (m_currentSelectGender.Equals("All"))
                        m_view.Filter = null;
                    else
                    {
                        Gender selectGender = (Gender)Enum.Parse(typeof(Gender), m_currentSelectGender);
                        m_view.Filter = obj =>
                                            {
                                                Student student = (Student)obj;
                                                return student.Gender == selectGender;
                                            };
                    }
                }
            }
        }

        public ObservableCollection<Student> Students
        {
            get { return m_students; }
        }

        public ICommand SaveCommand
        {
            get { return m_saveCommand; }
        }

        public ICommand RemoveCommand
        {
            get { return m_removeCommand; }
        }

        #endregion

        // *************************************************************** //
        #region [ methods ]

        public void Save()
        {
            m_archive.Save(m_students.ToArray());
        }

        public void Remove(Student student)
        {
            m_students.Remove(student);
        }

        public void OnSelectionChanged()
        {
            m_removeCommand.FireCanExecuteChanged();
        }

        private void NotifyPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion
    }
}
