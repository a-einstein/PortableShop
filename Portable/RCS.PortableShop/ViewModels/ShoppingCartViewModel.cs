using RCS.AdventureWorks.Common.DomainClasses;
using RCS.AdventureWorks.Common.Interfaces;
using RCS.PortableShop.Common.Interfaces;
using RCS.PortableShop.Common.ViewModels;
using RCS.PortableShop.Resources;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.CommunityToolkit.ObjectModel;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace RCS.PortableShop.ViewModels
{
    /// <summary>
    /// Collection level Viewmodel on CartItems.
    /// </summary>
    public class ShoppingCartViewModel :
        ItemsViewModel<CartItemViewModel>
    {
        #region Construction
        public ShoppingCartViewModel(IRepository<List<CartItem>, CartItem> cartItemsRepository)
        {
            CartItemsRepository = cartItemsRepository;
        }

        protected override void SetCommands()
        {
            base.SetCommands();

            DeleteCommand = new AsyncCommand<CartItemViewModel>(Delete);
        }
        #endregion

        #region Services
        private IRepository<List<CartItem>, CartItem> CartItemsRepository { get; }
        #endregion

        #region Refresh
        public override async Task RefreshView()
        {
            await Initialize().ConfigureAwait(true);

            // Note that the repository is leading. Changes to the collection are performed there.
            // After which a new view is created by reloading.

            await ClearView().ConfigureAwait(true);
            await Read().ConfigureAwait(true);

            UpdateAggregates();
        }

        protected override async Task ClearView()
        {
            await base.ClearView().ConfigureAwait(true);

            UpdateAggregates();
        }

        public override string MakeTitle() { return Labels.Cart; }
        #endregion

        #region CRUD
        // TODO Maybe replace async void like in:
        // https://johnthiriet.com/mvvm-going-async-with-async-command/
        // Necessary?

        public async Task CartProduct(IShoppingProduct shoppingProduct)
        {
            var existing = Items.FirstOrDefault(item => item.ProductId == shoppingProduct.Id);

            if (existing == default)
            {
                await CartItemsRepository.Create(new CartItem(shoppingProduct)).ConfigureAwait(true);

                await RefreshView().ConfigureAwait(true);
            }
            else
            {
                existing.Quantity++;

                await CartItemsRepository.Update(existing.CartItem);
            }
        }

        protected override async Task Read()
        {
            await MainThread.InvokeOnMainThreadAsync(() =>
             {
                 foreach (var item in CartItemsRepository.Items)
                 {
                     Items.Add(new CartItemViewModel(item));
                 }
             }).ConfigureAwait(true);

            UpdateAggregates();
        }

        private static readonly BindableProperty DeleteCommandProperty =
            BindableProperty.Create(nameof(DeleteCommand), typeof(ICommand), typeof(ShoppingCartViewModel));

        public ICommand DeleteCommand
        {
            get => (ICommand)GetValue(DeleteCommandProperty);
            private set => SetValue(DeleteCommandProperty, value);
        }

        private async Task Delete(CartItemViewModel cartItem)
        {
            await CartItemsRepository.Delete(cartItem.CartItem).ConfigureAwait(true);

            await RefreshView().ConfigureAwait(true);
        }

        protected override void Items_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            base.Items_CollectionChanged(sender, e);

            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    (e.NewItems[0] as CartItemViewModel).PropertyChanged += CartItem_PropertyChanged;
                    break;
                case NotifyCollectionChangedAction.Remove:
                    (e.OldItems[0] as CartItemViewModel).PropertyChanged -= CartItem_PropertyChanged;
                    break;
            }
        }

        private void CartItem_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(CartItemViewModel.Quantity))
            {
                // Aggregate from the single to the collection level.
                UpdateAggregates();
            }
        }
        #endregion

        #region Aggregates
        private void UpdateAggregates()
        {
            // Note the thread is particularly relevant for UWP.
            MainThread.BeginInvokeOnMainThread(() =>
            {
                ProductItemsCount = SumQuantities();
                TotalValue = SumValues();
            });
        }

        private int SumQuantities()
        {
            return Items.Count > 0
                ? Items.Sum(item => item.Quantity)
                : 0;
        }

        private decimal SumValues()
        {
            return Items.Count > 0
                ? Items.Sum(item => item.Value)
                : 0;
        }

        private static readonly BindableProperty ProductItemCountProperty =
            BindableProperty.Create(nameof(ProductItemsCount), typeof(int), typeof(ShoppingCartViewModel), 0);

        public int ProductItemsCount
        {
            get => (int)GetValue(ProductItemCountProperty);
            set => SetValue(ProductItemCountProperty, value);
        }

        private static readonly BindableProperty TotalValueProperty =
            BindableProperty.Create(nameof(TotalValue), typeof(decimal), typeof(ShoppingCartViewModel), (decimal)0);

        public decimal TotalValue
        {
            get => (decimal)GetValue(TotalValueProperty);
            set => SetValue(TotalValueProperty, value);
        }
        #endregion
    }
}
