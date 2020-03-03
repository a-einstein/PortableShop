using Android.App;
using Android.Content.PM;
using Android.OS;
using RCS.PortableShop.Main;
using System;
using Xamarin.Forms.Platform.Android;

namespace RCS.PortableShop.Droid
{
    [Activity(
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

            // Note this currently would not do anything, as mentioned here:
            // https://forums.xamarin.com/discussion/149309/global-exception-handling
            // TODO Follow this matter.
            AppDomain.CurrentDomain.UnhandledException += CurrentDomainUnhandledException;

            // TODO Check and apply functionalities.
            // https://docs.microsoft.com/en-gb/xamarin/essentials/

            // https://docs.microsoft.com/en-gb/xamarin/essentials/get-started
            Xamarin.Essentials.Platform.Init(this, bundle);

            Xamarin.Forms.Forms.Init(this, bundle);
            LoadApplication(new MainApplication());
        }

        private static void CurrentDomainUnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            throw new NotImplementedException();
        }

        // https://docs.microsoft.com/en-gb/xamarin/essentials/get-started
        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
    }
}

