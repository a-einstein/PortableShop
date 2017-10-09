using RCS.PortableShop.ViewModels;
using RCS.PortableShop.Views;
using System.Threading.Tasks;
using Page = RCS.PortableShop.Common.Pages.Page;

namespace RCS.PortableShop.Main
{
    public class MainPage : Page
    {
        private bool initialized;

        protected override async Task Initialize()
        {
            await base.Initialize();

            if (!initialized)
            {
                var productsView = new ProductsView() { ViewModel = new ProductsViewModel() { Page = this } };

                var shoppingWrapperViewModel = new ShoppingWrapperViewModel() { WrappedContent = productsView };
                var shoppingWrapperView = new ShoppingWrapperView() { ViewModel = shoppingWrapperViewModel };

                Content = shoppingWrapperView;

                await shoppingWrapperView.Refresh();

                initialized = true;
            }
        }

        // TODO It would be desirable to stack and pop query pages, enabling return to previous ones without having to set the filter again.
        // A forward button would also be a logical addition.
        // But care should be taken to limit the possibilities to prevent over complication..
        protected override bool OnBackButtonPressed()
        {
            // Prevent backing out of the application.
            return true;
        }
    }
}
