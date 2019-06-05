using RCS.PortableShop.Localization;
using RCS.PortableShop.Resources;
using System.Diagnostics;
using System.Reflection;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace RCS.PortableShop.Main
{
    public partial class MainApplication : Application
    {
        private const string debugPrefix = ">>>> Debug:";

        public MainApplication()
        {
            InitializeComponent();

            // HACK See OnStart.
            StartActions();
        }

        private static void ListResources()
        {
            ListAssemblyResources(typeof(Labels).GetTypeInfo().Assembly);
        }

        private static void ListAssemblyResources(Assembly assembly)
        {
            foreach (var resource in assembly.GetManifestResourceNames())
                Debug.WriteLine($"{debugPrefix} Found resource: {resource}");
        }

        private static void SetCulture()
        {
            // This lookup is NOT required for Windows platforms - the Culture will be automatically set
            switch (Device.RuntimePlatform)
            {
                case Device.iOS:
                case Device.Android:

                    // determine the correct, supported .NET culture
                    var currentCultureInfo = DependencyService.Get<ILocalize>().GetCurrentCultureInfo();
                    Labels.Culture = currentCultureInfo;

                    // set the Thread for locale-aware methods
                    DependencyService.Get<ILocalize>().SetLocale(currentCultureInfo);

                    break;
            }
        }


        protected override async void OnStart()
        {
            base.OnStart();

            // Moved to constructor because of https://bugzilla.xamarin.com/show_bug.cgi?id=60337
            //await StartActions();
        }

        private async Task StartActions()
        {
#if DEBUG
            ListResources();
#endif
            SetCulture();

            MainPage = new MainShell();
        }
    }
}
