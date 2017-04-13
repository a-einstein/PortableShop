using Android.App;
using Android.OS;
using RCS.PortableShop.Main;
using Xamarin.Forms.Platform.Android;

namespace RCS.PortableShop.Droid
{
    [Activity(
        Label = "Cyclone", 
        Icon = "@drawable/application", 
        Theme = "@style/MainTheme"
       )]
    public class MainActivity : FormsAppCompatActivity
    {
        protected override void OnCreate(Bundle bundle)
        {
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            base.OnCreate(bundle);

            global::Xamarin.Forms.Forms.Init(this, bundle);
            LoadApplication(new MainApplication());
        }
    }
}

