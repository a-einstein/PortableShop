using System;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace RCS.PortableShop.ViewModels
{
    public class MainShellViewModel : ShoppingViewModel
    {
        #region Construction
        protected override void SetCommands()
        {
            base.SetCommands();

            ShowProductsCommand = new Command(async () => await ShowProducts());
            OpenSupportCommand = new Command(() => OpenSupport());
            UpdateCommand = new Command(() => Update());
        }
        #endregion

        #region Products
        public static readonly BindableProperty ShowProductsCommandProperty =
            BindableProperty.Create(nameof(ShowProductsCommand), typeof(ICommand), typeof(MainShellViewModel));

        public ICommand ShowProductsCommand
        {
            get { return (ICommand)GetValue(ShowProductsCommandProperty); }
            private set
            {
                SetValue(ShowProductsCommandProperty, value);
                RaisePropertyChanged(nameof(ShowProductsCommand));
            }
        }

        protected static async Task ShowProducts()
        {
            Shell.FlyoutIsPresented = false;
            await PopToRoot();
        }
        #endregion

        #region Cart
        protected override async Task ShowCart()
        {
            Shell.FlyoutIsPresented = false;
            await base.ShowCart();
        }
        #endregion

        #region Support

        public static readonly BindableProperty OpenSupportCommandProperty =
            BindableProperty.Create(nameof(OpenSupportCommand), typeof(ICommand), typeof(MainShellViewModel));

        public ICommand OpenSupportCommand
        {
            get { return (ICommand)GetValue(OpenSupportCommandProperty); }
            private set
            {
                SetValue(OpenSupportCommandProperty, value);
                RaisePropertyChanged(nameof(OpenSupportCommand));
            }
        }

        protected static void OpenSupport()
        {
            // TODO Make this Configureable.
            OpenWeb("https://github.com/a-einstein/PortableShop/blob/master/README.md");
        }
        #endregion

        #region Update
        public static readonly BindableProperty UpdateCommandProperty =
             BindableProperty.Create(nameof(UpdateCommand), typeof(ICommand), typeof(MainShellViewModel));

        public ICommand UpdateCommand
        {
            get { return (ICommand)GetValue(UpdateCommandProperty); }
            set 
            { 
                SetValue(UpdateCommandProperty, value); 
                RaisePropertyChanged(nameof(UpdateCommand));
            }
        }

        protected static void Update()
        {
            // Note this is only very rough and may fail on the still open application and other technicalities afterwards.

            // TODO Check versions first. Proceed with either reporting no update needed, or consent to close the application. Even better: check automatically at startup.

            // TODO Make this Configureable.
            OpenWeb("https://rcsadventureworac85.blob.core.windows.net/portableshop-releases/latest/RCS.CyclOne.apk");
        }
        #endregion

        #region Utilities
        private static void OpenWeb(string webAddress)
        {
            Shell.FlyoutIsPresented = false;

            Browser.OpenAsync(new Uri(webAddress));
        }
        #endregion
    }
}
