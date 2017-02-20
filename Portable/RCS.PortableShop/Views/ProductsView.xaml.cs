using RCS.PortableShop.Common.Views;

namespace RCS.PortableShop.Views
{
    public partial class ProductsView : ImplicitModelView
    {
        public ProductsView()
        {
            InitializeComponent();
        }

        private int preservedActionBlockWidth = 0;
        private int preservedActionBlockHeight = 0;

        protected override void OnSizeAllocated(double width, double height)
        {
            base.OnSizeAllocated(width, height);

            Orientate(ref actionBlock, ref preservedActionBlockWidth, ref preservedActionBlockHeight, width, height);
        }
    }
}
