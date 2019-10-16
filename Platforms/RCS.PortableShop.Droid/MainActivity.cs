﻿using Android.App;
using Android.Content.PM;
using Android.OS;
using RCS.PortableShop.Main;
using System;
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

            // Note this currently would not do anything, as mentioned here:
            // https://forums.xamarin.com/discussion/149309/global-exception-handling
            // TODO Follow this matter.
            AppDomain.CurrentDomain.UnhandledException += CurrentDomainUnhandledException;

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

        private void CurrentDomainUnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            throw new NotImplementedException();
        }
    }
}

