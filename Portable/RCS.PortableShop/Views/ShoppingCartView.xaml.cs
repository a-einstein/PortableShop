using RCS.PortableShop.ViewModels;
//using System.ComponentModel.Composition;
using View = RCS.PortableShop.Common.Views.View;

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
