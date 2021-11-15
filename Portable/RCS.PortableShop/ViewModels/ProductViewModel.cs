using Microsoft.Extensions.DependencyInjection;
using RCS.AdventureWorks.Common.DomainClasses;
using RCS.AdventureWorks.Common.Interfaces;
using RCS.PortableShop.Common.Interfaces;
using RCS.PortableShop.Common.ViewModels;
using RCS.PortableShop.Interfaces;
using RCS.PortableShop.Main;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.CommunityToolkit.ObjectModel;
using Xamarin.Forms;

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

            PhotoCommand = new AsyncCommand<ImageSource>(ShowPhoto);
            CartCommand = new AsyncCommand<IShoppingProduct>(CartProduct);
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

            var contentView = new ContentView()
            {
                // TODO Since Xamarin.Forms 2.3.4.224 the resource is no longer found here.
                // HACK The white, though it combines nicely with the white backgrounds of the current pictures.
                BackgroundColor = resources.ContainsKey("ProductsLevel1Colour") ? (Color)resources["ProductsLevel1Colour"] : Color.White,

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