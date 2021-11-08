using Microsoft.Extensions.DependencyInjection;
using RCS.PortableShop.Common.ViewModels;
using RCS.PortableShop.Main;
using RCS.PortableShop.Views;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.CommunityToolkit.ObjectModel;
using Xamarin.Forms;

namespace RCS.PortableShop.ViewModels
{
    public abstract class ShoppingViewModel : ViewModel
    {
        #region Construction
        protected override void SetCommands()
        {
            base.SetCommands();

            // Note it is not possible to set this in BindableProperty.Create().
            ShowCartCommand = new AsyncCommand(ShowCart);
        }
        #endregion

        #region Services
        private static CartViewModel CartViewModel => Startup.ServiceProvider.GetRequiredService<CartViewModel>();
        #endregion

        #region Navigation

        private static readonly BindableProperty ShowCartCommandProperty =
             BindableProperty.Create(nameof(ShowCartCommand), typeof(ICommand), typeof(ShoppingWrapperViewModel));

        public ICommand ShowCartCommand
        {
            get => (ICommand)GetValue(ShowCartCommandProperty);
            private set => SetValue(ShowCartCommandProperty, value);
        }

        protected virtual async Task ShowCart()
        {
            var cartView = new CartView() { ViewModel = CartViewModel };

            await PushPage(cartView).ConfigureAwait(true);
        }
        #endregion
    }
}
