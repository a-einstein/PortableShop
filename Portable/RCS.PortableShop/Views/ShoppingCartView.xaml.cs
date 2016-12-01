using RCS.WpfShop.Common.Views;
using RCS.WpfShop.Modules.Products.ViewModels;
using System.ComponentModel.Composition;

namespace RCS.WpfShop.Modules.Products.Views
{
    [Export]
    public partial class ShoppingCartView : View
    {
        public ShoppingCartView()
        {
            InitializeComponent();
        }

        // Note this couples to a specific class.
        // To avoid this the ViewModel should be set by an explicit import again.
        // There seem to be no other options on the attribute.
        [ImportingConstructor]
        public ShoppingCartView(ShoppingCartViewModel viewModel)
            : this()
        {
            ViewModel = viewModel;
        }
    }
}
