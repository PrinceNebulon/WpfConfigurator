using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace WpfConfiguratorLib.editors.helpers
{
    public class GridLengthPercentageInverseConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var val = 100 - (double)value;
            var gridLength = new GridLength(val, GridUnitType.Star);

            return gridLength;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var val = (GridLength)value;

            return val.Value;
        }
    }
}