using RCS.PortableShop.Localization;
using RCS.PortableShop.Resources;
using System.Diagnostics;
using System.Reflection;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

// Note this is on assembly level. The stated namespace below does not seem to matter.
// I applied this on all assemblies containing XAML. I also tried this separately on the classes at first.
// It all turned out to be fragile, sometimes causing compilation problems.
// Check out the settings of and comments on XamlCompilation elsewhere.
[assembly: XamlCompilation(XamlCompilationOptions.Compile)]

namespace RCS.PortableShop.Main
{
    public partial class MainApplication : Application
    {
        private const string debugPrefix = ">>>> Debug:";

        public MainApplication()
        {
            InitializeComponent();

            // HACK See OnStart.
            // Note See no way to get rid of warning CS4014.
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
            await Task.Run(() =>
            {
                base.OnStart();

                // Moved to constructor because of https://bugzilla.xamarin.com/show_bug.cgi?id=60337
                //await StartActions().ConfigureAwait(true);
            }
            ).ConfigureAwait(true);
        }

        private async Task StartActions()
        {
            // As awaiting other actions caused problems this is just to suppress warning CS1998.
            await Task.Run(() => { }).ConfigureAwait(true);

#if DEBUG
            ListResources();
#endif
            SetCulture();

            // Note this needs to be on the main thread.
            MainPage = new MainShell();
        }
    }
}
