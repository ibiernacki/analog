using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace Views.Converters
{
    [ValueConversion(typeof(string), typeof(DateTime?))]
    public class StringToNullableDateTimeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is DateTime dateValue)
            {
                return dateValue.ToString("yyyy-MM-dd HH:mm:ss.fff");
            }

            return DependencyProperty.UnsetValue;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is string stringValue)
            {
                if (DateTime.TryParse(stringValue, CultureInfo.InvariantCulture, DateTimeStyles.None,out DateTime result))
                {
                    return result;
                }
            }

            return null;
        }
    }
}