using System;
using System.Globalization;

namespace RCS.PortableShop.Common.Converters
{
    /*
    public class CategoriesFormatter : SingleDirectionMultiConverter
    {
        public override object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            // Conversions to prevent exceptions.
            string category = values[0] as string;
            string subcategory = !String.IsNullOrEmpty(category) ? values[1] as string : null;
            string separator = !String.IsNullOrEmpty(category) && !String.IsNullOrEmpty(subcategory) ? "/" : null;

            return $"{category} {separator} {subcategory}";
        }
    }
    */
}
