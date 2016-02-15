using System.Windows;
using System.Windows.Controls;
using System.Diagnostics;

namespace _05.GridLayout
{
    internal enum MyEnum
    {
        Tag1,
        Tag2,
        Tag3
    }

    /// <summary>
    /// Interaction logic for SimpleBtnClickDemo.xaml
    /// </summary>
    public partial class SimpleBtnClickDemo : Window
    {
        public SimpleBtnClickDemo()
        {
            InitializeComponent();
        }

        private void OnBtnClick(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;
            tbxDisplayContent.Text = button.Content.ToString();

            Debug.Assert(button.Tag is MyEnum);
            MyEnum tag = (MyEnum)button.Tag;
            tbxDisplayTag.Text = tag.ToString();
        }
    }
}
