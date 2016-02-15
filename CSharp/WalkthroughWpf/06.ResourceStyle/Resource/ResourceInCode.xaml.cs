using System.Windows;
using System.Windows.Media;

namespace _06.ResourceStyle.Resource
{
    /// <summary>
    /// Interaction logic for ResourceInCode.xaml
    /// </summary>
    public partial class ResourceInCode : Window
    {
        public ResourceInCode()
        {
            InitializeComponent();

            // --------------------- add resources
            this.Resources["brush1"] = new SolidColorBrush(Colors.Red);
            this.Resources.Add("brush2", new SolidColorBrush(Colors.Blue));

            // --------------------- use resources
            btnOk.BorderBrush = (Brush)FindResource("brush1");
            // note: uses 'TryFindResource', then when reference the non-existed key
            // it will return null, if the property to be assigned can deal with null,
            // like below one, then no exception will be thrown, and it just makes
            // the control border-less
            btnCancel.BorderBrush = (Brush)TryFindResource("notExisted");
        }
    }
}
