using System;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;

namespace MessageHelper
{
    public class ValueToBrushConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string input;
            try
            {
                DataGridCell dgc = (DataGridCell)value;
                System.Data.DataRowView rowView = (System.Data.DataRowView)dgc.DataContext;
                input = (string)rowView.Row.ItemArray[dgc.Column.DisplayIndex];
            }
            catch (InvalidCastException e)
            {
                return DependencyProperty.UnsetValue;
            }
            switch (input)
            {
                case "true": return Brushes.Red;
                case "false": return Brushes.Green;
                default: return DependencyProperty.UnsetValue;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}