using RCS.PortableShop.ServiceClients.Products.ProductsService;
using System;
using System.ServiceModel;

namespace RCS.PortableShop.Model
{
    public abstract class ProductsServiceConsumer : IDisposable
    {
        #region Constants
        static public TimeSpan Timeout { get; } = new TimeSpan(0, 0, 15);

        public enum Errors
        {
            ServiceError
        }
        #endregion

        #region Service
        private ProductsServiceClient productsServiceClient;

        protected IProductsService ProductsServiceClient
        {
            get
            {
                // TODO Make this better configurable. There normally does not seem to be an .config file.
                // Note this points to a BasicHttpBinding variant.
                const string endpointAddress = "http://rcs-vostro/ProductsServicePub/ProductsService.svc/ProductsServiceB";

                if (productsServiceClient == null)
                {

                    // Note that currently wsHttpBinding is not supported, but should be as it is part of System.ServiceModel 4.0.0.0.
                    var binding = new BasicHttpBinding() { OpenTimeout = Timeout, SendTimeout = Timeout, ReceiveTimeout = Timeout, CloseTimeout = Timeout };

                    productsServiceClient = new ProductsServiceClient(binding, new EndpointAddress(endpointAddress));
                }

                return productsServiceClient;
            }
        }
        #endregion

        #region IDisposable

        // Check out the IDisposable documentation for details on the pattern applied here.
        // Note that it can have implications on derived classes too.

        // Has Dispose already been called?
        private bool disposed = false;

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposed)
                return;

            // Free managed objects here.
            if (disposing)
            {
                productsServiceClient?.CloseAsync();
            }

            // Free unmanaged objects here.
            { }

            disposed = true;
        }

        ~ProductsServiceConsumer()
        {
            Dispose(false);
        }

        #endregion
    }
}
