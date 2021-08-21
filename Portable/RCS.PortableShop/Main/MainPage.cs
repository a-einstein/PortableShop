using RCS.PortableShop.Model;
using RCS.PortableShop.Resources;
using RCS.PortableShop.ServiceClients.Products.Wrappers;
using RCS.PortableShop.ViewModels;
using RCS.PortableShop.Views;
using System;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Page = RCS.PortableShop.Common.Pages.Page;

namespace RCS.PortableShop.Main
{
    public class MainPage : Page
    {
        protected override async void OnAppearing()
        {
            base.OnAppearing();

            try
            {
                // TODO Crashes in Mono when service is not entirely working, particularly during startup.
                // Also see here:
                // https://forums.xamarin.com/discussion/149309/global-exception-handling
                // Make sure to do this not earlier than OnAppearing as exceptions may appear, like because of application being in background.
                await Refresh().ConfigureAwait(true);
            }
            catch (Exception exception)
            {
                Alert(this, Labels.Error, Labels.ErrorUnknown, exception.Message, true);
            }
        }

        private bool initialized;

        protected override async Task Initialize()
        {
            await base.Initialize().ConfigureAwait(true);

            if (!initialized)
            {
                initialized = true;

                SubscribeMessages(this);

                var productsView = new ProductsView() { ViewModel = new ProductsViewModel() };

                var shoppingWrapperViewModel = new ShoppingWrapperViewModel() { WrappedContent = productsView };
                var shoppingWrapperView = new ShoppingWrapperView() { ViewModel = shoppingWrapperViewModel };

                Content = shoppingWrapperView;
            }
        }

        private bool serviceErrorDisplaying;

        // Note this initializes to 2001.
        private DateTime serviceErrorFirstDisplayed;

        // This value is tested on 3 service calls at startup. There is no multiplication operator.
        private readonly TimeSpan serviceErrorGraceTime = ServiceClient.Timeout + ServiceClient.Timeout;

        // Note this only works for pages.
        private void SubscribeMessages(Page page)
        {
            // Use the MessagingCenter mechanism to connect ViewModels or other (non GUI) code to this Page.

            MessagingCenter.Subscribe<ProductsServiceConsumer, string>(this, ProductsServiceConsumer.Message.ServiceError.ToString(), (sender, details) =>
            {
                // Try to prevent stacking muliple related messages, like at startup.
                // TODO Finetune this. It can also unwantedly prevent messages, like after changing page.
                if (!serviceErrorDisplaying && DateTime.Now > serviceErrorFirstDisplayed + serviceErrorGraceTime)
                {
                    serviceErrorDisplaying = true;
                    serviceErrorFirstDisplayed = DateTime.Now;

                    Alert(page, Labels.Error, Labels.ErrorService, details);

                    serviceErrorDisplaying = false;
                }
            });

            MessagingCenter.Subscribe<CartItemsRepository>(this, CartItemsRepository.Message.CartError.ToString(), sender =>
            {
                Alert(page, Labels.Error, Labels.ErrorCart);
            });
        }

        private static void Alert(Page page, string kind, string description, string details = null, bool quit = false)
        {
            // Note the thread is particularly relevant for UWP.
            MainThread.BeginInvokeOnMainThread(async () =>
            {
                // Note below about Quit. 
                // TODO Activate when possible, if ever.
                /*
                var continuation = (quit)
                ? Labels.Quit
                : Labels.Close;
                */
                var continuation = Labels.Close;


                if (string.IsNullOrWhiteSpace(details))
                    await page.DisplayAlert(kind, description, continuation).ConfigureAwait(true);
                else
                {
                    var showDetails = await page.DisplayAlert(kind, description, Labels.Details, continuation).ConfigureAwait(true);

                    if (showDetails)
                        await page.DisplayAlert(Labels.Details, details, Labels.Close).ConfigureAwait(true);
                }

                /*
                if (quit)
                    // Aparently this is not (yet) implemented.
                    // https://github.com/xamarin/Xamarin.Forms/issues/10636
                    Application.Current.Quit();
                */
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
