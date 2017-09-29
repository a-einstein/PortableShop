using Android.App;
using Android.Content;
using Android.Support.V7.App;
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
        protected override void OnResume()
        {
            base.OnResume();

            // TODO There is a black 'gap' between the the SplashActivity and the MainActivity, apparently since last changes, 
            Task.Factory.StartNew(() => { StartActivity(new Intent(Application.Context, typeof(MainActivity))); });
        }
    }
}