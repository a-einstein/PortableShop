using Microsoft.Extensions.DependencyInjection;
using RCS.AdventureWorks.Common.DomainClasses;
using RCS.PortableShop.Common.ViewModels;
using RCS.PortableShop.Interfaces;
using RCS.PortableShop.Main;
using RCS.PortableShop.Model;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace RCS.PortableShop.ViewModels
{
    public class ProductViewModel : ItemViewModel<Product>, IShopper
    {
        #region Construction
        protected override void SetCommands()
        {
            base.SetCommands();

            PhotoCommand = new Command<ImageSource>(ShowPhoto);
            CartCommand = new Command<Product>(CartProduct);
        }
        #endregion

        #region Repositories
        private static ProductsRepository ProductsRepository => Startup.ServiceProvider.GetRequiredService<ProductsRepository>();
        #endregion

        #region Refresh
        protected override async Task Read()
        {
            if (ItemId.HasValue)
            {
                var result = await ProductsRepository.ReadDetails((int)ItemId).ConfigureAwait(true);
                Item = result;
            }
        }
        #endregion

        #region Photo
        public static readonly BindableProperty PhotoCommandProperty =
            BindableProperty.Create(nameof(PhotoCommand), typeof(ICommand), typeof(ProductViewModel));

        public ICommand PhotoCommand
        {
            get { return (ICommand)GetValue(PhotoCommandProperty); }
            private set
            {
                SetValue(PhotoCommandProperty, value);
                RaisePropertyChanged(nameof(PhotoCommand));
            }
        }

        // Use the existing ImageSource to avoid an unnecessary conversion.
        private async void ShowPhoto(ImageSource imageSource)
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
        public static readonly BindableProperty CartCommandProperty =
            BindableProperty.Create(nameof(CartCommand), typeof(ICommand), typeof(ProductViewModel));

        public ICommand CartCommand
        {
            get { return (ICommand)GetValue(CartCommandProperty); }
            set
            {
                SetValue(CartCommandProperty, value);
                RaisePropertyChanged(nameof(CartCommand));
            }
        }

        private void CartProduct(Product product)
        {
            // TODO Do this directly on the repository? (Might need initialisation first.)
            ShoppingCartViewModel.Instance.CartProduct(product);
        }
        #endregion
    }
}