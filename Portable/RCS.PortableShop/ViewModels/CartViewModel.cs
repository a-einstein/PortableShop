using CommunityToolkit.Mvvm.Input;
using RCS.AdventureWorks.Common.DomainClasses;
using RCS.AdventureWorks.Common.Interfaces;
using RCS.PortableShop.Common.Interfaces;
using RCS.PortableShop.Common.ViewModels;
using RCS.PortableShop.Resources;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Windows.Input;

namespace RCS.PortableShop.ViewModels
{
    /// <summary>
    /// Collection level Viewmodel on CartItems.
    /// </summary>
    public class CartViewModel :
        ItemsViewModel<CartItemViewModel>
    {
        #region Construction
        public CartViewModel(IRepository<List<CartItem>, CartItem> cartItemsRepository)
        {
            CartItemsRepository = cartItemsRepository;
        }

        protected override void SetCommands()
        {
            base.SetCommands();

            // TODO MAUI Check out RelayCommand attribute, including CanExecute attribute.
            DeleteCommand = new AsyncRelayCommand<CartItemViewModel>(Delete);
        }
        #endregion

        #region Services
        private IRepository<List<CartItem>, CartItem> CartItemsRepository { get; }
        #endregion

        #region Refresh
        protected override void ClearView()
        {
            UpdateAggregates();

            base.ClearView();
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
            // TODO Perhaps hide Repository.Items.
            // Use an asynchronous Read.

            var sortedItems = new List<CartItem>(CartItemsRepository.Items).OrderBy(item => item.Name);

            await Application.Current.Dispatcher.DispatchAsync(() =>
            {
                foreach (var item in sortedItems)
                {
                    Items.Add(new CartItemViewModel(item));
                }
            });

            UpdateAggregates();

            await base.Read();
        }

        private static readonly BindableProperty DeleteCommandProperty =
            BindableProperty.Create(nameof(DeleteCommand), typeof(ICommand), typeof(CartViewModel));

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
            BindableProperty.Create(nameof(ProductItemsCount), typeof(int), typeof(CartViewModel), 0);

        public int ProductItemsCount
        {
            get => (int)GetValue(ProductItemCountProperty);
            set => SetValue(ProductItemCountProperty, value);
        }

        private static readonly BindableProperty TotalValueProperty =
            BindableProperty.Create(nameof(TotalValue), typeof(decimal), typeof(CartViewModel), (decimal)0);

        public decimal TotalValue
        {
            get => (decimal)GetValue(TotalValueProperty);
            set => SetValue(TotalValueProperty, value);
        }
        #endregion
    }
}
