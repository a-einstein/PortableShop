using System.Globalization;

namespace RCS.PortableShop.Common.Extensions
{
    public interface IMultiValueConverter
    {
        object Convert(object[] values, Type targetType, object parameter, CultureInfo culture);
    }
}
