using Android.App;
using Android.Content.PM;
using Android.OS;
using RCS.PortableShop.Main;
using System.Net;
using Xamarin.Forms.Platform.Android;

namespace RCS.PortableShop.Droid
{
    [Activity(
        Label = "CyclOne", 
        Icon = "@drawable/application", 
        Theme = "@style/MainTheme",
        // Prevent calls to OnCreate on orientation change and on background state.
        ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation
       )]
    public class MainActivity : FormsAppCompatActivity
    {
        protected override void OnCreate(Bundle bundle)
        {
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            base.OnCreate(bundle);

            ServicePointManager.ServerCertificateValidationCallback = delegate 
            {
                // Note that a simple exported self certified crt file does not really get installed on Android.
                // TODO Improve this for own server.
                // HACK
                return true;
            };

            global::Xamarin.Forms.Forms.Init(this, bundle);
            LoadApplication(new MainApplication());
        }
    }
}

