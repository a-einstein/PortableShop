using Microsoft.Extensions.DependencyInjection;
using RCS.AdventureWorks.Common.DomainClasses;
using RCS.PortableShop.Common.ViewModels;
using RCS.PortableShop.Interfaces;
using RCS.PortableShop.Main;
using RCS.PortableShop.Model;
using RCS.PortableShop.Views;
using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace RCS.PortableShop.ViewModels
{
    public class ProductsViewModel : FilterItemsViewModel<ProductsOverviewObject, ProductCategory, ProductSubcategory>, IShopper
    {
        #region Construction
        public ProductsViewModel(
            ProductCategoriesRepository productCategoriesRepository,
            ProductSubcategoriesRepository productSubcategoriesRepository,
            ProductsRepository productsRepository)
        {
            ProductCategoriesRepository = productCategoriesRepository;
            ProductSubcategoriesRepository = productSubcategoriesRepository;
            ProductsRepository = productsRepository;
        }

        protected override void SetCommands()
        {
            base.SetCommands();

            CartCommand = new Command<ProductsOverviewObject>(CartProduct);
        }
        #endregion

        #region Services
        private ProductCategoriesRepository ProductCategoriesRepository { get; }
        private ProductSubcategoriesRepository ProductSubcategoriesRepository { get; }
        private ProductsRepository ProductsRepository { get; }

        private static ProductViewModel ProductViewModel => Startup.ServiceProvider.GetRequiredService<ProductViewModel>();
        private static ShoppingCartViewModel ShoppingCartViewModel => Startup.ServiceProvider.GetRequiredService<ShoppingCartViewModel>();
        #endregion

        #region Filtering
        // At least 3 characters.
        private static readonly BindableProperty ValidTextFilterExpressionProperty =
            BindableProperty.Create(nameof(ValidTextFilterExpression), typeof(string), typeof(ProductsViewModel), @"\w{3}");

        public string ValidTextFilterExpression
        {
            get => (string)GetValue(ValidTextFilterExpressionProperty);
            set
            {
                SetValue(ValidTextFilterExpressionProperty, value);
                RaisePropertyChanged(nameof(ValidTextFilterExpression));
            }
        }

        // TODO This would better be handled inside the repository.
        protected override async Task<bool> InitializeFilters()
        {
            // TODO To base?
            // TODO Reconsider this approach.
            MasterFilterItems.CollectionChanged += MasterFilterItems_CollectionChanged;
            DetailFilterItems.CollectionChanged += DetailFilterItems_CollectionChanged;

            // Do this sequentially instead of using Task.WhenAll 
            // as that caused threading problems in WcfClient.
            await ProductCategoriesRepository.Refresh().ConfigureAwait(true);
            await ProductSubcategoriesRepository.Refresh().ConfigureAwait(true);

            await MainThread.InvokeOnMainThreadAsync(() =>
            {
                var categories = ProductCategoriesRepository.Items;
                var masterFilterItems = new ObservableCollection<ProductCategory>();

                foreach (var item in categories)
                {
                    masterFilterItems.Add(item);
                }

                // Do an assignment, as there is not much use to follow up on each item. 
                // TODO maybe follow the approach on ItemsViewModel.Items.
                MasterFilterItems = masterFilterItems;

                var subcategories = ProductSubcategoriesRepository.Items;

                foreach (var item in subcategories)
                {
                    DetailFilterItemsSource.Add(item);
                }

                // Retrieve both settings first, as assigning MasterFilterValue changes DetailFilterItems, DetailFilterValue and Settings.ProductSubategoryId.
                var retrievedCategoryId = Settings.ProductCategoryId;
                var retrievedSubcategoryId = Settings.ProductSubategoryId;

                MasterFilterValue = retrievedCategoryId.HasValue
                    ? MasterFilterItems.FirstOrDefault(value => value.Id == retrievedCategoryId.Value)
                    : MasterFilterItems.FirstOrDefault(value => !value.IsEmpty);

                // Note that MasterFilterValue also determines DetailFilterItems.
                DetailFilterValue = retrievedSubcategoryId.HasValue
                ? DetailFilterItems.FirstOrDefault(value => value.Id == retrievedSubcategoryId.Value)
                : DetailFilterItems.FirstOrDefault(value => !value.IsEmpty);

                // Note the settings mechanism does work, but there is a binding problem in ClearableEntry, as described here:
                // https://forums.xamarin.com/discussion/84044/cannot-create-a-user-control-with-two-way-binding-to-view-model-property-names
                // On a suggestion I have changed the bindings along this chain, but that did not help.

                // It does work with a simple Entry in the view instead, but that loses the functionality of ClearableEntry.
                // To avoid confusion, don't set the value, as it is not visible but influences the query.

                //TextFilterValue = Settings.TextFilter;
            }).ConfigureAwait(true);

            return true;
        }

        private void MasterFilterItems_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            RaisePropertyChanged(nameof(MasterFilterItems));
        }

        private void DetailFilterItems_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            RaisePropertyChanged(nameof(DetailFilterItems));
        }

        public override ProductCategory MasterFilterValue
        {
            get => base.MasterFilterValue;
            set
            {
                Settings.ProductCategoryId = value?.Id;
                base.MasterFilterValue = value;
            }
        }

        public override ProductSubcategory DetailFilterValue
        {
            get => base.DetailFilterValue;
            set
            {
                Settings.ProductSubategoryId = value?.Id;
                base.DetailFilterValue = value;
            }
        }

        public override string TextFilterValue
        {
            get => base.TextFilterValue;
            set
            {
                Settings.TextFilter = value;
                base.TextFilterValue = value;
            }
        }

        protected override bool FilterCanExecute()
        {
            return
                !Awaiting &&
                (MasterFilterValue != null && !MasterFilterValue.IsEmpty ||
                !string.IsNullOrEmpty(TextFilterValue) && Regex.IsMatch(TextFilterValue, @"\w{3}", RegexOptions.IgnoreCase));
        }

        protected override async Task<bool> ReadFiltered()
        {
            var masterFilterValue = MasterFilterValue;
            var detailFilterValue = DetailFilterValue;
            var textFilterValue = TextFilterValue;

            var task = ProductsRepository.Refresh(masterFilterValue, detailFilterValue, textFilterValue);
            await task.ConfigureAwait(true);
            var succeeded = task.Status != TaskStatus.Faulted;

            if (succeeded)
                await MainThread.InvokeOnMainThreadAsync(() =>
                 {
                     foreach (var item in ProductsRepository.Items)
                         Items.Add(item);
                 }).ConfigureAwait(true);

            return succeeded;
        }

        protected override Func<ProductSubcategory, bool> DetailFilterItemsSelector(bool preserveEmptyElement = true)
        {
            return subcategory =>
                MasterFilterValue != null &&
                !MasterFilterValue.IsEmpty &&
                (preserveEmptyElement && subcategory.IsEmpty || subcategory.ProductCategoryId == MasterFilterValue.Id);
        }
        #endregion

        #region Navigation
        protected override async void ShowDetails(ProductsOverviewObject productsOverviewObject)
        {
            ProductViewModel.ItemId = productsOverviewObject.Id;
            var productView = new ProductView() { ViewModel = ProductViewModel };

            var wrapperViewModel = new ShoppingWrapperViewModel() { WrappedContent = productView };
            var wrapperView = new ShoppingWrapperView() { ViewModel = wrapperViewModel };

            await PushPage(wrapperView).ConfigureAwait(true);
        }
        #endregion

        #region Shopping
        private static readonly BindableProperty CartCommandProperty =
            BindableProperty.Create(nameof(CartCommand), typeof(ICommand), typeof(ProductsViewModel));

        public ICommand CartCommand
        {
            get => (ICommand)GetValue(CartCommandProperty);
            set
            {
                SetValue(CartCommandProperty, value);
                RaisePropertyChanged(nameof(CartCommand));
            }
        }

        private static void CartProduct(ProductsOverviewObject productsOverviewObject)
        {
            // TODO Do this directly on the repository? (Might need initialisation first.)
            ShoppingCartViewModel.CartProduct(productsOverviewObject);
        }
        #endregion
    }
}