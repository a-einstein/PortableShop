﻿using System.Globalization;
// TODO May use another.
using IMultiValueConverter = RCS.PortableShop.Common.Extensions.IMultiValueConverter;

namespace RCS.PortableShop.Common.Converters
{
    public abstract class SingleDirectionMultiConverter : IMultiValueConverter
    {
        public abstract object Convert(object[] values, Type targetType, object parameter, CultureInfo culture);

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
