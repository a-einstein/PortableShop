using RCS.PortableShop.Resources;
using RCS.PortableShop.ViewModels;
using RCS.PortableShop.Views;
using Xamarin.Forms;

namespace RCS.PortableShop.Main
{
    public class MainPage : ContentPage
    {
        // TODO Share code with ViewModel.PushPage().
        public MainPage()
        {
            var productsView = new ProductsView();

            var shoppingWrapperViewModel = new ShoppingWrapperViewModel() { WrappedContent = productsView };
            var shoppingWrapperView = new ShoppingWrapperView() { ViewModel = shoppingWrapperViewModel };

            Content = shoppingWrapperView;

            // TODO Apparently the explicit translation is superfluous. Check this for xaml and possibly cleanup.
            //Title = TranslateExtension.ProvideValue(Labels.Shop) as string;
            Title = Labels.Shop;

            ToolbarItems.Add(new ToolbarItem("R", "Refresh.png", shoppingWrapperView.ViewModel.Refresh));
            ToolbarItems.Add(new ToolbarItem("I", "About.png", About));

            shoppingWrapperViewModel.Refresh();
        }

        private async void About()
        {
            // TODO The version has to get shared with the Android manifest (to start with).
            await DisplayAlert(Labels.AboutLabel, string.Format(Labels.AboutText, Labels.Shop, Labels.Developer, "0.6.0"), Labels.Close);
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
