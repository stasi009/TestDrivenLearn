using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Messaging;
using Utility;

namespace DemoMessage
{
    /// <summary>
    /// Interaction logic for SyncWithMessage.xaml
    /// </summary>
    public partial class SyncWithMessage : Window
    {
        // ******************************************* //
        #region "ViewModel"

        private sealed class ViewModel : ViewModelBase
        {
            public CheckRecordViewModel[] All { get; private set; }
            public ObservableCollection<CheckRecordViewModel> Selected { get; private set; }

            public ViewModel()
            {
                var names = new[] { "Tom", "Dick", "Mary" };
                All = (from name in names
                       select new CheckRecordViewModel(name)).ToArray();

                Selected = new ObservableCollection<CheckRecordViewModel>();

                Messenger.Default.Register<CheckRecordViewModel>(this, record =>
                {
                    if (record.IsSelected)
                    {
                        Selected.Add(record);
                    }
                    else
                    {
                        Selected.Remove(record);
                    }
                });
            }
        }

        #endregion

        // ******************************************* //
        public SyncWithMessage()
        {
            InitializeComponent();
            this.DataContext = new ViewModel();
        }
    }
}
