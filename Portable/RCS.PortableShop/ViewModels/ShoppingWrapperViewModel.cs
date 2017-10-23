using RCS.PortableShop.Common.ViewModels;
using RCS.PortableShop.Views;
using System.Threading.Tasks;
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
            set
            {
                SetValue(WrappedContentProperty, value);

                // Chain the Title properties.
                SetBinding(TitleProperty, new Binding() { Path = "Title", Source = WrappedContent.ViewModel });
            }
        }
        #endregion

        #region Refresh

        public override async Task Refresh()
        {
            if (await Initialize())
                await WrappedContent.ViewModel.Refresh();
        }

        public override string MakeTitle() { return WrappedContent?.ViewModel.MakeTitle(); }
        #endregion

        #region Shopping
        public static readonly BindableProperty ShowCartCommandProperty =
             BindableProperty.Create(nameof(ShowCartCommand), typeof(ICommand), typeof(ShoppingWrapperViewModel));

        public ICommand ShowCartCommand
        {
            get { return (ICommand)GetValue(ShowCartCommandProperty); }
            private set
            {
                SetValue(ShowCartCommandProperty, value);
                RaisePropertyChanged(nameof(ShowCartCommand));
            }
        }

        protected async void ShowCart()
        {
            var shoppingCartView = new ShoppingCartView() { ViewModel = ShoppingCartViewModel.Instance };

            await PushPage(shoppingCartView);
        }
        #endregion
    }
}
