using RCS.AdventureWorks.Common.DomainClasses;
using RCS.PortableShop.Common.ViewModels;
using RCS.PortableShop.Model;
using RCS.PortableShop.Resources;
using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace RCS.PortableShop.ViewModels
{
    public class ShoppingCartViewModel : ItemsViewModel<CartItem>
    {
        // Note currently this class only stores in memory.

        #region Construct
        private ShoppingCartViewModel()
        {
            // HACK Typing is unclear here.
            // Besides, this currently is the only binding to a repository List.
            Items = CartItemsRepository.Instance.List as ObservableCollection<CartItem>;

            (CartItemsRepository.Instance.List as ObservableCollection<CartItem>).CollectionChanged += List_CollectionChanged;
        }

        private static volatile ShoppingCartViewModel instance;
        private static object syncRoot = new Object();

        // Note this class is a singleton, implemented along the way (but not entirely) of https://msdn.microsoft.com/en-us/library/ff650316.aspx
        // TODO This might no longer be necessary if properly shared on import.
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

        #region Refresh
        protected override void Clear()
        {
            ClearAggregates();
        }

        protected override async Task<bool> Read()
        {
            // This is not terribly useful. Alternatively the refresh button could be suppressed or disabled.
            UpdateAggregates();

            return true;
        }

        public override string Title { get { return Labels.Cart; } }
        #endregion

        #region CRUD
        public void CartProduct(IShoppingProduct productsOverviewObject)
        {
            CartItemsRepository.Instance.AddProduct(productsOverviewObject);
        }

        public static readonly BindableProperty DeleteCommandProperty =
            BindableProperty.Create(nameof(DeleteCommand), typeof(ICommand), typeof(ShoppingCartViewModel));

        public ICommand DeleteCommand
        {
            get { return (ICommand)GetValue(DeleteCommandProperty); }
            private set
            {
                SetValue(DeleteCommandProperty, value);
                RaisePropertyChanged(nameof(DeleteCommand));
            }
        }

        private void Delete(CartItem cartItem)
        {
            CartItemsRepository.Instance.DeleteProduct(cartItem);
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
            ProductItemsCount = CartItemsRepository.Instance.ProductsCount();
            TotalValue = CartItemsRepository.Instance.CartValue();
        }

        public static readonly BindableProperty ProductItemCountProperty =
            BindableProperty.Create(nameof(ProductItemsCount), typeof(int), typeof(ShoppingCartViewModel), defaultValue: 0);

        public int ProductItemsCount
        {
            get { return (int)GetValue(ProductItemCountProperty); }
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
            get { return (Decimal)GetValue(TotalValueProperty); }
            set
            {
                SetValue(TotalValueProperty, value);
                RaisePropertyChanged(nameof(TotalValue));
            }
        }
        #endregion
    }
}
