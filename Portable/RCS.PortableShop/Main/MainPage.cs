using RCS.PortableShop.ViewModels;
using RCS.PortableShop.Views;
using Xamarin.Forms;

namespace RCS.PortableShop.Main
{
    public class MainPage : ContentPage
    {
        public MainPage()
        {
            var viewModel = new ProductsViewModel() { Navigation = Navigation };
            var view = new ProductsView(viewModel);

            Content = view;
        }
    }
}
