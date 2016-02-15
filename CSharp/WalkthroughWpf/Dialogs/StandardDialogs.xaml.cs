using System.Windows;
using System.Windows.Controls;
using Microsoft.Win32;

namespace Dialogs
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private OpenFileDialog m_dlgOpenFile; // in "Microsoft.Win32"
        private SaveFileDialog m_dlgSaveFile; // in "Microsoft.Win32"
        private PrintDialog m_dlgPrint; // in "System.Windows.Controls"

        public MainWindow()
        {
            InitializeComponent();
        }

        private static void InitFileDialog(FileDialog dlg)
        {
            dlg.FileName = "default csharp";
            dlg.DefaultExt = ".cs";
            dlg.Filter = "C#(.cs)|*.cs";
            dlg.InitialDirectory = @"D:\study\programming\CSharp\WPF\WalkthroughWpf\Dialogs";
        }

        private static void FillLabelWithFilePath(Label label, FileDialog dlg)
        {
            label.Content = dlg.ShowDialog() == true ? dlg.FileName : "Non Selected";
        }

        /// <summary>
        /// note: OpenFileDialog has the built-in function which checks that the file need to be opened
        /// must already exist in the disk, otherwise it will popup a warning to remind the user
        /// that the specified filename doesn't exist
        /// </summary>
        private void btnOpenFile_Click(object sender, RoutedEventArgs e)
        {
            if (m_dlgOpenFile == null)
            {
                m_dlgOpenFile = new OpenFileDialog();
                InitFileDialog(m_dlgOpenFile);
            }
            FillLabelWithFilePath(lblOpenFile, m_dlgOpenFile);
        }

        /// <summary>
        /// note: SaveFileDialog has its built-in function to remind the user that
        /// the selected file has already existed, and ask the user whether to overwrite or not
        /// and SaveFileDialog always support a new filename which hasn't existed in the disk
        /// </summary>
        private void btnSaveFile_Click(object sender, RoutedEventArgs e)
        {
            if (m_dlgSaveFile == null)
            {
                m_dlgSaveFile = new SaveFileDialog();
                InitFileDialog(m_dlgSaveFile);
            }
            FillLabelWithFilePath(lblSaveFile, m_dlgSaveFile);
        }

        private void btnPrint_Click(object sender, RoutedEventArgs e)
        {
            if (m_dlgPrint == null)
            {
                m_dlgPrint = new PrintDialog
                                 {
                                     PageRangeSelection = PageRangeSelection.AllPages,
                                     UserPageRangeEnabled = true
                                 };
            }
            lblPrint.Content = m_dlgPrint.ShowDialog() == true ? "Printing,..." : "No Print";
        }

        private void btnBrowseFolder_Click(object sender, RoutedEventArgs e)
        {
            // note: we are using a dialog from WinForm
            // this dialog is implemented in assembly "System.Windows.Form"
            // and we cannot import the namespace of "System.Windows.Forms" at the header of this file
            // which will cause naming conflict
            using (var dlg = new System.Windows.Forms.FolderBrowserDialog
                          {
                              SelectedPath = @"D:\study\programming" // set default selected path
                          })
            {
                lblFolder.Content = dlg.ShowDialog() == System.Windows.Forms.DialogResult.OK
                                        ? dlg.SelectedPath
                                        : "Canceled";
            }// note: the dialog must be disposed to free resources
        }
    }
}
