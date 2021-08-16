using System;

namespace RCS.PortableShop.ServiceClients.Products.Wrappers
{
    // For some reason the libraries below are limited to 4.5.0, bound by UniversalWindowsPlatform.
    // - System.ServiceModel.Http
    //
    // The following issues may be related.
    // https://developercommunity.visualstudio.com/t/nugets-not-included-in-build/1278878
    // https://docs.microsoft.com/en-us/answers/questions/175752/cannot-load-systemservicemodelprimitivesdll-in-uwp.html
    // https://github.com/xamarin/Xamarin.Forms/issues/13868
    //
    // UWP currently only works for Release (with still a warning about System.Security.Principal.Windows).
    // Debug fails on Microsoft.Graphics.Canvas.
    //
    // Solution may be no sooner than .net 6, maybe migrating to MAUI as well.

    public abstract class ServiceClient
    {
        // TODO Store these centrally or pass as parameter somewhere.
        // Probably use https://docs.microsoft.com/en-us/aspnet/core/fundamentals/configuration/options.

        public static TimeSpan Timeout { get; } = new TimeSpan(0, 0, 15);

         // Note HttpClient.BaseAddress could also be set instead.
         protected const string serviceDomain = "https://rcsworks.nl";
    }
}
