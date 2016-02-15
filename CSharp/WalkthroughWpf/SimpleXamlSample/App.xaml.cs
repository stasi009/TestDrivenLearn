using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Windows;

namespace SimpleXamlSample
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private void OnAppStartupCallback(object sender, StartupEventArgs e)
        {
            // note: this event fires before showing the main window
            MessageBox.Show("Application starts up, press OK then show the main window");
        }
    }
}
