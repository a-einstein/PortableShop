using Microsoft.Extensions.DependencyInjection;
using RCS.AdventureWorks.Common.DomainClasses;
using RCS.PortableShop.Common.ViewModels;
using RCS.PortableShop.Main;
using RCS.PortableShop.Model;
using RCS.PortableShop.Resources;
using System;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows.Input;
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
        {
            // Note this currently is the only direct binding to a repository List. See other comments.
            Items = CartItemsRepository.List;

            CartItemsRepository.List.CollectionChanged += List_CollectionChanged;
        }

        private static volatile ShoppingCartViewModel instance;
        private static object syncRoot = new Object();

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
        protected override void Clear()
        {
            ClearAggregates();
        }

        protected override async Task Read()
        {
            await Task.Run(() =>
            {
                // This is not terribly useful. Alternatively the refresh button could be suppressed or disabled.
                UpdateAggregates();
            }
            ).ConfigureAwait(true);
        }

        public override string MakeTitle() { return Labels.Cart; }
        #endregion

        #region CRUD
        public void CartProduct(IShoppingProduct productsOverviewObject)
        {
            CartItemsRepository.AddProduct(productsOverviewObject);
        }

        public static readonly BindableProperty DeleteCommandProperty =
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

        private void Delete(CartItem cartItem)
        {
            CartItemsRepository.DeleteProduct(cartItem);
        }

        private void List_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    (e.NewItems[0] as CartItem).PropertyChanged += CartItem_PropertyChanged;
                    break;
                case NotifyCollectionChangedAction.Remove:
                    (e.OldItems[0] as CartItem).PropertyChanged -= CartItem_PropertyChanged;
                    break;
            }

            UpdateAggregates();
        }

        private void CartItem_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(CartItem.Quantity))
            {
                UpdateAggregates();
            }
        }
        #endregion

        #region Aggregates
        private void ClearAggregates()
        {
            ProductItemsCount = 0;
            TotalValue = 0;
        }

        private void UpdateAggregates()
        {
            ProductItemsCount = CartItemsRepository.ProductsCount();
            TotalValue = CartItemsRepository.CartValue();
        }

        public static readonly BindableProperty ProductItemCountProperty =
            BindableProperty.Create(nameof(ProductItemsCount), typeof(int), typeof(ShoppingCartViewModel), defaultValue: 0);

        public int ProductItemsCount
        {
            get => (int)GetValue(ProductItemCountProperty);
            set
            {
                SetValue(ProductItemCountProperty, value);
                RaisePropertyChanged(nameof(ProductItemsCount));
            }
        }

        public static readonly BindableProperty TotalValueProperty =
            BindableProperty.Create(nameof(TotalValue), typeof(Decimal), typeof(ShoppingCartViewModel), defaultValue: (Decimal)0);

        public Decimal TotalValue
        {
            get => (Decimal)GetValue(TotalValueProperty);
            set
            {
                SetValue(TotalValueProperty, value);
                RaisePropertyChanged(nameof(TotalValue));
            }
        }
        #endregion
    }
}
