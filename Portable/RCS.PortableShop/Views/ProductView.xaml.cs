using RCS.PortableShop.Common.Views;

namespace RCS.PortableShop.Views
{
    public partial class ProductView : View
    {
        public ProductView()
        {
            InitializeComponent();
        }

        private int preservedProductControlWidth = 0;
        private int preservedProductControlHeight = 0;

        protected override void OnSizeAllocated(double width, double height)
        {
            base.OnSizeAllocated(width, height);

            Orientate(ref productControl, ref preservedProductControlWidth, ref preservedProductControlHeight,  width, height);
        }
    }
}
