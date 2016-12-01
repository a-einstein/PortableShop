using RCS.PortableShop.Main;
namespace RCS.PortableShop.Windows
{
    public sealed partial class MainPage
    {
        public MainPage()
        {
            this.InitializeComponent();

            LoadApplication(new MainApplication());
        }
    }
}
