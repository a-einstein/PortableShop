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

            var tasks = new Task[]
            {
                ProductCategoriesRepository.Refresh(),
                ProductSubcategoriesRepository.Refresh()
            };

            await Task.WhenAll(tasks).ConfigureAwait(true);

            var categories = ProductCategoriesRepository.Items;
            var masterFilterItems = new ObservableCollection<ProductCategory>();

            foreach (var item in categories)
            {
                masterFilterItems.Add(item);
            }

            MainThread.BeginInvokeOnMainThread(() =>
            {
                // Do an assignment, as there is not much use to follow up on each item. 
                // TODO maybe follow the approach on ItemsViewModel.Items.
                MasterFilterItems = masterFilterItems;

                var subcategories = ProductSubcategoriesRepository.Items;

                foreach (var item in subcategories)
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
                // TODO Fix this, as it may even result in empty query results.
                TextFilterValue = Settings.TextFilter;
            });

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

        public new ProductCategory MasterFilterValue
        {
            get => base.MasterFilterValue;
            set
            {
                Settings.ProductCategoryId = value?.Id ?? null;
                base.MasterFilterValue = value;
            }
        }

        public new ProductSubcategory DetailFilterValue
        {
            get => base.DetailFilterValue;
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
                MainThread.BeginInvokeOnMainThread(() =>
                {
                    foreach (var item in ProductsRepository.Items)
                        Items.Add(item);
                });

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
            get => (ICommand)GetValue(CartCommandProperty);
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