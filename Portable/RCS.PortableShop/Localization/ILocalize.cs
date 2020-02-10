using System.Globalization;

namespace RCS.PortableShop.Localization
{
    public interface ILocalize
    {
        CultureInfo GetCurrentCultureInfo();

        CultureInfo GetFallbackCultureInfo(string dotnetLanguage);

        void SetLocale(CultureInfo cultureInfo);
    }
}
