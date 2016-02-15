using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace _06.ResourceStyle.Resource
{
    /// <summary>
    /// Interaction logic for DynamicResource.xaml
    /// </summary>
    public partial class DynamicResource : Window
    {
        private BrushConverter m_brushConverter;

        public DynamicResource()
        {
            InitializeComponent();
            m_brushConverter = new BrushConverter();

            // note: build dynamic resource by code
            btnDynResourceInCode.SetResourceReference(Button.ForegroundProperty,"myBrush");
        }

        private void OnButtonClicked(object sender, RoutedEventArgs e)
        {
            string strcolor = tbxColor.Text;

            if (string.IsNullOrEmpty(strcolor))
            {
                MessageBox.Show("Please input a color name", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            else
            {
                // note: when update the value of the resource
                // only the property built upon "DynamicResource" will be redraw
                // the property buit upon "StaticResource" will remain the same, no redraw
                // note: use "BrushConverter" to convert a name to color
                this.Resources["myBrush"] = m_brushConverter.ConvertFromString(strcolor) as SolidColorBrush;
            }
        }
    }
}
