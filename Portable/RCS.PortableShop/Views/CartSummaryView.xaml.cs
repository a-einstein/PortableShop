using Microsoft.Extensions.DependencyInjection;
using RCS.PortableShop.Common.Views;
using RCS.PortableShop.Main;
using RCS.PortableShop.ViewModels;

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
