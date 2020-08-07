using System;

namespace RCS.PortableShop.ServiceClients.Products.Wrappers
{
    // Note that for UWP the following libraries had to be reduced to version 4.5.3.
    // For unknown reasons the versions 4.6.0 and 4.7.0 resulted in an exception on System.ServiceModel.Primitives.
    // - System.ServiceModel.Duplex
    // - System.ServiceModel.Http
    // - System.ServiceModel.NetTcp
    // - System.ServiceModel.Primitives
    //
    // The following issues may be related.
    // https://stackoverflow.com/questions/57094843/nuget-hell-cannot-reference-nuget-package-in-uwp-application-due-to-public-key
    // https://github.com/dotnet/wcf/issues/3743

    public abstract class ServiceClient
    {
        // TODO Store these centrally or pass as parameter somewhere.
        // Probably use https://docs.microsoft.com/en-us/aspnet/core/fundamentals/configuration/options.

        public static TimeSpan Timeout { get; } = new TimeSpan(0, 0, 15);

         // Note HttpClient.BaseAddress could also be set instead.
         protected const string serviceDomain = "https://rcsworks.nl";
    }
}
