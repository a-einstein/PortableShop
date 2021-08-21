using Microsoft.Extensions.DependencyInjection;
using RCS.PortableShop.Common.Views;
using RCS.PortableShop.Main;
using RCS.PortableShop.ViewModels;

namespace RCS.PortableShop.Views
{
    public partial class ShoppingCartSummaryView : View
    {
        public ShoppingCartSummaryView()
        {
            InitializeComponent();

            ViewModel = ShoppingCartViewModel;
        }

        #region Services
        private static ShoppingCartViewModel ShoppingCartViewModel => Startup.ServiceProvider.GetRequiredService<ShoppingCartViewModel>();
        #endregion
    }
}
