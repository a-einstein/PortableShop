using CommunityToolkit.Mvvm.Input;
using RCS.AdventureWorks.Common.DomainClasses;
using RCS.AdventureWorks.Common.Interfaces;
using RCS.PortableShop.Common.Interfaces;
using RCS.PortableShop.Common.ViewModels;
using RCS.PortableShop.Interfaces;
using RCS.PortableShop.Main;
using System.Windows.Input;
using View = RCS.PortableShop.Common.Views.View;

namespace RCS.PortableShop.ViewModels
{
    public class ProductViewModel :
        ItemViewModel<Product>, IShopper
    {
        #region Construction
        public ProductViewModel(IFilterRepository<List<ProductsOverviewObject>, ProductsOverviewObject, ProductCategory, ProductSubcategory, int> productsRepository)
        {
            ProductsRepository = productsRepository;
        }

        protected override void SetCommands()
        {
            base.SetCommands();

            // TODO MAUI Check out RelayCommand attribute, including CanExecute attribute.
            PhotoCommand = new AsyncRelayCommand<ImageSource>(ShowPhoto);
            CartCommand = new AsyncRelayCommand<IShoppingProduct>(CartProduct);
        }
        #endregion

        #region Services
        private IFilterRepository<List<ProductsOverviewObject>, ProductsOverviewObject, ProductCategory, ProductSubcategory, int> ProductsRepository { get; }

        private static CartViewModel CartViewModel => Startup.ServiceProvider.GetRequiredService<CartViewModel>();
        #endregion

        #region Refresh
        protected override async Task Read()
        {
            if (ItemId.HasValue)
            {
                var result = await ProductsRepository.Details((int)ItemId).ConfigureAwait(true);
                Item = result;
            }

            await base.Read();
        }
        #endregion

        #region Photo

        private static readonly BindableProperty PhotoCommandProperty =
            BindableProperty.Create(nameof(PhotoCommand), typeof(ICommand), typeof(ProductViewModel));

        public ICommand PhotoCommand
        {
            get => (ICommand)GetValue(PhotoCommandProperty);
            private set => SetValue(PhotoCommandProperty, value);
        }

        // Use the existing ImageSource to avoid an unnecessary conversion.
        private async Task ShowPhoto(ImageSource imageSource)
        {
            var resources = Application.Current.Resources;

            var backgroundColor = Colors.White;

            // Note colours may be part of themes.
            // TODO Find a more elegant method, possibly with LINQ. Though this may even be fastest.
            // https://code-maze.com/csharp-how-to-merge-dictionaries/
            foreach (var dictionary in resources.MergedDictionaries)
            {
                if (dictionary.ContainsKey("ProductsLevel1Colour"))
                {
                    backgroundColor = ((Color)dictionary["ProductsLevel1Colour"]);
                }
            }

            var contentView = new View()
            {
                BackgroundColor = backgroundColor,
                Content = new Image() { Source = imageSource }
            };

            await PushPage(contentView, Title).ConfigureAwait(true);
        }
        #endregion

        #region Shopping

        private static readonly BindableProperty CartCommandProperty =
            BindableProperty.Create(nameof(CartCommand), typeof(ICommand), typeof(ProductViewModel));

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