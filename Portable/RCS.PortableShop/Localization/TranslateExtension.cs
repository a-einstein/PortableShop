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

        public static object ProvideValue(string text)
        {
            if (text == null)
                return string.Empty;

            var resourceTypeInfo = typeof(Labels).GetTypeInfo();
            var resourceBasename = resourceTypeInfo.FullName;
            var resourceAssembly = resourceTypeInfo.Assembly;

            var resourceManager = new ResourceManager(resourceBasename, resourceAssembly);

            var translation = resourceManager.GetString(text, CultureInfo);

            if (translation == null)
            {
#if DEBUG
                throw new ArgumentException($"Key '{text}' was not found in resources '{resourceBasename}' for culture '{CultureInfo.Name}'", nameof(text));
#else
                translation = text; // HACK: returns the key, which GETS DISPLAYED TO THE USER
#endif
            }

            return translation;
        }

        private static CultureInfo cultureInfo;

        private static CultureInfo CultureInfo
        {
            get
            {
                // TODO Maybe change this to accommodate for intermediate language changes.
                if ((Device.OS == TargetPlatform.iOS || Device.OS == TargetPlatform.Android) && cultureInfo == null)
                {
                    cultureInfo = DependencyService.Get<ILocalize>().GetCurrentCultureInfo();
                }

                return cultureInfo;
            }
        }
    }
}
