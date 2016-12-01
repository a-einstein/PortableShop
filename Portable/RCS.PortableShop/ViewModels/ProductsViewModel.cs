using Prism.Commands;
using RCS.AdventureWorks.Common.DomainClasses;
using RCS.WpfShop.Common.ViewModels;
using RCS.WpfShop.Common.Views;
using RCS.WpfShop.Common.Windows;
using RCS.WpfShop.Modules.Products.Model;
using RCS.WpfShop.Modules.Products.Views;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Threading;

namespace RCS.WpfShop.Modules.Products.ViewModels
{
    [Export]
    public class ProductsViewModel : FilterItemsViewModel<ProductsOverviewObject, ProductCategory, ProductSubcategory>, IShopper, IPartImportsSatisfiedNotification
    {
        private Dispatcher uiDispatcher;

        public ProductsViewModel()
        {
            uiDispatcher = Dispatcher.CurrentDispatcher;
        }

        private bool filterInitialized;

        // Note this also works without actual imports.
        // TODO This seems to come too early, before navigation.
        public void OnImportsSatisfied()
        {
            Refresh();
        }

        public override void Refresh()
        {
            Items.Clear();

            if (!filterInitialized)
            {
                Task.Run(async () => await InitializeFilters()).
                ContinueWith((previous) => ReadFiltered());
            }
            else
            {
                ReadFiltered();
            }
        }

        // TODO This would better be handled inside the repository.
        protected override async Task InitializeFilters()
        {
            await Task.WhenAll
            (
                ProductCategoriesRepository.Instance.ReadList(),
                ProductSubcategoriesRepository.Instance.ReadList()
            ).ContinueWith((previous) =>
            {
                // Need to update on the UI thread.
                uiDispatcher.Invoke(delegate
                {
                    foreach (var item in ProductCategoriesRepository.Instance.List)
                    {
                        MasterFilterItems.Add(item);
                    }

                    // To trigger the enablement.
                    RaisePropertyChanged(nameof(MasterFilterItems));

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
                });
            });
        }

        protected void ReadFiltered()
        {
            ProductCategory masterFilterValue = null;
            ProductSubcategory detailFilterValue = null;
            string textFilterValue = null;

            // Need to get these from the UI thread.
            uiDispatcher.Invoke(delegate
            {
                masterFilterValue = MasterFilterValue;
                detailFilterValue = DetailFilterValue;
                textFilterValue = TextFilterValue;
            });

            Task<IList<ProductsOverviewObject>>.Run(() =>
            {
                return ProductsRepository.Instance.ReadList(masterFilterValue, detailFilterValue, textFilterValue).Result;
            })
            .ContinueWith((previous) =>
            {
                // Need to update on the UI thread.
                uiDispatcher.Invoke(delegate
                {
                    foreach (var item in previous.Result)
                    {
                        Items.Add(item);
                    }

                    RaisePropertyChanged(nameof(ItemsCount));
                });
            });
        }

        protected override Func<ProductSubcategory, bool> DetailFilterItemsSelector(bool preserveEmptyElement = true)
        {
            return subcategory =>
                MasterFilterValue != null &&
                !MasterFilterValue.IsEmpty &&
                (preserveEmptyElement && subcategory.IsEmpty || subcategory.ProductCategoryId == MasterFilterValue.Id);
        }

        protected override void SetCommands()
        {
            base.SetCommands();

            CartCommand = new DelegateCommand<ProductsOverviewObject>(CartProduct);
        }

        protected override void ShowDetails(ProductsOverviewObject productsOverviewObject)
        {
            ProductViewModel productViewModel = new ProductViewModel();
            View productView = new ProductView() { ViewModel = productViewModel };

            OkWindow productWindow = new OkWindow() { View = productView, };
            productWindow.SetBinding(Window.TitleProperty, new Binding($"{nameof(productViewModel.Item)}.{nameof(productsOverviewObject.Name)}") { Source = productViewModel });
            productWindow.Show();

            productViewModel.Refresh(productsOverviewObject.Id);
        }

        // Note this does not work as explicit interface implementation.
        public ICommand CartCommand { get; set; }

        private void CartProduct(ProductsOverviewObject productsOverviewObject)
        {
            ShoppingCartViewModel.Instance.CartProduct(productsOverviewObject);
        }
    }
}
