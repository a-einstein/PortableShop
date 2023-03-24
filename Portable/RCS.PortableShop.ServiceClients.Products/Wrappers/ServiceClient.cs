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
        
        // TODO This might also be part of a configuration file.
        public static TimeSpan Timeout { get; } = new TimeSpan(0, 0, 15);
    }
}
