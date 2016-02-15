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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace _16.Navigation.PageFunction
{
    /// <summary>
    /// Interaction logic for CallFunctionPage.xaml
    /// </summary>
    public partial class CallFunctionPage : Page
    {
        private SolidColorBrush m_okBrush;
        private SolidColorBrush m_cancelBrush;

        public CallFunctionPage()
        {
            InitializeComponent();

            m_cancelBrush = new SolidColorBrush(Colors.Red);
            m_okBrush = new SolidColorBrush(Colors.Green);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            InputFunc func = new InputFunc();
            func.Return += this.OnFuncReturn;
            NavigationService.Navigate(func);
        }

        private void OnFuncReturn(object sender, ReturnEventArgs<string> evtargs)
        {
            SolidColorBrush brush = null;
            if (evtargs == null)
            {
                brush = m_cancelBrush;
                lblDisplay.Content = "Canceled";
            }
            else
            {
                brush = m_okBrush;
                lblDisplay.Content = evtargs.Result;
            }

            lblDisplay.BorderBrush = brush;
            lblDisplay.Foreground = brush;
        }
    }
}
