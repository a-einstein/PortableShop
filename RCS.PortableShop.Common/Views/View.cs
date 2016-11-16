using RCS.PortableShop.Common.ViewModels;
using Xamarin.Forms;

namespace RCS.PortableShop.Common.Views
{
    public abstract class View : ContentView
    {
        public static readonly BindableProperty DataContextProperty;
        public object DataContext { get; set; }

        public ViewModel ViewModel
        {
            get { return DataContext as ViewModel; }
            set { DataContext = value; }
        }
    }
}
