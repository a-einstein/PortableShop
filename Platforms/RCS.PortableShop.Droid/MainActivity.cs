﻿using Android.App;
using Android.Content.PM;
using Android.OS;
using RCS.PortableShop.Main;

namespace RCS.PortableShop.Droid
{
    [Activity(
        Label = "Cyclone", 
        Icon = "@drawable/Cyclone", 
        Theme = "@style/MainTheme", 
        MainLauncher = false, 
        ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation
        )]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
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

