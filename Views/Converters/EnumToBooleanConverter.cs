using System;
using System.Globalization;
using System.Windows.Data;

namespace Views.Converters
{
    public class EnumToBooleanConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null) return null;
            return value.Equals(parameter);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null) return null;
            return value.Equals(true) 
                ? parameter 
                : Binding.DoNothing;
        }
    }
}