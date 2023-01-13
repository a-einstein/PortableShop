using View = RCS.PortableShop.Common.Views.View;

namespace RCS.PortableShop.Views
{
    public partial class ProductView : View
    {
        public ProductView()
        {
            InitializeComponent();
        }

        private int preservedProductControlWidth;
        private int preservedProductControlHeight;

        protected override void OnSizeAllocated(double width, double height)
        {
            base.OnSizeAllocated(width, height);

            Orientate(ref productControl, ref preservedProductControlWidth, ref preservedProductControlHeight, width, height);
        }
    }
}
