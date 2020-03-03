using RCS.PortableShop.Localization;
using System;
using System.Globalization;
using System.Threading;
using Xamarin.Forms;

[assembly: Dependency(typeof(RCS.PortableShop.Droid.Localization.Localize))]

namespace RCS.PortableShop.Droid.Localization
{
    // Based on https://github.com/xamarin/xamarin-forms-samples/blob/master/TodoLocalized/TodoLocalized.Android/Locale_Android.cs
    public class Localize : ILocalize
    {
        private const string debugPrefix = ">>>> Debug:";

        public void SetLocale(CultureInfo cultureInfo)
        {
            Thread.CurrentThread.CurrentCulture = cultureInfo;
            Thread.CurrentThread.CurrentUICulture = cultureInfo;

            Console.WriteLine($"{debugPrefix} CurrentCulture set to '{cultureInfo.Name}'");
        }

        public CultureInfo GetCurrentCultureInfo()
        {
            var androidLocale = Java.Util.Locale.Default;
            var dotnetLanguage = AndroidToDotnetLanguage(androidLocale.ToString());

            // this gets called a lot - try/catch can be expensive so consider caching or something
            CultureInfo cultureInfo;

            try
            {
                cultureInfo = new CultureInfo(dotnetLanguage);
            }
            catch (CultureNotFoundException exception)
            {
                Console.WriteLine($"{debugPrefix} Setting {nameof(cultureInfo)} to '{dotnetLanguage}' failed. ({exception.Message})");
                cultureInfo = GetFallbackCultureInfo(dotnetLanguage);
            }

            return cultureInfo;
        }

        private static string AndroidToDotnetLanguage(string androidLanguage)
        {
            Console.WriteLine($"{debugPrefix} Android Language: {androidLanguage}");

            // See some background here:
            // https://docs.microsoft.com/en-us/xamarin/android/app-fundamentals/localization

            var dotnetLanguage = androidLanguage.Replace("_", "-");

            //certain languages need to be converted to CultureInfo equivalent
            switch (androidLanguage)
            {
                case "ms-BN":   // "Malaysian (Brunei)" not supported .NET culture
                case "ms-MY":   // "Malaysian (Malaysia)" not supported .NET culture
                case "ms-SG":   // "Malaysian (Singapore)" not supported .NET culture
                    dotnetLanguage = "ms"; // closest supported
                    break;
                case "in-ID":  // "Indonesian (Indonesia)" has different code in  .NET 
                    dotnetLanguage = "id-ID"; // correct code for .NET
                    break;
                case "gsw-CH":  // "Schwiizertüütsch (Swiss German)" not supported .NET culture
                    dotnetLanguage = "de-CH"; // closest supported
                    break;
                    // add more application-specific cases here (if required)
                    // ONLY use cultures that have been tested and known to work
            }

            Console.WriteLine($"{debugPrefix} .NET Language/Locale: {dotnetLanguage}");

            return dotnetLanguage;
        }

        public CultureInfo GetFallbackCultureInfo(string dotnetLanguage)
        {
            CultureInfo cultureInfo;
            const string englishCode = "en";

            var fallback = DotnetFallbackLanguage(new PlatformCulture(dotnetLanguage));

            try
            {
                Console.WriteLine($"{debugPrefix} Trying {fallback}.");
                cultureInfo = new CultureInfo(fallback);
            }
            catch (CultureNotFoundException exception)
            {
                // iOS language not valid .NET culture, falling back to English
                Console.WriteLine($"{debugPrefix} {fallback} couldn't be set, using '{englishCode}'. ({exception.Message})");
                cultureInfo = new CultureInfo(englishCode);
            }

            return cultureInfo;
        }

        private static string DotnetFallbackLanguage(PlatformCulture platformCulture)
        {
            // iOS locale not valid .NET culture (eg. "en-ES" : English in Spain)
            // fallback to first characters, in this case "en".
            Console.WriteLine($"{debugPrefix} .NET Fallback Language: {platformCulture.LanguageCode}");
            var dotnetLanguage = platformCulture.LanguageCode; // use the first part of the identifier (two chars, usually);

            switch (platformCulture.LanguageCode)
            {
                case "gsw":
                    dotnetLanguage = "de-CH"; // equivalent to German (Switzerland) for this app
                    break;
                    // add more application-specific cases here (if required)
                    // ONLY use cultures that have been tested and known to work
            }

            Console.WriteLine($"{debugPrefix} .NET Fallback Language/Locale: {dotnetLanguage} (application-specific)");

            return dotnetLanguage;
        }
    }
}