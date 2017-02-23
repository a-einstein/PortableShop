using RCS.PortableShop.ViewModels;
using RCS.PortableShop.Views;
using Xamarin.Forms;

namespace RCS.PortableShop.Main
{
    public class MainPage : ContentPage
    {
        public MainPage()
        {
            var productsView = new ProductsView();

            var mainViewModel = new MainViewModel(productsView);
            var mainView = new MainView() { ViewModel = mainViewModel };

            Content = mainView;
        }

        protected override bool OnBackButtonPressed()
        {
            // Prevent backing out of the application.
            return true;
        }
    }
}
