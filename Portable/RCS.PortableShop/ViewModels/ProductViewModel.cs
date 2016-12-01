using Prism.Commands;
using RCS.AdventureWorks.Common.DomainClasses;
using RCS.WpfShop.Common.ViewModels;
using RCS.WpfShop.Modules.Products.Model;
using System.Windows.Input;

namespace RCS.WpfShop.Modules.Products.ViewModels
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

            CartCommand = new DelegateCommand<Product>(CartProduct);
        }

        // Note this does not work as explicit interface implementation.
        public ICommand CartCommand { get; set; }

        private void CartProduct(Product product)
        {
            ShoppingCartViewModel.Instance.CartProduct(product);
        }
    }
}