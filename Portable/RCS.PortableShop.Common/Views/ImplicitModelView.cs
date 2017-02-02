using RCS.PortableShop.Common.ViewModels;

namespace RCS.PortableShop.Common.Views
{
    public class ImplicitModelView : View
    {
        public new ViewModel ViewModel
        {
            get { return base.ViewModel; }
            protected set { base.ViewModel = value; }
        }
    }
}
