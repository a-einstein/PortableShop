using RCS.AdventureWorks.Common.DomainClasses;
using RCS.PortableShop.Common.ViewModels;
using RCS.PortableShop.Interfaces;
using RCS.PortableShop.Model;
using System.Windows.Input;
using Xamarin.Forms;

namespace RCS.PortableShop.ViewModels
{
    public class ProductViewModel : ItemViewModel<Product>, IShopper
    {
        #region Initialize
        protected override void SetCommands()
        {
            PhotoCommand = new Command<ImageSource>(ShowPhoto);
            CartCommand = new Command<Product>(CartProduct);
        }
        #endregion

        #region Filtering
        public override async void Refresh()
        {
            if (ItemId.HasValue)
                Item = await ProductsRepository.Instance.ReadDetails((int)ItemId);
        }
        #endregion

        #region Photo
        public ICommand PhotoCommand { get; private set; }

        // Use the existing ImageSource to avoid an unnecessary conversion.
        private void ShowPhoto(ImageSource imageSource)
        {
            var resources = Application.Current.Resources;

            var contentView = new ContentView()
            {
                // TODO Since Xamarin.Forms 2.3.4.224 the resource is no longer found here.
                // HACK The white, though it combines nicely with the white backgrounds of the current pictures.
                BackgroundColor = resources.ContainsKey("ProductsLevel1Colour") ? (Color)resources["ProductsLevel1Colour"] : Color.White,

                Content = new Image() { Source = imageSource }
            };

            PushPage(contentView, Item?.Name);
        }
        #endregion

        #region Shopping

        // Note this does not work as explicit interface implementation.
        public ICommand CartCommand { get; set; }

        private void CartProduct(Product product)
        {
            ShoppingCartViewModel.Instance.CartProduct(product);
        }
        #endregion
    }
}