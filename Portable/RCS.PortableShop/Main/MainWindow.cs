using RCS.PortableShop.ViewModels;
using RCS.PortableShop.Views;
using Xamarin.Forms;

namespace RCS.PortableShop.Main
{
    public class MainWindow : ContentPage
    {
        public MainWindow()
        {
            var view = new MainView();
            view.ViewModel = new MainViewModel();

            Content = view;
        }
    }
}
