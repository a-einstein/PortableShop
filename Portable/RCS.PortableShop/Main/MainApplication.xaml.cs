using RCS.PortableShop.Localization;
using RCS.PortableShop.Model;
using RCS.PortableShop.Resources;
using System;
using System.Diagnostics;
using System.Reflection;
using Xamarin.Forms;

namespace RCS.PortableShop.Main
{
    public partial class MainApplication : Application
    {
        private const string debugPrefix = ">>>> Debug:";

        // Note this initializes to 2001.
        private DateTime errorFirstReported;

        // This value is tested on 3 service calls at startup. There is no multiplication operator.
        private TimeSpan errorTimeout = ProductsServiceConsumer.Timeout + ProductsServiceConsumer.Timeout; 

        public MainApplication()
        {
            InitializeComponent();

#if DEBUG
            ListResources();
#endif
            SetCulture();

            var mainPage = new MainPage();
            NavigationPage.SetHasNavigationBar(mainPage, false);
            MainPage = new NavigationPage(mainPage);

            // Use this mechanism to connect ViewModels or other non GUI code to this Page.
            MessagingCenter.Subscribe<ProductsServiceConsumer>(this, ProductsServiceConsumer.Errors.serviceError.ToString(), (sender) =>
            {
                // Try to prevent stacking muliple related errors, like at startup.
                if (DateTime.Now > errorFirstReported + errorTimeout)
                {
                    errorFirstReported = DateTime.Now;
                    mainPage.DisplayAlert(Labels.Error, Labels.ServiceError, Labels.Close);
                }
            });
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
