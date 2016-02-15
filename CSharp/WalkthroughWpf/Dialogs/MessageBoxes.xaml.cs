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

namespace Dialogs
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class MessageBoxes : Window
    {
        private enum MsgBoxOpt
        {
            TitleMsg,
            ButtonTitleMsg,
            ImagBtnTitleMsg,
            DefaultImgBtnTitleMsg
        }

        public MessageBoxes()
        {
            InitializeComponent();

            FillMsgBoxOptions();
        }

        /// <summary>
        /// note: sample for operating ComboBox by code
        /// </summary>
        private void FillMsgBoxOptions()
        {
            Tuple<MsgBoxOpt, string>[] optAndTexts = 
            {
                Tuple.Create(MsgBoxOpt.TitleMsg,"Title and Message"),
                Tuple.Create(MsgBoxOpt.ButtonTitleMsg,"Tile,Message and Buttons"),
                Tuple.Create(MsgBoxOpt.ImagBtnTitleMsg,"Title,Message,Button and Image"),
                Tuple.Create(MsgBoxOpt.DefaultImgBtnTitleMsg,"Title,Message,Button,Image and Default result")
            };

            foreach (var optTxt in optAndTexts)
            {
                cmbOptions.Items.Add(new ComboBoxItem
                                         {
                                             Content = optTxt.Item2,
                                             Tag = optTxt.Item1,
                                         });
            }
            cmbOptions.SelectedIndex = 0;
        }

        private void btnShowMsgbox_Click(object sender, RoutedEventArgs e)
        {
            ComboBoxItem currentItem = cmbOptions.SelectedItem as ComboBoxItem;
            MsgBoxOpt opt = (MsgBoxOpt)currentItem.Tag;

            MessageBoxResult result = MessageBoxResult.None;
            switch (opt)
            {
                case MsgBoxOpt.TitleMsg:
                    result = MessageBox.Show("Message", "Title");
                    break;

                case MsgBoxOpt.ButtonTitleMsg:
                    result = MessageBox.Show("Message", "Title", MessageBoxButton.OKCancel);
                    break;

                case MsgBoxOpt.ImagBtnTitleMsg:
                    result = MessageBox.Show("Question?", "Title", MessageBoxButton.YesNoCancel, MessageBoxImage.Question);
                    break;

                case MsgBoxOpt.DefaultImgBtnTitleMsg:
                    result = MessageBox.Show("Question?", "Title", MessageBoxButton.YesNoCancel,
                                             MessageBoxImage.Question, MessageBoxResult.Cancel);
                    break;

                default:
                    throw new ArgumentOutOfRangeException("un-recognized MessageBox options");
            }// switch

            lblResult.Content = string.Format("Result = '{0}'", result);
        }// FillMsgBoxOptions
    }
}
