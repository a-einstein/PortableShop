using RCS.PortableShop.Resources;
using System;
using System.Globalization;
using System.Reflection;
using System.Resources;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace RCS.PortableShop.Localization
{
    [ContentProperty("Text")]
    // Exclude the 'Extension' suffix when using in xaml.
    public class TranslateExtension : IMarkupExtension
    {
        public string Text { get; set; }

        public object ProvideValue(IServiceProvider serviceProvider)
        {
            return ProvideValue(Text);
        }

        private static object ProvideValue(string key)
        {
            if (string.IsNullOrEmpty(key))
                return string.Empty;

            var resourceTypeInfo = typeof(Labels).GetTypeInfo();
            var resourceBasename = resourceTypeInfo.FullName;
            var resourceAssembly = resourceTypeInfo.Assembly;

            var resourceManager = new ResourceManager(resourceBasename, resourceAssembly);

            var translation = resourceManager.GetString(key, CultureInfo);

            if (string.IsNullOrEmpty(translation))
            {
                var fallbackCultureInfo = DependencyService.Get<ILocalize>().GetFallbackCultureInfo(CultureInfo.Name);
                translation = resourceManager.GetString(key, fallbackCultureInfo);

                if (string.IsNullOrEmpty(translation))
                    translation = key; // HACK: returns the key, which GETS DISPLAYED TO THE USER
            }

            return translation;
        }

        private static CultureInfo cultureInfo;

        private static CultureInfo CultureInfo
        {
            get
            {
                // TODO Maybe change this to accommodate for intermediate language changes.
                if (cultureInfo == null)
                {
                    switch (Device.RuntimePlatform)
                    {
                        case Device.iOS:
                        case Device.Android:
                            cultureInfo = DependencyService.Get<ILocalize>().GetCurrentCultureInfo();
                            break;
                    }
                }

                return cultureInfo;
            }
        }
    }
}
