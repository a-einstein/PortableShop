using RCS.PortableShop.Model;
using RCS.PortableShop.Resources;
using RCS.PortableShop.ViewModels;
using RCS.PortableShop.Views;
using System;
using System.Threading.Tasks;
using Xamarin.Forms;
using Page = RCS.PortableShop.Common.Pages.Page;

namespace RCS.PortableShop.Main
{
    public class MainPage : Page
    {
        protected override async void OnAppearing()
        {
            base.OnAppearing();

            // TODO Crashes in Mono when service is not entirely working, particularly during startup.
            // Note this only occurs during debugging, as installed application it is resistant.
            // Currently have no way of catching this, not even in Android.
            // Also see here:
            // https://forums.xamarin.com/discussion/149309/global-exception-handling
            try
            {
                await Refresh();
            }
            catch (Exception)
            {
                 throw;
            }
        }

        private bool initialized;

        protected override async Task Initialize()
        {
            await base.Initialize();

            if (!initialized)
            {
                SubscribeMessages(this);

                var productsView = new ProductsView() { ViewModel = new ProductsViewModel() };

                var shoppingWrapperViewModel = new ShoppingWrapperViewModel() { WrappedContent = productsView };
                var shoppingWrapperView = new ShoppingWrapperView() { ViewModel = shoppingWrapperViewModel };

                Content = shoppingWrapperView;

                initialized = true;
            }
        }

        private bool serviceErrorDisplaying;

        // Note this initializes to 2001.
        private DateTime serviceErrorFirstDisplayed;

        // This value is tested on 3 service calls at startup. There is no multiplication operator.
        private TimeSpan serviceErrorGraceTime = ProductsServiceConsumer.Timeout + ProductsServiceConsumer.Timeout;

        // Note this only works for pages.
        private void SubscribeMessages(Page page)
        {
            // Use the MessagingCenter mechanism to connect ViewModels or other (non GUI) code to this Page.

            MessagingCenter.Subscribe<ProductsServiceConsumer, string>(this, ProductsServiceConsumer.Errors.ServiceError.ToString(), async (sender, details) =>
            {
                // Try to prevent stacking muliple related messages, like at startup.
                // TODO Finetune this. It can also unwantedly prevent messages, like after changing page.
                if (!serviceErrorDisplaying && DateTime.Now > serviceErrorFirstDisplayed + serviceErrorGraceTime)
                {
                    serviceErrorDisplaying = true;
                    serviceErrorFirstDisplayed = DateTime.Now;

                    if (string.IsNullOrWhiteSpace(details))
                        await page.DisplayAlert(Labels.Error, Labels.ErrorService, Labels.Close);
                    else
                    {
                        var showDetails = await page.DisplayAlert(Labels.Error, Labels.ErrorService, Labels.Details, Labels.Close);

                        if (showDetails)
                            await page.DisplayAlert(Labels.Details, details, Labels.Close);
                    }

                    serviceErrorDisplaying = false;
                }
            });

            MessagingCenter.Subscribe<CartItemsRepository>(this, CartItemsRepository.Errors.CartError.ToString(), (sender) =>
            {
                page.DisplayAlert(Labels.Error, Labels.ErrorCart, Labels.Close);
            });
        }

        // TODO It would be desirable to stack and pop query pages, enabling return to previous ones without having to set the filter again.
        // A forward button would also be a logical addition.
        // But care should be taken to limit the possibilities to prevent over complication..
       
        // TODO Does not seem to work anymore. Move to Shell?
        protected override bool OnBackButtonPressed()
        {
            // Prevent backing out of the application.
            return true;
        }
    }
}
