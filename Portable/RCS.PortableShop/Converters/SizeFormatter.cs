using RCS.PortableShop.Common.Extensions;
using System;
using System.Globalization;

namespace RCS.PortableShop.Converters
{
    public class SizeFormatter : IMultiValueConverter 
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            // Conversions to prevent exceptions.
            return $"{values[0] as string} {values[1] as string}";
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
