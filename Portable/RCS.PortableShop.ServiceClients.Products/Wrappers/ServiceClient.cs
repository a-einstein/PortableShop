using System;

namespace RCS.PortableShop.ServiceClients.Products.Wrappers
{
    public abstract class ServiceClient
    {
        // TODO Store these centrally or pass as parameter somewhere.
        // Probably use https://docs.microsoft.com/en-us/aspnet/core/fundamentals/configuration/options.

        public static TimeSpan Timeout { get; } = new TimeSpan(0, 0, 15);

         // Note HttpClient.BaseAddress could also be set instead.
         protected const string serviceDomain = "https://rcsworks.nl";
    }
}
