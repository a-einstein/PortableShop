﻿using RCS.PortableShop.Resources;
using System.Diagnostics;
using System.Reflection;

// Note this is on assembly level. The stated namespace below does not seem to matter.
// I applied this on all assemblies containing XAML. I also tried this separately on the classes at first.
// It all turned out to be fragile, sometimes causing compilation problems.
// Check out the settings of and comments on XamlCompilation elsewhere.
//[assembly: XamlCompilation(XamlCompilationOptions.Compile)]

namespace RCS.PortableShop.Main
{
    // TODO Make naming consistent. Take platforms into account. 
    public partial class App : Application
    {
        private const string debugPrefix = ">>>> Debug:";

        public App()
        {
            InitializeComponent();

            //OnStart();
            StartActions();
        }

        private static void ListResources()
        {
            ListAssemblyResources(typeof(Labels).GetTypeInfo().Assembly);
        }

        private static void ListAssemblyResources(Assembly assembly)
        {
            foreach (var resource in assembly.GetManifestResourceNames())
                Debug.WriteLine($"{debugPrefix} Found resource: {resource}");
        }

        // TODO Obsolete?
        protected override void OnStart()
        {
            base.OnStart();
            //StartActions();
        }

        private void StartActions()
        {
#if DEBUG
            ListResources();
#endif

            //Startup.Init();

            // Currently this crashes for exclusively the RELEASE build of x86 and x64.
            // This might be related to https://github.com/xamarin/Xamarin.Forms/issues/11736
            // TODO follow that issue and test for new releases.

            // Note this needs to be on the main thread.
            MainPage = new MainShell();
        }
    }
}
