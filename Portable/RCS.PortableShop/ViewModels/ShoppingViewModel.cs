﻿using RCS.PortableShop.Common.ViewModels;
using RCS.PortableShop.Views;
using System.Threading.Tasks;
using System.Windows.Input;
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
            ShowCartCommand = new Command(async () => await ShowCart().ConfigureAwait(true));
        }
        #endregion

        #region Navigation
        public static readonly BindableProperty ShowCartCommandProperty =
             BindableProperty.Create(nameof(ShowCartCommand), typeof(ICommand), typeof(ShoppingWrapperViewModel));

        public ICommand ShowCartCommand
        {
            get => (ICommand)GetValue(ShowCartCommandProperty);
            private set
            {
                SetValue(ShowCartCommandProperty, value);
                RaisePropertyChanged(nameof(ShowCartCommand));
            }
        }

        protected virtual async Task ShowCart()
        {
            var shoppingCartView = new ShoppingCartView() { ViewModel = ShoppingCartViewModel.Instance };

            await PushPage(shoppingCartView).ConfigureAwait(true);
        }
        #endregion
    }
}
