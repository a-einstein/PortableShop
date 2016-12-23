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

            // TODO Does not work. Get rid of large margin/padding.
            Padding = 0;

            Content = view;
        }
    }
}
