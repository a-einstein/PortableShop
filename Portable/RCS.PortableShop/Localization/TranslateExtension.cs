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

        public static object ProvideValue(string key)
        {
            if (string.IsNullOrEmpty(key))
                return string.Empty;

            var resourceTypeInfo = typeof(Labels).GetTypeInfo();
            var resourceBasename = resourceTypeInfo.FullName;
            var resourceAssembly = resourceTypeInfo.Assembly;

            var resourceManager = new ResourceManager(resourceBasename, resourceAssembly);

            var translation = resourceManager.GetString(key, CultureInfo);

            // Note there is already a fallback mechanism in .net which used to be set as below (optional UltimateResourceFallbackLocation):
            // [assembly: NeutralResourcesLanguage("en-US", UltimateResourceFallbackLocation.Satellite)]
            // Nowadays it can also be set as NeutralLanguage in project properties.
            // See this (old) documentation:
            // https://docs.microsoft.com/en-us/previous-versions/visualstudio/visual-studio-2008/sb6a8618%28v%3dvs.90%29
            // I had to move from my previous setup with an empty base resource to one with an English base as the above settings did not work out.

            // So here it is more likely that the key is not assigned.

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
