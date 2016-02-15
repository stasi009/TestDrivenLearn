using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
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

namespace _12.BindToList
{
    /// <summary>
    /// Interaction logic for Group.xaml
    /// </summary>
    public partial class Group : Window
    {
        private readonly GroupStyle m_custGrpStyle;
        private readonly AgeRanger m_ageRange;

        public Group()
        {
            InitializeComponent();

            m_custGrpStyle = (GroupStyle)FindResource("customGrpStyle");
            m_ageRange = new AgeRanger();
        }

        private void GroupByProperty(PropertyGroupDescription groupOption, GroupStyle grpStyle)
        {
            PersonCollection persons = (PersonCollection)FindResource("persons");
            ICollectionView view = CollectionViewSource.GetDefaultView(persons);

            if (view.GroupDescriptions.Count == 0)
            {
                view.GroupDescriptions.Add(groupOption);
                lbxPersons.GroupStyle.Add(grpStyle);
            }
            else
            {
                view.GroupDescriptions.Clear();
                lbxPersons.GroupStyle.Clear();
            }
        }

        private void btnDefGroup_Click(object sender, RoutedEventArgs e)
        {
            GroupByProperty(new PropertyGroupDescription("Age"), GroupStyle.Default);
        }

        private void btnCustGroup_Click(object sender, RoutedEventArgs e)
        {
            GroupByProperty(new PropertyGroupDescription("Age"), m_custGrpStyle);
        }

        private void btnRangeGroup_Click(object sender, RoutedEventArgs e)
        {
            GroupByProperty(new PropertyGroupDescription("Age", m_ageRange), m_custGrpStyle);
        }
    }

    public sealed class AgeRanger : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (int)value < 50 ? "Under Half" : "Over Half";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
