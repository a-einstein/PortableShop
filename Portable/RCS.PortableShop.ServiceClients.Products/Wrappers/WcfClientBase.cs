using Microsoft.Extensions.Options;
using System.ServiceModel;
using System.ServiceModel.Channels;

namespace RCS.PortableShop.ServiceClients.Products.Wrappers
{
    public abstract class WcfClientBase : ServiceClient
    {
        #region Construction
        public WcfClientBase(IOptions<ServiceOptions> serviceOptions)
            : base(serviceOptions)
        { }
        #endregion

        #region ProductsServiceClient
        // Note that currently wsHttpBinding is not supported, but should be as it is part of System.ServiceModel 4.0.0.0.
        protected static Binding Binding()
        {
            int maxSize = 655360;

            var binding = new BasicHttpBinding(BasicHttpSecurityMode.Transport)
            {
                // TODO This might also be part of a configuration file.

                OpenTimeout = Timeout,
                SendTimeout = Timeout,
                ReceiveTimeout = Timeout,
                CloseTimeout = Timeout,

                // Note For TransferMode.Buffered, MaxReceivedMessageSize and MaxBufferSize must be the same value.
                // Arbitrary increased value (like on WPF) to prevent exception on larger query results.
                MaxReceivedMessageSize = maxSize,
                MaxBufferSize = maxSize,
                ReaderQuotas = System.Xml.XmlDictionaryReaderQuotas.Max,
                // TODO Should be paged.
                AllowCookies = true
            };

            return binding;
        }
        #endregion
    }
}
