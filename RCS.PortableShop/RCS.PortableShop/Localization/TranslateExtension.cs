using System;
using System.Globalization;
using System.Reflection;
using System.Resources;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace RCS.PortableShop.Localization
{
    // Exclude the 'Extension' suffix when using in Xaml markup.
    [ContentProperty("Text")]
    public class TranslateExtension : IMarkupExtension
    {
        readonly CultureInfo cultureInfo;

        const string resourceBasename = "RCS.PortableShop.Resources.Labels";
        string resourceManifestname = $"{resourceBasename}.resources";

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
                return "";

            ResourceManager resourceManager = new ResourceManager(resourceBasename, typeof(TranslateExtension).GetTypeInfo().Assembly);

            // TODO >> Crashes here. Maybe irrelevant, but it mentions <resourceBasename>.resources, which is an Id in XamarinStudio. Changing that does not make a difference.
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
