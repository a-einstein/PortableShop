using RCS.PortableShop.ViewModels;
using RCS.PortableShop.Views;
using Xamarin.Forms;

namespace RCS.PortableShop.Main
{
    public class MainWindow : ContentPage
    {
        public MainWindow()
        {
            var viewModel = new ProductsViewModel();
            var view = new ProductsView(viewModel);

            // TODO Does not work. Get rid of large margin/padding.
            Padding = 0;

            Content = view;
        }
    }
}
