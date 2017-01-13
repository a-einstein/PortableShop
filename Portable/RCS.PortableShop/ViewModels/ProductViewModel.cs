using RCS.AdventureWorks.Common.DomainClasses;
using RCS.PortableShop.Common.ViewModels;
using RCS.PortableShop.Interfaces;
using RCS.PortableShop.Model;
using RCS.PortableShop.Resources;
using RCS.PortableShop.Views;
using System.Windows.Input;
using Xamarin.Forms;

namespace RCS.PortableShop.ViewModels
{
    public class ProductViewModel : ItemViewModel<Product>, IShopper
    {
        #region Filtering
        public override async void Refresh(object productId)
        {
            // TODO Check for errors.
            Item = await ProductsRepository.Instance.ReadDetails((int)productId);
        }
        #endregion

        #region Initialize
        protected override void SetCommands()
        {
            base.SetCommands();

            PhotoCommand = new Command<ImageSource>(ShowPhoto);
            CartCommand = new Command<Product>(CartProduct);
            ShowCartCommand = new Command(ShowCart);
        }
        #endregion

        #region Photo
        public ICommand PhotoCommand { get; private set; }

        // Use the existing ImageSource to avoid an unnecessary conversion.
        private void ShowPhoto(ImageSource imageSource)
        {
            PushPage(new Image() { Source = imageSource }, Item.Name);
        }
        #endregion

        #region Shopping

        // Note this does not work as explicit interface implementation.
        public ICommand CartCommand { get; set; }

        private void CartProduct(Product product)
        {
            ShoppingCartViewModel.Instance.CartProduct(product);
        }

        // TODO Maybe turn IShopper into a base class.
        public ICommand ShowCartCommand { get; set; }

        protected void ShowCart()
        {
            var viewModel = ShoppingCartViewModel.Instance;
            viewModel.Navigation = Navigation;

            var view = new ShoppingCartView() { ViewModel = viewModel };

            PushPage(view, Labels.Cart);

            viewModel.Refresh();
        }
        #endregion
    }
}