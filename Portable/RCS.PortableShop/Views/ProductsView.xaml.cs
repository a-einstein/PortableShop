using View = RCS.PortableShop.Common.Views.View;

namespace RCS.PortableShop.Views
{
    public partial class ProductsView : View
    {
        public ProductsView()
        {
            InitializeComponent();
        }

        private int preservedActionBlockWidth;
        private int preservedActionBlockHeight;

        protected override void OnSizeAllocated(double width, double height)
        {
            base.OnSizeAllocated(width, height);

            Orientate(ref actionBlock, ref preservedActionBlockWidth, ref preservedActionBlockHeight, width, height);
        }
    }
}
