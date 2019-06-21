using System;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace RCS.PortableShop.ViewModels
{
    public class MainShellViewModel : ShoppingViewModel
    {
        protected override void SetCommands()
        {
            base.SetCommands();

            ShowProductsCommand = new Command(async () => await ShowProducts());
            OpenSupportCommand = new Command(() => OpenSupport());
        }

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

        protected override async Task ShowCart()
        {
            Shell.FlyoutIsPresented = false;
            await base.ShowCart();
        }

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
            Shell.FlyoutIsPresented = false;

            // TODO Make this Configureable.
            Device.OpenUri(new Uri("https://github.com/a-einstein/PortableShop/blob/master/README.md"));
        }
    }
}
