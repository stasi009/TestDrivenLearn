using System.Windows;

namespace DemoXaml
{
    /// <summary>
    /// Interaction logic for xStatic.xaml
    /// </summary>
    public partial class WinxStatic : Window
    {
        public static readonly string WinTitle = "Demo x:Static";
        public static readonly string ShowText = "x:Static is to access Static Variable in Code-Behind";

        public WinxStatic()
        {
            InitializeComponent();
        }
    }
}
