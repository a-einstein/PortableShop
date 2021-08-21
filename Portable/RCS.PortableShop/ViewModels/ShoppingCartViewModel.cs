using Microsoft.Extensions.DependencyInjection;
using RCS.AdventureWorks.Common.DomainClasses;
using RCS.PortableShop.Common.ViewModels;
using RCS.PortableShop.Main;
using RCS.PortableShop.Model;
using RCS.PortableShop.Resources;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace RCS.PortableShop.ViewModels
{
    public class ShoppingCartViewModel : ItemsViewModel<CartItem>
    {
        /*
        TODO This is a bit overdone. It is a double singleton. Only one of those is necessary because it stores in memory. 
        Once bound to the combination of a user and a database with instant updates THAT would be the singleton data store.
        The current sharing of List as Items is undesirable, but done to prevent the otherwise necessary synchronisation of them.
        This relates to warning CA2227 on Items and the like.
        */

        #region Construction
        private ShoppingCartViewModel()
        { }

        private static volatile ShoppingCartViewModel instance;
        private static readonly object syncRoot = new object();

        // Note this class is a singleton, implemented along the way (but not entirely) of https://msdn.microsoft.com/en-us/library/ff650316.aspx
        public static ShoppingCartViewModel Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (syncRoot)
                    {
                        if (instance == null)
                            instance = new ShoppingCartViewModel();
                    }
                }

                return instance;
            }
        }

        protected override void SetCommands()
        {
            base.SetCommands();

            DeleteCommand = new Command<CartItem>(Delete);
        }
        #endregion

        #region Repositories
        private static CartItemsRepository CartItemsRepository => Startup.ServiceProvider.GetRequiredService<CartItemsRepository>();
        #endregion

        #region Refresh
        private bool dirty = false;

        public override async Task Refresh()
        {
            await Initialize().ConfigureAwait(true);

            // Prevent unnecessary action when just navigating to the full view.
            // Note that actions can already be performed and reflected in the summary.
            if (dirty)
            {
                dirty = false;

                // Note that the repository is leading. 
                // Changes here are perfomed there, afterwhich it is reloaded.
                await Clear().ConfigureAwait(true);
                await Read().ConfigureAwait(true);
            }
        }

        protected override async Task Clear()
        {
            await base.Clear().ConfigureAwait(true);

            UpdateAggregates();
        }

        public override string MakeTitle() { return Labels.Cart; }
        #endregion

        #region CRUD
        // TODO Maybe replace async void like in:
        // https://johnthiriet.com/mvvm-going-async-with-async-command/
        // Necessary?

        public async void CartProduct(IShoppingProduct productsOverviewObject)
        {
            var existing = Items.FirstOrDefault(item => item.ProductId == productsOverviewObject.Id);

            if (existing == default)
            {
                await CartItemsRepository.Create(productsOverviewObject).ConfigureAwait(true);
                dirty = true;
                await Refresh().ConfigureAwait(true);
            }
            else
            {
                // Note this triggers CartItem_PropertyChanged.
                existing.Quantity++;
            }
        }

        protected override async Task Read()
        {
            await MainThread.InvokeOnMainThreadAsync(() =>
             {
                 foreach (var item in CartItemsRepository.Items)
                 {
                     // Use a copy to maintain separation between this and the repository though they contain the same type of items.
                     Items.Add(item.Copy());
                 }
             }).ConfigureAwait(true);

            UpdateAggregates();
        }

        private static readonly BindableProperty DeleteCommandProperty =
            BindableProperty.Create(nameof(DeleteCommand), typeof(ICommand), typeof(ShoppingCartViewModel));

        public ICommand DeleteCommand
        {
            get => (ICommand)GetValue(DeleteCommandProperty);
            private set
            {
                SetValue(DeleteCommandProperty, value);
                RaisePropertyChanged(nameof(DeleteCommand));
            }
        }

        private async void Delete(CartItem cartItem)
        {
            await CartItemsRepository.Delete(cartItem).ConfigureAwait(true);
            dirty = true;

            await Refresh().ConfigureAwait(true);
        }

        protected override void Items_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            base.Items_CollectionChanged(sender, e);

            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    (e.NewItems[0] as CartItem).PropertyChanged += CartItem_PropertyChanged;
                    break;
                case NotifyCollectionChangedAction.Remove:
                    (e.OldItems[0] as CartItem).PropertyChanged -= CartItem_PropertyChanged;
                    break;
            }
        }

        private void CartItem_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(CartItem.Quantity))
            {
                CartItemsRepository.Update(sender as CartItem).ConfigureAwait(true);
                dirty = true;
            }
        }
        #endregion

        #region Aggregates
        private void UpdateAggregates()
        {
            // Note the thread is particularly relevant for UWP.
            MainThread.BeginInvokeOnMainThread(() =>
            {
                ProductItemsCount = Count();
                TotalValue = Value();
            });
        }

        private int Count()
        {
            return Items.Count > 0
                ? Items.Sum(item => item.Quantity)
                : 0;
        }

        private decimal Value()
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
            set
            {
                SetValue(ProductItemCountProperty, value);
                RaisePropertyChanged(nameof(ProductItemsCount));
            }
        }

        private static readonly BindableProperty TotalValueProperty =
            BindableProperty.Create(nameof(TotalValue), typeof(decimal), typeof(ShoppingCartViewModel), (decimal)0);

        public decimal TotalValue
        {
            get => (decimal)GetValue(TotalValueProperty);
            set
            {
                SetValue(TotalValueProperty, value);
                RaisePropertyChanged(nameof(TotalValue));
            }
        }
        #endregion
    }
}
