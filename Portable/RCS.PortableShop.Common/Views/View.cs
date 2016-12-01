using RCS.PortableShop.Common.ViewModels;
using Xamarin.Forms;

namespace RCS.PortableShop.Common.Views
{
    public abstract class View : ContentView
    {
         public ViewModel ViewModel
        {
            get { return BindingContext as ViewModel; }
            set { BindingContext = value; }
        }
     }
}
