using System;
using System.Collections.Generic;
using System.Linq;
using Xamarin.Essentials;

namespace RCS.PortableShop.Model
{
    public static class Settings
    {
        public enum ServiceType
        {
            WCF,
            WebApi
        }

        public static List<ServiceType> ServiceTypes { get; } = Enum.GetValues(typeof(ServiceType)).Cast<ServiceType>().ToList();

        private const string serviceTypeKey = "ServiceType";
        private static ServiceType? serviceTypeSelected;

        // Not entirely happy using Xamarin.Essentials here.
        // But need to initialize here for service calls not to fail, 
        // and do not want to ignore an already stored value.

        // Note that this is non nullable.
        public static ServiceType ServiceTypeSelected
        {
            get
            {
                if (!serviceTypeSelected.HasValue)
                    serviceTypeSelected = (ServiceType)Preferences.Get(serviceTypeKey, (int)ServiceType.WebApi);

                return serviceTypeSelected.Value;
            }
            set
            {
                serviceTypeSelected = value;
                Preferences.Set(serviceTypeKey, (int)value);
            }
        }

        private const string productCategoryIdKey = "ProductCategoryId";
        private static int? productCategoryId;
        public static int? ProductCategoryId
        {
            get
            {
                if (!productCategoryId.HasValue)
                {
                    var retrievedValue = Preferences.Get(productCategoryIdKey, default(int));

                    if (retrievedValue != default)
                        productCategoryId = retrievedValue;
                    else
                        productCategoryId = null;
                }

                return productCategoryId;
            }
            set
            {
                productCategoryId = value;
                Preferences.Set(productCategoryIdKey, value ?? default);
            }
        }

        private const string productSubategoryIdKey = "ProductSubategoryId";
        private static int? productSubategoryId;
        public static int? ProductSubategoryId
        {
            get
            {
                if (!productSubategoryId.HasValue)
                {
                    var retrievedValue = Preferences.Get(productSubategoryIdKey, default(int));

                    if (retrievedValue != default)
                        productSubategoryId = retrievedValue;
                    else
                        productSubategoryId = null;
                }

                return productSubategoryId;
            }
            set
            {
                productSubategoryId = value;
                Preferences.Set(productSubategoryIdKey, value ?? default);
            }
        }

        private const string textFilterKey = "TextFilter";
        private static string textFilter;
        public static string TextFilter
        {
            get { return textFilter ??= Preferences.Get(textFilterKey, default(string)); }
            set
            {
                textFilter = value;
                Preferences.Set(textFilterKey, value);
            }
        }
    }
}
