using RCS.PortableShop.Common.Views;
using Xamarin.Forms.Xaml;

namespace RCS.PortableShop.Views
{
    // HACK Prevents compilation error for unknown reason.
    [XamlCompilation(XamlCompilationOptions.Skip)]
    public partial class ShoppingCartSummaryView : ImplicitModelView
    {
        public ShoppingCartSummaryView()
        {
            InitializeComponent();
        }
    }
}
