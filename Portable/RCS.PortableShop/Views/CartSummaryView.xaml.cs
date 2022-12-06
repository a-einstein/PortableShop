using RCS.PortableShop.Main;
using RCS.PortableShop.ViewModels;
using View = RCS.PortableShop.Common.Views.View;

namespace RCS.PortableShop.Views
{
    public partial class CartSummaryView : View
    {
        public CartSummaryView()
        {
            InitializeComponent();

            ViewModel = CartViewModel;
        }

        #region Services
        private static CartViewModel CartViewModel => Startup.ServiceProvider.GetRequiredService<CartViewModel>();
        #endregion
    }
}
