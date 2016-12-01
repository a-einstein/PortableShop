using RCS.PortableShop.Common.Views;
using RCS.PortableShop.ViewModels;
using System.ComponentModel.Composition;

namespace RCS.PortableShop.Views
{
    public partial class ShoppingCartView : View
    {
        public ShoppingCartView()
        {
            InitializeComponent();
        }

        // Note this couples to a specific class.
        // To avoid this the ViewModel should be set by an explicit import again.
        // There seem to be no other options on the attribute.
        public ShoppingCartView(ShoppingCartViewModel viewModel)
            : this()
        {
            ViewModel = viewModel;
        }
    }
}
