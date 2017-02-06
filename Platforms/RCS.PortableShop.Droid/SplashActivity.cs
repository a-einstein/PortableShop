using Android.App;
using Android.OS;

namespace RCS.PortableShop.Droid
{
    [Activity(
        Theme = "@style/PortableShopTheme.Splash", 
        MainLauncher = true, 
        NoHistory = true
        )]
    public class SplashActivity : Activity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            StartActivity(typeof(MainActivity));
        }
    }
}