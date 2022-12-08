using CommunityToolkit.Mvvm.Input;
using RCS.AdventureWorks.Common.DomainClasses;
using RCS.AdventureWorks.Common.Interfaces;
using RCS.PortableShop.Common.Interfaces;
using RCS.PortableShop.Common.ViewModels;
using RCS.PortableShop.Interfaces;
using RCS.PortableShop.Main;
using RCS.PortableShop.Model;
using RCS.PortableShop.Views;
using System.Text.RegularExpressions;
using System.Windows.Input;

namespace RCS.PortableShop.ViewModels
{
    public class ProductsViewModel :
        FilterItemsViewModel<ProductsOverviewObject, ProductCategory, ProductSubcategory>, IShopper
    {
        #region Construction
        public ProductsViewModel(
            IRepository<List<ProductCategory>, ProductCategory> productCategoriesRepository,
            IRepository<List<ProductSubcategory>, ProductSubcategory> productSubcategoriesRepository,
            IFilterRepository<List<ProductsOverviewObject>, ProductsOverviewObject, ProductCategory, ProductSubcategory, int> productsRepository)
        {
            ProductCategoriesRepository = productCategoriesRepository;
            ProductSubcategoriesRepository = productSubcategoriesRepository;
            ProductsRepository = productsRepository;
        }

        protected override void SetCommands()
        {
            base.SetCommands();

            // TODO MAUI Check out RelayCommand attribute, including CanExecute attribute.
            CartCommand = new AsyncRelayCommand<IShoppingProduct>(CartProduct);
        }
        #endregion

        #region Services
        private IRepository<List<ProductCategory>, ProductCategory> ProductCategoriesRepository { get; }
        private IRepository<List<ProductSubcategory>, ProductSubcategory> ProductSubcategoriesRepository { get; }
        private IFilterRepository<List<ProductsOverviewObject>, ProductsOverviewObject, ProductCategory, ProductSubcategory, int> ProductsRepository { get; }

        private static ProductViewModel ProductViewModel => Startup.ServiceProvider.GetRequiredService<ProductViewModel>();
        private static CartViewModel CartViewModel => Startup.ServiceProvider.GetRequiredService<CartViewModel>();
        #endregion

        #region Filtering
        // At least 3 characters.
        private static readonly BindableProperty ValidTextFilterExpressionProperty =
            BindableProperty.Create(nameof(ValidTextFilterExpression), typeof(string), typeof(ProductsViewModel), @".{3}");

        public string ValidTextFilterExpression
        {
            get => (string)GetValue(ValidTextFilterExpressionProperty);
            set => SetValue(ValidTextFilterExpressionProperty, value);
        }

        // TODO This would better be handled inside the repository.
        protected override async Task<bool> InitializeFilters()
        {
            // Do this sequentially instead of using Task.WhenAll 
            // as that caused threading problems in WcfClient.
            var succeeded = await ProductCategoriesRepository.Refresh().ConfigureAwait(true);
            succeeded &= await ProductSubcategoriesRepository.Refresh().ConfigureAwait(true);

            if (succeeded) await Application.Current.Dispatcher.DispatchAsync(() =>
            {
                var categories = ProductCategoriesRepository.Items;

                foreach (var item in categories)
                {
                    MasterFilterItems.Add(item);
                }

                // Extra event. For some bindings (ItemsSource) those from ObservableCollection are enough, but for others (IsEnabled) this is needed.
                OnPropertyChanged(nameof(MasterFilterItems));

                var subcategories = ProductSubcategoriesRepository.Items;

                foreach (var item in subcategories)
                {
                    DetailFilterItemsSource.Add(item);
                }

                // Retrieve both settings first, as assigning MasterFilterValue changes DetailFilterItems, DetailFilterValue and Settings.ProductSubategoryId.
                var retrievedCategoryId = Settings.ProductCategoryId;
                var retrievedSubcategoryId = Settings.ProductSubategoryId;
                var retrievedTextFilter = Settings.TextFilter;

                var retrievedFilterEmpty = !retrievedCategoryId.HasValue &&
                                           !retrievedSubcategoryId.HasValue &&
                                           retrievedTextFilter == default;

                // Note that MasterFilterValue also determines DetailFilterItems.
                // Note that it currently is allowed to only have a TextFilter.
                if (!retrievedFilterEmpty)
                {
                    MasterFilterValue = MasterFilterItems.FirstOrDefault(value => value.Id == retrievedCategoryId);
                    DetailFilterValue = DetailFilterItems.FirstOrDefault(value => value.Id == retrievedSubcategoryId);
                }
                else
                {
                    MasterFilterValue = MasterFilterItems.FirstOrDefault(value => !value.IsEmpty);
                    DetailFilterValue = DetailFilterItems.FirstOrDefault(value => !value.IsEmpty);
                }

                TextFilterValue = retrievedTextFilter;
            }).ConfigureAwait(true);

            return succeeded;
        }

        protected override bool FilterCanExecute()
        {
            return
                base.FilterCanExecute() &&

                (MasterFilterValue != null && !MasterFilterValue.IsEmpty ||
                !string.IsNullOrEmpty(TextFilterValue) && Regex.IsMatch(TextFilterValue, @".{3}", RegexOptions.IgnoreCase));
        }

        protected override async Task<bool> ReadFiltered()
        {
            // Copy filter values as duration of refresh is unknown.
            var masterFilterValue = MasterFilterValue;
            var detailFilterValue = DetailFilterValue;
            var textFilterValue = TextFilterValue;

            var task = ProductsRepository.Refresh(masterFilterValue, detailFilterValue, textFilterValue);
            await task.ConfigureAwait(true);
            var succeeded = task.Status != TaskStatus.Faulted;

            if (succeeded)
            {
                // Store filter only when executed and succeeded.
                // TODO This could already be part of the base class, if no longer bound to explicit properties in Settings.
                Settings.ProductCategoryId = masterFilterValue?.Id;
                Settings.ProductSubategoryId = detailFilterValue?.Id;
                Settings.TextFilter = textFilterValue;

                // Copy items.
                await Application.Current.Dispatcher.DispatchAsync(() =>
                {
                    foreach (var item in ProductsRepository.Items)
                        Items.Add(item);
                });
            }

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
        protected override async Task ShowDetails(ProductsOverviewObject productsOverviewObject)
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
            set => SetValue(CartCommandProperty, value);
        }

        private static Task CartProduct(IShoppingProduct product)
        {
            return CartViewModel.CartProduct(product);
        }
        #endregion
    }
}