using RCS.PortableShop.Common.ViewModels;
using RCS.PortableShop.Views;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using Page = RCS.PortableShop.Common.Pages.Page;
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
        private bool initialized;

        protected override async Task<bool> Initialize()
        {
            var baseInitialized = await base.Initialize();

            if (baseInitialized && !initialized)
            {
                Adorn();

                initialized = true;
            }

            return baseInitialized && initialized;
        }

        public override async Task Refresh()
        {
            if (await Initialize())
                await WrappedContent.ViewModel.Refresh();
        }

        public override string Title { get { return WrappedContent.ViewModel.Title; } }
        #endregion

        #region Navigation
        public override Page Page
        {
            get { return WrappedContent.ViewModel.Page; }
            set { WrappedContent.ViewModel.Page = value; }
        }
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

        protected void ShowCart()
        {
            var shoppingCartView = new ShoppingCartView() { ViewModel=ShoppingCartViewModel.Instance};

            // Deactivated because of Page.Title updating.
            //PushPage(shoppingCartView);

            // Workaround as long as Page.Title updating does not work.
            PushPage(shoppingCartView, shoppingCartView.ViewModel.Title);
        }
        #endregion
    }
}
