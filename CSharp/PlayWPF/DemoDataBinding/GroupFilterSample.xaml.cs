using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Data;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;

namespace DemoDataBinding
{
    public sealed class ReverseConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            bool chosen = (bool)value;
            return !chosen;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    /// <summary>
    /// Interaction logic for GroupFilterSample.xaml
    /// </summary>
    public partial class GroupFilterSample : Window
    {
        // ---------------------------------------------- //
        #region "inner classes"

        private sealed class StudentViewModel : ViewModelBase
        {
            public string Name { get; private set; }
            public string Department { get; private set; }

            private bool _isSelect4Add;
            public bool IsSelect4Add
            {
                get { return _isSelect4Add; }
                set
                {
                    if (_isSelect4Add == value) return;
                    _isSelect4Add = value;
                    RaisePropertyChanged("IsSelect4Add");
                }
            }

            private bool _isSelect4Remove;
            public bool IsSelect4Remove
            {
                get { return _isSelect4Remove; }
                set
                {
                    if (_isSelect4Remove == value) return;
                    _isSelect4Remove = value;
                    RaisePropertyChanged("IsSelect4Remove");
                }
            }

            private bool _isChosen;
            public bool IsChosen
            {
                get { return _isChosen; }
                set
                {
                    if (_isChosen == value) return;
                    _isChosen = value;
                    RaisePropertyChanged("IsChosen");
                }
            }

            public StudentViewModel(string name, string department)
            {
                this.Name = name;
                this.Department = department;
            }
        }

        private sealed class MainViewModel : ViewModelBase
        {
            #region "member fields"

            private readonly StudentViewModel[] _allStudents;
            private readonly ICollectionView _viewAllStudents;

            private readonly ObservableCollection<StudentViewModel> _chosenStudents;
            private readonly ICollectionView _viewChosenStudents;

            private bool _selectAllToAdd = false;
            private bool _selectAllToRemove = false;

            #endregion

            #region "commands"

            public RelayCommand SelectCmd { get; private set; }
            /// <summary>
            /// note: it doesn't behave right when first filter and then select all
            /// it will select all the items, not just those being filtered out
            /// because we have no way to access the filter set of the ICollectionView
            /// so maybe the only way is we have to use ObservableCollection to hold
            /// the eligible items ourselves
            /// </summary>
            private void OnSelect()
            {
                foreach (var student in _allStudents)
                {
                    if (student.IsSelect4Add)
                    {
                        student.IsSelect4Add = false;
                        student.IsChosen = true;
                        _chosenStudents.Add(student);
                    }
                }
            }

            public RelayCommand DeselectCmd { get; private set; }
            private void OnDeselect()
            {
                var toremoveItems = (from item in this._chosenStudents
                                     where item.IsSelect4Remove
                                     select item).ToArray();
                foreach (var item in toremoveItems)
                {
                    item.IsChosen = false;
                    item.IsSelect4Remove = false;
                    _chosenStudents.Remove(item);
                }
            }

            public RelayCommand SelectAllToAddCmd { get; private set; }

            public RelayCommand SelectAllToRemoveCmd { get; private set; }

            #endregion

            #region "constructor"
            public MainViewModel()
            {
                _allStudents = new StudentViewModel[]
                                       {
                                           new StudentViewModel("Chuanlin","EE"),
                                           new StudentViewModel("Dave","CS"),
                                           new StudentViewModel("Tom","EE"),
                                           new StudentViewModel("Mary","Physics"), 
                                           new StudentViewModel("Dick","CS"),
                                           new StudentViewModel("Alice","Physics"), 
                                       };

                _viewAllStudents = CollectionViewSource.GetDefaultView(_allStudents);
                _viewAllStudents.GroupDescriptions.Add(new PropertyGroupDescription("Department"));
                _viewAllStudents.Filter = obj => Filter(obj, this.SearchTxt4Add);
                _viewAllStudents.SortDescriptions.Add(new SortDescription("Department", ListSortDirection.Ascending));

                _chosenStudents = new ObservableCollection<StudentViewModel>();
                _viewChosenStudents = CollectionViewSource.GetDefaultView(_chosenStudents);
                _viewChosenStudents.GroupDescriptions.Add(new PropertyGroupDescription("Department"));
                _viewChosenStudents.Filter = obj => Filter(obj, this.SearchTxt4Remove);
                _viewChosenStudents.SortDescriptions.Add(new SortDescription("Department", ListSortDirection.Ascending));

                SelectCmd = new RelayCommand(OnSelect);
                DeselectCmd = new RelayCommand(OnDeselect);

                SelectAllToAddCmd = new RelayCommand(() =>
                {
                    _selectAllToAdd = !_selectAllToAdd;
                    foreach (var student in _allStudents)
                    {
                        if (!student.IsChosen)
                        {
                            student.IsSelect4Add = _selectAllToAdd;
                        }
                    }
                });

                SelectAllToRemoveCmd = new RelayCommand(() =>
                {
                    _selectAllToRemove = !_selectAllToRemove;
                    foreach (var student in _chosenStudents)
                    {
                        student.IsSelect4Remove = _selectAllToRemove;
                    }
                });
            }



            #endregion

            #region "properties"

            public ICollectionView ViewAllStudents
            {
                get { return _viewAllStudents; }
            }

            public ICollectionView ViewChosenStudents
            {
                get { return _viewChosenStudents; }
            }

            private string _searchTxt4Add;
            public string SearchTxt4Add
            {
                get { return _searchTxt4Add; }
                set
                {
                    if (_searchTxt4Add == value) return;
                    _searchTxt4Add = value.ToLower();
                    RaisePropertyChanged("SearchTxt4Add");
                    _viewAllStudents.Refresh();
                }
            }

            private string _searchTxt4Remove;
            public string SearchTxt4Remove
            {
                get { return _searchTxt4Remove; }
                set
                {
                    if (_searchTxt4Remove == value) return;
                    _searchTxt4Remove = value;
                    RaisePropertyChanged("SearchTxt4Remove");
                    _viewChosenStudents.Refresh();
                }
            }

            #endregion

            #region "private helpers"

            private static bool Filter(object obj, string searchTxt)
            {
                var student = (StudentViewModel)obj;
                if (string.IsNullOrEmpty(searchTxt))
                {
                    return true;
                }
                else
                {
                    return student.Name.ToLower().Contains(searchTxt);
                }
            }

            #endregion
        }

        #endregion

        // ---------------------------------------------- //
        public GroupFilterSample()
        {
            InitializeComponent();
            this.DataContext = new MainViewModel();
        }
    }
}
