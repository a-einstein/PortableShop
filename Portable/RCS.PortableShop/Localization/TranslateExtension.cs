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
        readonly CultureInfo cultureInfo;

        public TranslateExtension()
        {
            if (Device.OS == TargetPlatform.iOS || Device.OS == TargetPlatform.Android)
            {
                cultureInfo = DependencyService.Get<ILocalize>().GetCurrentCultureInfo();
            }
        }

        public string Text { get; set; }

        public object ProvideValue(IServiceProvider serviceProvider)
        {
            if (Text == null)
                return string.Empty;

            var resourceTypeInfo = typeof(Labels).GetTypeInfo();
            var resourceBasename = resourceTypeInfo.FullName;
            var resourceAssembly = resourceTypeInfo.Assembly;

            var resourceManager = new ResourceManager(resourceBasename, resourceAssembly);

            var translation = resourceManager.GetString(Text, cultureInfo);

            if (translation == null)
            {
#if DEBUG
                throw new ArgumentException($"Key '{Text}' was not found in resources '{resourceBasename}' for culture '{cultureInfo.Name}'", nameof(Text));
#else
                translation = Text; // HACK: returns the key, which GETS DISPLAYED TO THE USER
#endif
            }

            return translation;
        }
    }
}
