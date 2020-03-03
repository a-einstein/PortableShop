using Microsoft.Extensions.DependencyInjection;
using RCS.AdventureWorks.Common.DomainClasses;
using RCS.PortableShop.Common.ViewModels;
using RCS.PortableShop.Interfaces;
using RCS.PortableShop.Main;
using RCS.PortableShop.Model;
using RCS.PortableShop.Views;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace RCS.PortableShop.ViewModels
{
    public class ProductsViewModel : FilterItemsViewModel<ProductsOverviewObject, ProductCategory, ProductSubcategory>, IShopper
    {
        #region Construction
        protected override void SetCommands()
        {
            base.SetCommands();

            CartCommand = new Command<ProductsOverviewObject>(CartProduct);
        }
        #endregion

        #region Repositories
        private static ProductCategoriesRepository ProductCategoriesRepository => Startup.ServiceProvider.GetRequiredService<ProductCategoriesRepository>();
        private static ProductSubcategoriesRepository ProductSubcategoriesRepository => Startup.ServiceProvider.GetRequiredService<ProductSubcategoriesRepository>();
        private static ProductsRepository ProductsRepository => Startup.ServiceProvider.GetRequiredService<ProductsRepository>();
        #endregion

        #region Filtering

        // At least 3 characters.
        public static readonly BindableProperty ValidTextFilterExpressionProperty =
            BindableProperty.Create(nameof(ValidTextFilterExpression), typeof(string), typeof(ProductsViewModel), @"\w{3}");

        public string ValidTextFilterExpression
        {
            get { return (string)GetValue(ValidTextFilterExpressionProperty); }
            set
            {
                SetValue(ValidTextFilterExpressionProperty, value);
                RaisePropertyChanged(nameof(ValidTextFilterExpression));
            }
        }

        // TODO This would better be handled inside the repository.
        protected override async Task<bool> InitializeFilters()
        {
            var results = await Task.WhenAll
            (
                ProductCategoriesRepository.ReadList(),
                ProductSubcategoriesRepository.ReadList()
            ).ConfigureAwait(true);

            var succeeded = results.All<bool>(result => result);

            if (succeeded)
            // Note that using the UI thread (by BeginInvokeOnMainThread) only did bad.
            {
                var masterFilterItems = new ObservableCollection<ProductCategory>();

                foreach (var item in ProductCategoriesRepository.List)
                {
                    masterFilterItems.Add(item);
                }

                // Do an assignment, as just changing the ObservableCollection plus even a PropertyChanged does not work. There seems to be no good way to handle CollectionChanged. 
                // TODO maybe follow the approach on ItemsViewModel.Items.
                MasterFilterItems = masterFilterItems;

                foreach (var item in ProductSubcategoriesRepository.List)
                {
                    DetailFilterItemsSource.Add(item);
                }

                var retrievedCategoryId = Settings.ProductCategoryId;

                MasterFilterValue = retrievedCategoryId.HasValue
                    ? MasterFilterItems.FirstOrDefault(value => value.Id == retrievedCategoryId.Value)
                    : MasterFilterItems.FirstOrDefault(value => !value.IsEmpty);

                var retrievedSubcategoryId = Settings.ProductSubategoryId;

                // Note that MasterFilterValue also determines DetailFilterItems.
                DetailFilterValue = retrievedSubcategoryId.HasValue
                    ? DetailFilterItems.FirstOrDefault(value => value.Id == retrievedSubcategoryId.Value)
                    : DetailFilterItems.FirstOrDefault(value => !value.IsEmpty);

                // TODO This seems to work, but the view field is not updated.
                TextFilterValue = Settings.TextFilter;
            }

            return succeeded;
        }

        public new ProductCategory MasterFilterValue
        {
            get
            {
                return base.MasterFilterValue;
            }
            set
            {
                Settings.ProductCategoryId = value?.Id ?? null;
                base.MasterFilterValue = value;
            }
        }

        public new ProductSubcategory DetailFilterValue
        {
            get
            {
                return base.DetailFilterValue;
            }
            set
            {
                Settings.ProductSubategoryId = value?.Id ?? null;
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
                ((MasterFilterValue != null && !MasterFilterValue.IsEmpty) || 
                (!string.IsNullOrEmpty(TextFilterValue) && (Regex.IsMatch(TextFilterValue, @"\w{3}", RegexOptions.IgnoreCase))));
        }

        protected override async Task<bool> ReadFiltered()
        {
            // Note that using the UI thread (by BeginInvokeOnMainThread) only did bad.
            var masterFilterValue = MasterFilterValue;
            var detailFilterValue = DetailFilterValue;
            var textFilterValue = TextFilterValue;

            var result = await ProductsRepository.ReadList(masterFilterValue, detailFilterValue, textFilterValue).ConfigureAwait(true);
            var succeeded = result != null;

            if (succeeded)
                // Note that using the UI thread (by BeginInvokeOnMainThread) only did bad.
                foreach (var item in result)
                    Items.Add(item);

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
            var productView = new ProductView() { ViewModel = new ProductViewModel() { ItemId = productsOverviewObject.Id } };

            var wrapperViewModel = new ShoppingWrapperViewModel() { WrappedContent = productView };
            var wrapperView = new ShoppingWrapperView() { ViewModel = wrapperViewModel };

            await PushPage(wrapperView).ConfigureAwait(true);
        }
        #endregion

        #region Shopping
        public static readonly BindableProperty CartCommandProperty =
            BindableProperty.Create(nameof(CartCommand), typeof(ICommand), typeof(ProductsViewModel));

        public ICommand CartCommand
        {
            get { return (ICommand)GetValue(CartCommandProperty); }
            set
            {
                SetValue(CartCommandProperty, value);
                RaisePropertyChanged(nameof(CartCommand));
            }
        }

        private void CartProduct(ProductsOverviewObject productsOverviewObject)
        {
            // TODO Do this directly on the repository? (Might need initialisation first.)
            ShoppingCartViewModel.Instance.CartProduct(productsOverviewObject);
        }
        #endregion
    }
}