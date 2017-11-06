using RCS.AdventureWorks.Common.DomainClasses;
using RCS.PortableShop.Common.ViewModels;
using RCS.PortableShop.Interfaces;
using RCS.PortableShop.Model;
using RCS.PortableShop.Views;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace RCS.PortableShop.ViewModels
{
    public class ProductsViewModel : FilterItemsViewModel<ProductsOverviewObject, ProductCategory, ProductSubcategory>, IShopper
    {
        #region Construct
        protected override void SetCommands()
        {
            base.SetCommands();

            CartCommand = new Command<ProductsOverviewObject>(CartProduct);
        }
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
            bool succeeded;

            try
            {
                var results = await Task.WhenAll
                (
                    ProductCategoriesRepository.Instance.ReadList(),
                    ProductSubcategoriesRepository.Instance.ReadList()
                );

                succeeded = results.All<bool>(result => result == true);

                // Note that using the UI thread (by BeginInvokeOnMainThread) only did bad.

                ObservableCollection<ProductCategory> masterFilterItems = new ObservableCollection<ProductCategory>(); ;

                foreach (var item in ProductCategoriesRepository.Instance.List)
                {
                    masterFilterItems.Add(item);
                }

                // Do an assignment, as just changing the ObservableCollection plus even a PropertyChanged does not work. There seems to be no good way to handle CollectionChanged. 
                // TODO maybe follow the approach on ItemsViewModel.Items.
                MasterFilterItems = masterFilterItems;

                foreach (var item in ProductSubcategoriesRepository.Instance.List)
                {
                    detailFilterItemsSource.Add(item);
                }

                int masterDefaultId = 1;
                MasterFilterValue = MasterFilterItems.FirstOrDefault(category => category.Id == masterDefaultId);

                // Note that MasterFilterValue also determines DetailFilterItems.
                int detailDefaultId = 1;
                DetailFilterValue = DetailFilterItems.FirstOrDefault(subcategory => subcategory.Id == detailDefaultId);

                TextFilterValue = default(string);

            }
            catch (Exception)
            {
                succeeded = false;
            }

            return succeeded;
        }

        protected override async Task<bool> ReadFiltered()
        {
            ProductCategory masterFilterValue = null;
            ProductSubcategory detailFilterValue = null;
            string textFilterValue = null;

            // Note that using the UI thread (by BeginInvokeOnMainThread) only did bad.
            masterFilterValue = MasterFilterValue;
            detailFilterValue = DetailFilterValue;
            textFilterValue = TextFilterValue;

            var result = await ProductsRepository.Instance.ReadList(masterFilterValue, detailFilterValue, textFilterValue);
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

        #region Details
        protected override async void ShowDetails(ProductsOverviewObject productsOverviewObject)
        {
            var productView = new ProductView() { ViewModel = new ProductViewModel() { ItemId = productsOverviewObject.Id } };

            var wrapperViewModel = new ShoppingWrapperViewModel() { WrappedContent = productView };
            var wrapperView = new ShoppingWrapperView() { ViewModel = wrapperViewModel };

            await PushPage(wrapperView);
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
            ShoppingCartViewModel.Instance.CartProduct(productsOverviewObject);
        }
        #endregion
    }
}