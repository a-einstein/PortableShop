using RCS.PortableShop.Common.Styles;
using RCS.PortableShop.Common.Styles.Themes;
using RCS.PortableShop.Resources;
using System.Globalization;

namespace RCS.PortableShop.Model
{
    public static class Settings
    {
        #region Construction
        static Settings()
        {
            // TODO Evaluate this together with Theme.
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

        #region Theme
        // TODO Translate values.
        public static readonly List<Theme> Themes = Enum.GetValues<Theme>().Cast<Theme>().ToList();
        private const string themeKey = "Theme";

        private static Theme? theme;

        // Note this is non nullable.
        public static Theme Theme
        {
            get
            {
                if (!theme.HasValue)
                    theme = (Theme)Preferences.Get(themeKey, (int)Theme.Light);

                return theme.Value;
            }
            set
            {
                theme = value;
                Preferences.Set(themeKey, (int)value);

                // TODO Side-effect. Move?
                ApplyTheme();
            }
        }

        private static void ApplyTheme()
        {
            var mergedDictionaries = Application.Current.Resources.MergedDictionaries;

            if (mergedDictionaries != null)
            {
                mergedDictionaries.Clear();

                switch (Theme)
                {
                    case Theme.Dark:
                        mergedDictionaries.Add(new DarkTheme());
                        break;
                    case Theme.Light:
                    default:
                        mergedDictionaries.Add(new LightTheme());
                        break;
                }

                // TODO Preserve. Question is how in the Collection.
                mergedDictionaries.Add(new Stylesheet());
            }
        }
        #endregion

        #region ServiceType
        public static readonly List<ServiceType> ServiceTypes = Enum.GetValues<ServiceType>().Cast<ServiceType>().ToList();
        private const string serviceTypeKey = "ServiceType";

        private static ServiceType? serviceType;

        // Not entirely happy using Xamarin.Essentials here.
        // But need to initialize here for service calls not to fail, 
        // and do not want to ignore an already stored value.

        // Note this is non nullable.
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
        public static IList<CultureInfo> Cultures { get; } = new List<CultureInfo>()
        {
            new CultureInfo("en-GB", Labels.CultureEnglish),
            new CultureInfo("nl-NL", Labels.CultureDutch)
        };

        private const string cultureKey = "Culture";
        public static CultureInfo Culture
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

                // TODO Side-effect. Move?
                // Switch culture.
                // TODO Change on the fly.
                System.Globalization.CultureInfo.CurrentCulture =
                    System.Globalization.CultureInfo.DefaultThreadCurrentCulture =
                    System.Globalization.CultureInfo.DefaultThreadCurrentUICulture = new System.Globalization.CultureInfo(Culture.Name);
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
