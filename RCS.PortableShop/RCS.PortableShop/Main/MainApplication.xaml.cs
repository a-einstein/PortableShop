using RCS.PortableShop.Localization;
using RCS.PortableShop.Resources;
using System.Diagnostics;
using System.Reflection;
using Xamarin.Forms;

namespace RCS.PortableShop.Main
{
    public partial class MainApplication : Application
    {
        public MainApplication()
        {
            InitializeComponent();

#if DEBUG
            ListResources();
#endif
            SetCulture();

            MainPage = new MainWindow();
        }

        private static void ListResources()
        {
            Debug.WriteLine("====== Resource debug info =========");

            ListAssemblyResources(typeof(Labels).GetTypeInfo().Assembly);

            Debug.WriteLine("====================================");
        }

        private static void ListAssemblyResources(Assembly assembly)
        {
            foreach (var resource in assembly.GetManifestResourceNames())
                Debug.WriteLine($"Found resource: {resource}");
        }

        private static void SetCulture()
        {
            // This lookup is NOT required for Windows platforms - the Culture will be automatically set
            if (Device.OS == TargetPlatform.iOS || Device.OS == TargetPlatform.Android)
            {
                // determine the correct, supported .NET culture
                var currentCultureInfo = DependencyService.Get<ILocalize>().GetCurrentCultureInfo();

                Labels.Culture = currentCultureInfo; // set the RESX for resource localization

                DependencyService.Get<ILocalize>().SetLocale(currentCultureInfo); // set the Thread for locale-aware methods
            }
        }

        protected override void OnStart()
        {
            // Handle when your app starts
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }
}
