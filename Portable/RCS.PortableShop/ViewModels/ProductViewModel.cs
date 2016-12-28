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
        public override async void Refresh(object productId)
        {
            // TODO Check for errors.
            Item = await ProductsRepository.Instance.ReadDetails((int)productId);
        }

        protected override void SetCommands()
        {
            base.SetCommands();

            PhotoCommand = new Command<ImageSource>(ShowPhoto);
            CartCommand = new Command<Product>(CartProduct);
        }

        public ICommand PhotoCommand { get; private set; }

        // Use the existing ImageSource to avoid an unnecessary conversion.
        private void ShowPhoto(ImageSource imageSource)
        {
            var page = new ContentPage()
            {
                Content = new Image() { Source = imageSource }
            };

            Navigation.PushAsync(page);
        }

        // Note this does not work as explicit interface implementation.
        public ICommand CartCommand { get; set; }

        private void CartProduct(Product product)
        {
            ShoppingCartViewModel.Instance.CartProduct(product);
        }
    }
}