using System;
using System.Globalization;
using System.Linq;

namespace RCS.PortableShop.Common.Converters
{
    public class HasValueMultiTester : SingleDirectionMultiConverter
    {
        public override object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            var hasValue = HasValue(values);

            // Invert if any parameter is passed.
            var result = parameter != null
                ? !hasValue
                : hasValue;

            return result;
        }

        private static bool HasValue(object[] values)
        {
            return values.Aggregate(false, (current, item) => current || HasValueTester.HasValue(item));
        }
    }
}
