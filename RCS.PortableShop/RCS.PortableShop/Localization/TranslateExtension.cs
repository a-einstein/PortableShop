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

            const string resourceBasename = "RCS.PortableShop.Resources.Labels";
            ResourceManager resourceManager = new ResourceManager(resourceBasename, typeof(TranslateExtension).GetTypeInfo().Assembly);

            // Note this only got working with the resources in the same assembly. Otherwise crashed.
            var translation = resourceManager.GetString(Text, cultureInfo);

            if (translation == null)
            {
#if DEBUG
                throw new ArgumentException(String.Format("Key '{0}' was not found in resources '{1}' for culture '{2}'.", Text, resourceBasename, cultureInfo.Name), "Text");
#else
                translation = Text; // HACK: returns the key, which GETS DISPLAYED TO THE USER
#endif
            }
            return translation;
        }
    }
}
