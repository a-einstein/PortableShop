using System;

namespace RCS.PortableShop.ServiceClients.Products.Wrappers
{
    // Note that for UWP the following libraries had to be reduced to version 4.5.3.
    //
    // For unknown reasons the versions 4.6.0, 4.7.0, 4.8.1 resulted in an exception on System.ServiceModel.Primitives.
    // Explicitly adding that library or one of the dependent ones  did not help.
    // - System.ServiceModel.Duplex
    // - System.ServiceModel.Http
    // - System.ServiceModel.NetTcp
    //
    // The following issues may be related.
    // https://developercommunity.visualstudio.com/t/nugets-not-included-in-build/1278878
    // https://docs.microsoft.com/en-us/answers/questions/175752/cannot-load-systemservicemodelprimitivesdll-in-uwp.html
    // https://github.com/xamarin/Xamarin.Forms/issues/13868

    public abstract class ServiceClient
    {
        // TODO Store these centrally or pass as parameter somewhere.
        // Probably use https://docs.microsoft.com/en-us/aspnet/core/fundamentals/configuration/options.

        public static TimeSpan Timeout { get; } = new TimeSpan(0, 0, 15);

         // Note HttpClient.BaseAddress could also be set instead.
         protected const string serviceDomain = "https://rcsworks.nl";
    }
}
