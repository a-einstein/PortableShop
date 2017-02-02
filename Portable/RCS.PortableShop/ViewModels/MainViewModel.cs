using RCS.PortableShop.Common.ViewModels;
using RCS.PortableShop.Resources;
using RCS.PortableShop.Views;
using System.Windows.Input;
using Xamarin.Forms;
using View = RCS.PortableShop.Common.Views.View;

namespace RCS.PortableShop.ViewModels
{
    public class MainViewModel : ViewModel
    {
        #region Initialization
        protected MainViewModel() { }

        public MainViewModel(View mainRegionContent)
            : this()
        {
            MainRegionContent = mainRegionContent;
        }

        protected override void SetCommands()
        {
            base.SetCommands();

            ShowCartCommand = new Command(ShowCart);
        }

        public static readonly BindableProperty MainRegionContentProperty =
            BindableProperty.Create(nameof(MainRegionContent), typeof(View), typeof(MainViewModel));

        // TODO This might better be moved to the View to better separate the two kind of objects.
        // TODO Once a View is assigned to MainView instead of MainViewModel the latter can be made implicit too.
        public View MainRegionContent
        {
            get { return (View)GetValue(MainRegionContentProperty); }
            set { SetValue(MainRegionContentProperty, value); }
        }
        #endregion

        #region Shopping

        public ICommand ShowCartCommand { get; set; }

        protected void ShowCart()
        {
            var shoppingCartView = new ShoppingCartView();

            PushPage(shoppingCartView, Labels.Cart);
        }
        #endregion
    }
}
