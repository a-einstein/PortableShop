using RCS.AdventureWorks.Common.DomainClasses;
using RCS.PortableShop.Common.ViewModels;
using RCS.PortableShop.Interfaces;
using RCS.PortableShop.Model;
using RCS.PortableShop.Resources;
using RCS.PortableShop.Views;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using View = RCS.PortableShop.Common.Views.View;

namespace RCS.PortableShop.ViewModels
{
    public class ProductsViewModel : FilterItemsViewModel<ProductsOverviewObject, ProductCategory, ProductSubcategory>, IShopper
    {
        public ProductsViewModel()
        {
            Refresh();
        }

        #region Filtering
        private bool filterInitialized;

        public override async void Refresh()
        {
            Items.Clear();

            if (!filterInitialized)
                await InitializeFilters();

            await ReadFiltered();
        }

        // TODO This would better be handled inside the repository.
        protected override async Task InitializeFilters()
        {
            await Task.WhenAll
            (
                ProductCategoriesRepository.Instance.ReadList(),
                ProductSubcategoriesRepository.Instance.ReadList()
            );

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

            filterInitialized = true;
        }

        protected async Task ReadFiltered()
        {
            ProductCategory masterFilterValue = null;
            ProductSubcategory detailFilterValue = null;
            string textFilterValue = null;

            // Note that using the UI thread (by BeginInvokeOnMainThread) only did bad.
            masterFilterValue = MasterFilterValue;
            detailFilterValue = DetailFilterValue;
            textFilterValue = TextFilterValue;

            var productsOverviewObjects = await ProductsRepository.Instance.ReadList(masterFilterValue, detailFilterValue, textFilterValue);

            // Note that using the UI thread (by BeginInvokeOnMainThread) only did bad.
            foreach (var item in productsOverviewObjects)
            {
                Items.Add(item);
            }
        }

        protected override Func<ProductSubcategory, bool> DetailFilterItemsSelector(bool preserveEmptyElement = true)
        {
            return subcategory =>
                MasterFilterValue != null &&
                !MasterFilterValue.IsEmpty &&
                (preserveEmptyElement && subcategory.IsEmpty || subcategory.ProductCategoryId == MasterFilterValue.Id);
        }
        #endregion

        #region Initialize
        protected override void SetCommands()
        {
            base.SetCommands();

            CartCommand = new Command<ProductsOverviewObject>(CartProduct);
            ShowCartCommand = new Command(ShowCart);
        }
        #endregion

        #region Details
        protected override void ShowDetails(ProductsOverviewObject productsOverviewObject)
        {
            var viewModel = new ProductViewModel() { Navigation = Navigation };
            var view = new ProductView() { ViewModel = viewModel };

            PushPage(view, productsOverviewObject.Name);

            viewModel.Refresh(productsOverviewObject.Id);
        }
        #endregion

        #region Shopping

        // Note this does not work as explicit interface implementation.
        public ICommand CartCommand { get; set; }

        private void CartProduct(ProductsOverviewObject productsOverviewObject)
        {
            ShoppingCartViewModel.Instance.CartProduct(productsOverviewObject);
        }

        // TODO Maybe turn IShopper into a base class.
        public ICommand ShowCartCommand { get; set; }

        protected void ShowCart()
        {
            // Note this view has got an implicit ViewModel;
            var view = new ShoppingCartView();
            view.ViewModel.Navigation = Navigation;

            PushPage(view, Labels.Cart);
        }
        #endregion
    }
}
