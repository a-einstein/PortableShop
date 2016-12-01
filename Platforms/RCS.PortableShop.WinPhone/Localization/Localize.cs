using RCS.PortableShop.Localization;
using System.Globalization;
using Xamarin.Forms;

[assembly: Dependency(typeof(RCS.PortableShop.WinPhone.Localization.Localize))]

namespace RCS.PortableShop.WinPhone.Localization
{
    public class Localize : ILocalize
    {
        public void SetLocale(CultureInfo cultureInfo) { }

        public CultureInfo GetCurrentCultureInfo()
        {
            // TODO Replace this with Windows Phone 8.1 specific code.
            //return Thread.CurrentThread.CurrentUICulture;

            // HACK.
            var englishCode = "en";
            var cultureInfo = new CultureInfo(englishCode);

            return cultureInfo;
        }
    }
}
