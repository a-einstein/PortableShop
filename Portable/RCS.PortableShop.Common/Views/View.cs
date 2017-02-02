using RCS.PortableShop.Common.ViewModels;
using Xamarin.Forms;

namespace RCS.PortableShop.Common.Views
{
    // This no longer can be abstract as it needs a default constructor for the regions.
    public class View : ContentView
    {
        public ViewModel ViewModel
        {
            get { return BindingContext as ViewModel; }
            set
            {
                BindingContext = value;
                ViewModel.Navigation = Navigation;
            }
        }
    }
}
