using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace WpfConfiguratorLib.view.editors.helpers
{
    public class GridLengthPercentageConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var val = (double)value;
            var gridLength = new GridLength(val, GridUnitType.Star);

            if (val > 50)
            {
                var x = 5;
            }

            return gridLength;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var val = (GridLength)value;

            return val.Value;
        }
    }
}