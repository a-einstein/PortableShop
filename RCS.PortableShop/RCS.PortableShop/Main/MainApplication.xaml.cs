using Xamarin.Forms;

namespace RCS.PortableShop.Main
{
    public partial class MainApplication : Application
    {
        public MainApplication()
        {
            InitializeComponent();

            MainPage = new MainWindow();
        }

        protected override void OnStart()
        {
            // Handle when your app starts
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }
}
