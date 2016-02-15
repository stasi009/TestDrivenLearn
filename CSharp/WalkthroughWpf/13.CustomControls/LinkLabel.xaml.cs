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

namespace _13.CustomControls
{
    /// <summary>
    /// Interaction logic for LinkLabel.xaml
    /// </summary>
    public partial class LinkLabel : UserControl
    {
        // ********************************************************* //
        #region [ dependency property definition ]

        private static readonly DependencyProperty TextProperty =
            DependencyProperty.Register("Text", typeof(string), typeof(LinkLabel),
            new PropertyMetadata(new PropertyChangedCallback(OnTextChanged)));

        private static readonly DependencyProperty UriProperty =
            DependencyProperty.Register("Uri", typeof(string), typeof(LinkLabel),
            new PropertyMetadata(new PropertyChangedCallback(OnUriChanged)));

        #endregion

        // ********************************************************* //
        #region [ properties ]

        // note: to be used in WPF's complex but elegant property system, the property here has no backing field
        // but actually operates a dictionary, where the key is the static-defined dependency property
        public string Text
        {
            get { return (string)GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }

        public string Uri
        {
            get { return (string)GetValue(UriProperty); }
            set { SetValue(UriProperty, value); }
        }

        #endregion

        // ********************************************************* //
        #region [ property change callback ]

        /// <summary>
        /// update the content of the hyperlink
        /// </summary>
        private static void OnTextChanged(DependencyObject sender, DependencyPropertyChangedEventArgs evtargs)
        {
            LinkLabel lnklable = (LinkLabel)sender;
            lnklable.webLink.Inlines.Clear();
            lnklable.webLink.Inlines.Add(new Run(evtargs.NewValue.ToString()));
        }

        private static void OnUriChanged(DependencyObject sender, DependencyPropertyChangedEventArgs evtargs)
        {
            LinkLabel label = (LinkLabel)sender;
            try
            {
                Uri newUri = new Uri(evtargs.NewValue.ToString());
                label.webLink.NavigateUri = newUri;
                label.webLink.ToolTip = string.Format("Link to '{0}'", newUri);
            }
            catch (UriFormatException exp)
            {
                label.webLink.ToolTip = string.Format("{0}:{1}", exp.Message, evtargs.NewValue.ToString());
            }
        }

        #endregion

        // ********************************************************* //
        public LinkLabel()
        {
            InitializeComponent();
        }

        // bad idea to navigate in the UI code
        private void webLink_RequestNavigate(object sender, RequestNavigateEventArgs e)
        {
            System.Diagnostics.Process.Start(e.Uri.ToString());
        }
    }
}
