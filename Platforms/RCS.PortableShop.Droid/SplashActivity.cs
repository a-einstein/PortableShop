using Android.App;
using Android.Content;
using AndroidX.AppCompat.App;
using System.Threading.Tasks;

namespace RCS.PortableShop.Droid
{
    [Activity(
        Theme = "@style/PortableShopTheme.Splash",
        MainLauncher = true,
        NoHistory = true
        )]
    // TODO This often does not appear while starting in landscape orientation. But sometimes it does, no idea what determines that.
    public class SplashActivity : AppCompatActivity
    {
        // https://docs.microsoft.com/en-gb/xamarin/essentials/get-started
        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }

        protected override void OnResume()
        {
            base.OnResume();

            // TODO There is a black 'gap' between the the SplashActivity and the MainActivity, apparently since last changes, 
            Task.Factory.StartNew(() => { StartActivity(new Intent(Application.Context, typeof(MainActivity))); });
        }
    }
}