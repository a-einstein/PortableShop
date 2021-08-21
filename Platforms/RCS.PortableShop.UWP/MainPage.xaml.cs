using RCS.PortableShop.Main;
namespace RCS.PortableShop.UWP
{
    public sealed partial class MainPage
    {
        public MainPage()
        {
            InitializeComponent();

            LoadApplication(new MainApplication());
        }
    }
}
