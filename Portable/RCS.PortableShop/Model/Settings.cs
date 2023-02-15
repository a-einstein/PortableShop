using RCS.PortableShop.Resources;
using System.Globalization;

namespace RCS.PortableShop.Model
{
    #region Construction
    public static class Settings
    {
        static Settings()
        {
            SetCulture();
        }

        private static void SetCulture()
        {
            if (Preferences.ContainsKey(cultureKey))
            {
                // TODO Make more transparent.
                Culture = Culture;
            }
         }
        #endregion

        #region ServiceType
        public static readonly List<ServiceType> ServiceTypes = Enum.GetValues(typeof(ServiceType)).Cast<ServiceType>().ToList();
        private const string serviceTypeKey = "ServiceType";

        private static ServiceType? serviceType;

        // Not entirely happy using Xamarin.Essentials here.
        // But need to initialize here for service calls not to fail, 
        // and do not want to ignore an already stored value.

        // Note that this is non nullable.
        public static ServiceType ServiceType
        {
            get
            {
                if (!serviceType.HasValue)
                    serviceType = (ServiceType)Preferences.Get(serviceTypeKey, (int)ServiceType.WebApi);

                return serviceType.Value;
            }
            set
            {
                serviceType = value;
                Preferences.Set(serviceTypeKey, (int)value);
            }
        }
        #endregion

        #region Culture
        public static IList<Culture> Cultures { get; } = new List<Culture>()
        {
            new Culture(Labels.CultureEnglish, "en-GB"),
            new Culture(Labels.CultureDutch, "nl-NL")
        };

        private const string cultureKey = "Culture";
        public static Culture Culture
        {
            get
            {
                // Try to read stored name (or take first available name).
                var cultureName = Preferences.Get(cultureKey, Cultures.FirstOrDefault().Name);

                // Use matching culture.
                return Cultures.FirstOrDefault(element => element.Name == cultureName);
            }
            set
            {
                // Store value.
                Preferences.Set(cultureKey, value.Name);

                // Switch culture.
                // TODO Change on the fly.
                CultureInfo.CurrentCulture = 
                    CultureInfo.DefaultThreadCurrentCulture = 
                    CultureInfo.DefaultThreadCurrentUICulture = new CultureInfo(Culture.Name);
            }
        }
        #endregion

        #region ProductCategory
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
        #endregion

        #region ProductSubategory
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
        #endregion

        #region TextFilter
        private const string textFilterKey = "TextFilter";
        private static string textFilter;

        public static string TextFilter
        {
            get
            {
                return textFilter ?? (textFilter = Preferences.Get(textFilterKey, default(string)));
            }
            set
            {
                textFilter = value;
                Preferences.Set(textFilterKey, value);
            }
        }
        #endregion
    }
}
