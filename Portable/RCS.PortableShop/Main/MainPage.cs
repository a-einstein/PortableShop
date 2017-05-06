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

            var mainViewModel = new MainViewModel(productsView);
            var mainView = new MainView() { ViewModel = mainViewModel };

            Content = mainView;

            // TODO Apparently the explicit translation is superfluous. Check this for xaml and possibly cleanup.
            //Title = TranslateExtension.ProvideValue(Labels.Products) as string;
            Title = Labels.Products;

            // TODO replace name by icon.
            ToolbarItems.Add(new ToolbarItem("R", "Refresh.png", mainView.ViewModel.Refresh));

            // TODO Add application icon here too?
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
