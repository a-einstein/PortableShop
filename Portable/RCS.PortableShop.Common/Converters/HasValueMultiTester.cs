using System;
using System.Globalization;

namespace RCS.PortableShop.Common.Converters
{
    public class HasValueMultiTester : SingleDirectionMultiConverter
    {
        public override object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            bool hasValue = HasValue(values);

            // Invert if any parameter is passed.
            var result = parameter != null
                ? !hasValue
                : hasValue;

            return result;
        }

        public static bool HasValue(object[] values)
        {
            bool hasValue = false;

            foreach (var item in values)
            {
                hasValue = hasValue || HasValueTester.HasValue(item);
            }

            return hasValue;
        }
    }
}
