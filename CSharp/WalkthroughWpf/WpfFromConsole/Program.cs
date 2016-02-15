
using System;
using System.Windows;

namespace WpfFromConsole
{
    sealed class MyApp : Application
    {
        // ***************************************************** //
        #region "constructor"

        public MyApp()
        {
            this.Startup += this.OnAppStartup;
        }

        #endregion

        // ***************************************************** //
        #region "event callbacks"

        private void OnAppStartup(object sender, StartupEventArgs evtargs)
        {
            Console.WriteLine("application starts up.");

            // note: by default, when all top-level windows closes, the application shuts down
            Window window = new Window { Title = "Created from console" };
            window.Show();
        }

        #endregion

        // ***************************************************** //
        #region "override methods"

        protected override void OnExit(ExitEventArgs e)
        {
            base.OnExit(e);
            Console.WriteLine("application exits.");
        }

        #endregion

        // ***************************************************** //
        #region "main function"
        [STAThread]
        static void Main(string[] args)
        {
            MyApp app = new MyApp();
            app.Run();
        }
        #endregion
    }
}
