using Newtonsoft.Json;
using RCS.PortableShop.ServiceClients.Products.ProductsService;
using System;
using System.Net.Http;
using System.ServiceModel;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace RCS.PortableShop.Model
{
    public abstract class ProductsServiceConsumer : IDisposable
    {
        #region Constants
        static public TimeSpan Timeout { get; } = new TimeSpan(0, 0, 15);
        static private string serviceDomain = "https://rcsworks.nl";
        static private string productsApi = $"{serviceDomain}/ProductsApi/api";

        // TODO Move elsewhere if both kept .
        public enum ServiceType
        {
            WCF,
            WebApi
        }

        // TODO Move to settings.
        protected ServiceType preferredServiceType = ServiceType.WebApi;

        #endregion

        #region Messaging
        public enum Errors
        {
            ServiceError
        }

        protected void Message(FaultException<ExceptionDetail> exception)
        {
            var detail = exception?.Detail?.InnerException?.Message;

            if (detail.Length > 10)
                // Trim trailing details like user name.
                detail = $"{detail.Remove(11)}...";

            Message(exception, detail);
        }

        protected void Message(Exception exception)
        {
            var detail = exception?.InnerException?.Message;

            Message(exception, detail);
        }

        private void Message(Exception exception, string detail)
        {
            var message = $"{exception?.Message}{Environment.NewLine}{detail}";

            MessagingCenter.Send<ProductsServiceConsumer, string>(this, Errors.ServiceError.ToString(), message);
        }

        #endregion

        #region WCF Service
        private ProductsServiceClient productsServiceClient;

        protected IProductsService ProductsServiceClient
        {
            get
            {
                try
                {
                    // TODO >> Does not work as the intended singleton. Is that useful & necessary? Think to have seen not.
                    if (productsServiceClient == null)
                    {
                        // TODO Make this better configurable. There does not seem to be a config file like on WPF.
                        // TODO If possible get transformation on configs. 

                        // Note that currently wsHttpBinding is not supported, but should be as it is part of System.ServiceModel 4.0.0.0.
                        var binding = new BasicHttpBinding(BasicHttpSecurityMode.Transport) { OpenTimeout = Timeout, SendTimeout = Timeout, ReceiveTimeout = Timeout, CloseTimeout = Timeout };

                        // Note this points to a BasicHttpBinding variant on the server.
                        string endpointAddress = $"{serviceDomain}/ProductsServicePub/ProductsService.svc/ProductsServiceB";

                        // Note the example bindings in ProductsServiceClient which could also be applied here by using EndpointConfiguration
                        productsServiceClient = new ProductsServiceClient(binding, new EndpointAddress(endpointAddress));
                    }

                }
                catch (Exception)
                {
                    throw;
                }

                return productsServiceClient;
            }
        }
        #endregion

        #region Web API

        // Note this needs to be a plural.
        protected  abstract string EntitiesName { get; }

        protected async Task<TResult> ReadApi<TResult>(TResult result)
        {
            var uri = new Uri($"{productsApi}/{EntitiesName}");

            return await ReadApi<TResult>(result, uri);
        }

        protected async Task<TResult> ReadApi<TResult>(TResult result, int id)
        {
            var uri = new Uri($"{productsApi}/{EntitiesName}/{id}");

            return await ReadApi<TResult>(result, uri);
        }

        private static async Task<TResult> ReadApi<TResult>(TResult result, Uri uri)
        {  
            // TODO Should be instantiated once. (Or Disposed of?)
            var httpClient = new HttpClient();

            var response = await httpClient.GetAsync(uri);

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                result = JsonConvert.DeserializeObject<TResult>(content);
            }

            return result;
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
