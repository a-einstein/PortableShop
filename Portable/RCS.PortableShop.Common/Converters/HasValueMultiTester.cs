using System;
using System.Globalization;

namespace RCS.WpfShop.Common.Converters
{
    public class HasValueMultiTester : SingleDirectionMultiConverter
    {
        public override object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            bool hasValue = false;

            foreach (var item in values)
            {
                hasValue = hasValue || HasValueTester.HasValue(item);
            }       

            // Invert if any parameter is passed.
            var result = parameter != null
                ? !hasValue
                : hasValue;

            return result;
        }
    }
}
