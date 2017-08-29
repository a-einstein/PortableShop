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

        private bool serviceErrorDisplaying;

        // Note this initializes to 2001.
        private DateTime serviceErrorFirstDisplayed;

        // This value is tested on 3 service calls at startup. There is no multiplication operator.
        private TimeSpan serviceErrorGraceTime = ProductsServiceConsumer.Timeout + ProductsServiceConsumer.Timeout;

        public MainApplication()
        {
            InitializeComponent();

#if DEBUG
            ListResources();
#endif
            SetCulture();

            var mainPage = new MainPage();
            MainPage = new NavigationPage(mainPage);

            SubscribeMessages(mainPage);
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

        private void SubscribeMessages(Page page)
        {
            // Use the MessagingCenter mechanism to connect ViewModels or other non GUI code to this Page.

            MessagingCenter.Subscribe<ProductsServiceConsumer, string>(this, ProductsServiceConsumer.Errors.ServiceError.ToString(), async (sender, details) =>
            {
                // Try to prevent stacking muliple related messages, like at startup.
                // TODO Finetune this. It can also unwantedly prevent messages, like after changing page.
                if (!serviceErrorDisplaying && DateTime.Now > serviceErrorFirstDisplayed + serviceErrorGraceTime)
                {
                    serviceErrorDisplaying = true;
                    serviceErrorFirstDisplayed = DateTime.Now;

                    if (string.IsNullOrWhiteSpace(details))
                        await page.DisplayAlert(Labels.Error, Labels.ServiceError, Labels.Close);
                    else
                    {
                        var showDetails = await page.DisplayAlert(Labels.Error, Labels.ServiceError, Labels.Details, Labels.Close);

                        if (showDetails)
                            await page.DisplayAlert(Labels.Details, details, Labels.Close);
                    }

                    serviceErrorDisplaying = false;
                }
            });

            MessagingCenter.Subscribe<CartItemsRepository>(this, CartItemsRepository.Errors.CartError.ToString(), (sender) =>
            {
                page.DisplayAlert(Labels.Error, Labels.CartError, Labels.Close);
            });
        }

        protected override void OnStart()
        {
            // Handle when your app starts
            // TODO Refresh here?
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
