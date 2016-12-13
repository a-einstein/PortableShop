using RCS.PortableShop.ServiceClients.Products.ProductsService;
using System;
using System.ServiceModel;

namespace RCS.PortableShop.Model
{
    public abstract class ProductsServiceConsumer : IDisposable
    {
        private ProductsServiceClient productsServiceClient;

        protected IProductsService ProductsServiceClient
        {
            get
            {
                // TODO Make this better configurable. There normally does not seem to be an .config file.
                const string endpointAddress = "http://rcs-vostro:80/ProductsServicePub/ProductsService.svc/ProductsService/";

                if (productsServiceClient == null)
                    productsServiceClient = new ProductsServiceClient(new BasicHttpBinding(), new EndpointAddress(endpointAddress));

                return productsServiceClient;
            }
        }

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
                //productsServiceClient?.Close();
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
