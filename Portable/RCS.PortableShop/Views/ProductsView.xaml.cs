using RCS.PortableShop.ViewModels;
//using System.ComponentModel.Composition;
using View = RCS.PortableShop.Common.Views.View;

namespace RCS.PortableShop.Views
{
    // TODO This way of ordering actually does not work. Also see elsewhere.
    //[ViewSortHint("20")]
    public partial class ProductsView : View
    {
        public ProductsView()
        {
            InitializeComponent();
        }

        // Note this couples to a specific class.
        // To avoid this the ViewModel should be set by an explicit import again.
        // There seem to be no other options on the attribute.
        public ProductsView(ProductsViewModel viewModel)
            : this()
        {
            ViewModel = viewModel;
        }
    }
}
