using System.ServiceModel;

namespace RCS.PortableShop.ServiceClients.Products.ProductsService
{
    // Added this part to implement IDisposable.
    // Oddly, it wasn't implemented, while it could still build.
    // Based on https://coding.abel.nu/2012/02/using-and-disposing-of-wcf-clients.
    public partial class ProductsServiceClient
    {
        #region IDisposable
        public void Dispose()
        {
            Dispose(true);
        }

        private void Dispose(bool disposing)
        {
            if (disposing)
            {
                try
                {
                    if (State != CommunicationState.Faulted)
                        Close();
                }
                finally
                {
                    if (State != CommunicationState.Closed)
                        Abort();
                }
            }
        }

         ~ProductsServiceClient()
        {
            Dispose(false);
        }
        #endregion
    }
}
