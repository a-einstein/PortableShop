using Microsoft.Extensions.Options;
using System;

namespace RCS.PortableShop.ServiceClients.Products.Wrappers
{
    public abstract class ServiceClient
    {
        #region Construction
        public ServiceClient(IOptions<ServiceOptions> serviceOptions)
        {
            ServiceOptions = serviceOptions.Value;
        }

        protected ServiceOptions ServiceOptions;
        #endregion


        // TODO Store these centrally or pass as parameter somewhere.
        // Probably use https://docs.microsoft.com/en-us/aspnet/core/fundamentals/configuration/options.

        public static TimeSpan Timeout { get; } = new TimeSpan(0, 0, 15);
    }
}
