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
        protected override void SetCommands()
        {
            base.SetCommands();

            ShowCartCommand = new Command(ShowCart);
        }

        public static readonly BindableProperty MainRegionContentProperty =
            BindableProperty.Create(nameof(MainRegionContent), typeof(View), typeof(MainViewModel));

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
            // Note this view has got an implicit ViewModel;
            var shoppingCartView = new ShoppingCartView();
            shoppingCartView.ViewModel.Navigation = Navigation;

            PushPage(shoppingCartView, Labels.Cart);
        }
        #endregion
    }
}
