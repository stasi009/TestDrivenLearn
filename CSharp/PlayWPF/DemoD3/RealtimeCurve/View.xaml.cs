using System.Windows;

namespace DemoD3.RealtimeCurve
{
    /// <summary>
    /// Interaction logic for RealtimeCurve.xaml
    /// </summary>
    public partial class View : Window
    {
        public View()
        {
            InitializeComponent();
            this.DataContext = new ViewModel(plotter);
        }
    }
}
