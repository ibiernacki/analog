using System;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Windows.Data;

namespace Views.Converters
{
    public class EnumDescriptionConverter : IValueConverter
    {
        object IValueConverter.Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var myEnum = (Enum) value;
            var description = GetEnumDescription(myEnum);
            return description;
        }

        object IValueConverter.ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return string.Empty;
        }

        private static string GetEnumDescription(Enum enumObject)
        {
            var fieldInfo = enumObject
                .GetType()
                .GetField(enumObject.ToString());

            var attributes = fieldInfo.GetCustomAttributes(false);

            if (!attributes.Any())
            {
                return enumObject.ToString();
            }

            var attrib = attributes.First() as DescriptionAttribute;

            return attrib == null 
                ? string.Empty 
                : attrib.Description;
        }
    }
}