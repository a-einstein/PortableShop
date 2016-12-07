using RCS.PortableShop.ViewModels;
using RCS.PortableShop.Views;
using Xamarin.Forms;

namespace RCS.PortableShop.Main
{
    public class MainWindow : ContentPage
    {
        public MainWindow()
        {
            var view = new ProductsView();
            view.ViewModel = new ProductsViewModel();

            Content = view;
        }
    }
}
