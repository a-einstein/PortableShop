using RCS.PortableShop.Localization;
using RCS.PortableShop.Resources;
using System.Diagnostics;
using System.Reflection;
using Xamarin.CommunityToolkit.Helpers;
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
            LocalizationResourceManager.Current.Init(Labels.ResourceManager);

            // This lookup is NOT required for Windows platforms - the Culture will be automatically set
            switch (Device.RuntimePlatform)
            {
                case Device.iOS:
                case Device.Android:

                    // determine the correct, supported .NET culture
                    // TODO Is this still necessary?
                    var currentCultureInfo = DependencyService.Get<ILocalize>().GetCurrentCultureInfo();

                    LocalizationResourceManager.Current.CurrentCulture = currentCultureInfo;

                    break;
            }
        }

        protected override void OnStart()
        {
            base.OnStart();
            StartActions();
        }

        private void StartActions()
        {
#if DEBUG
            ListResources();
#endif
            SetCulture();

            Startup.Init();

            // Currently this crashes for exclusively the RELEASE build of x86 and x64.
            // This might be related to https://github.com/xamarin/Xamarin.Forms/issues/11736
            // TODO follow that issue and test for new releases.

            // Note this needs to be on the main thread.
            MainPage = new MainShell();
        }
    }
}
