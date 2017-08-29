using RCS.PortableShop.Common.ViewModels;
using RCS.PortableShop.Resources;
using RCS.PortableShop.Views;
using System.Windows.Input;
using Xamarin.Forms;
using View = RCS.PortableShop.Common.Views.View;

namespace RCS.PortableShop.ViewModels
{
    // TODO Might as well keep this view and just change MainRegionContent.
    public class ShoppingWrapperViewModel : ViewModel
    {
        #region Construct
        protected override void SetCommands()
        {
            base.SetCommands();

            ShowCartCommand = new Command(ShowCart);
        }

        public static readonly BindableProperty WrappedContentProperty =
            BindableProperty.Create(nameof(WrappedContent), typeof(View), typeof(ShoppingWrapperViewModel));

        // TODO This might better be moved to the View to better separate the two kind of objects.
        // TODO Once a View is assigned to MainView instead of MainViewModel the latter can be made implicit too.
        public View WrappedContent
        {
            get { return (View)GetValue(WrappedContentProperty); }
            set { SetValue(WrappedContentProperty, value); }
        }
        #endregion

        #region Refresh
        public override async void Refresh()
        {
            WrappedContent.ViewModel.Refresh();
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
