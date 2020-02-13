using Newtonsoft.Json;
using RCS.PortableShop.ServiceClients.Products.ProductsService;
using System;
using System.Net.Http;
using System.ServiceModel;
using System.Threading.Tasks;
using Xamarin.Forms;
using static RCS.PortableShop.Model.Settings;

namespace RCS.PortableShop.Model
{
    public abstract class ProductsServiceConsumer : IDisposable
    {
        #region Construction        
        private readonly HttpClient httpClient;

        protected ProductsServiceConsumer()
        {
            // Simple optimisation.
            // TODO Improve as described here, as far as applicable https://josefottosson.se/you-are-probably-still-using-httpclient-wrong-and-it-is-destabilizing-your-software/
            httpClient = new HttpClient();
        }
        #endregion

        #region Constants
        static public TimeSpan Timeout { get; } = new TimeSpan(0, 0, 15);
        static private string serviceDomain = "https://rcsworks.nl";
        static private string productsApi = $"{serviceDomain}/ProductsApi";

        // TODO Separate these 2 types of service clients.
        protected ServiceType serviceType = ServiceType.WebApi;
        #endregion

        #region Messaging
        public enum Message
        {
            ServiceError
        }

        protected void SendMessage(FaultException<ExceptionDetail> exception)
        {
            var detail = exception?.Detail?.InnerException?.Message;

            if (detail.Length > 10)
                // Trim trailing details like user name.
                detail = $"{detail.Remove(11)}...";

            SendMessage(exception, detail);
        }

        protected void SendMessage(Exception exception)
        {
            var detail = exception?.InnerException?.Message;

            SendMessage(exception, detail);
        }

        private void SendMessage(Exception exception, string detail)
        {
            var message = $"{exception?.Message}{Environment.NewLine}{detail}";

            MessagingCenter.Send<ProductsServiceConsumer, string>(this, Message.ServiceError.ToString(), message);
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
        protected abstract string EntitiesName { get; }

        protected async Task<TResult> ReadApi<TResult>()
        {
            var uri = new Uri($"{productsApi}/{EntitiesName}");

            return await ReadApi<TResult>(uri).ConfigureAwait(true);
        }

        protected async Task<TResult> ReadApi<TResult>(string action, string parameters)
        {
            var uri = new Uri($"{productsApi}/{EntitiesName}/{action}?{parameters}");

            return await ReadApi<TResult>(uri).ConfigureAwait(true);
        }

        private async Task<TResult> ReadApi<TResult>(Uri uri)
        {
            var response = await httpClient.GetAsync(uri).ConfigureAwait(true);

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync().ConfigureAwait(true);
                return JsonConvert.DeserializeObject<TResult>(content);
            }
            else
            {
                return default(TResult);
            }
        }
        #endregion

        #region IDisposable
        // Check out the IDisposable documentation for details on the pattern applied here.
        // Note that it can have implications on derived classes too.

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

            if (disposing)
            {
                productsServiceClient?.Dispose();
                httpClient?.Dispose();
            }

            disposed = true;
        }

        ~ProductsServiceConsumer()
        {
            Dispose(false);
        }
        #endregion
    }
}
