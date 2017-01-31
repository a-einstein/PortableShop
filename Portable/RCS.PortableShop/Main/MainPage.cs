using RCS.PortableShop.ViewModels;
using RCS.PortableShop.Views;
using Xamarin.Forms;

namespace RCS.PortableShop.Main
{
    public class MainPage : ContentPage
    {
        public MainPage()
        {
            // TODO This ViewModel might as well be implicitly included.
            var productsViewModel = new ProductsViewModel() { Navigation = Navigation };
            var productsView = new ProductsView() { ViewModel = productsViewModel };

            var mainViewModel = new MainViewModel() { MainRegionContent = productsView, Navigation = Navigation };
            var mainView = new MainView() { ViewModel = mainViewModel };

            Content = mainView;
        }
    }
}
