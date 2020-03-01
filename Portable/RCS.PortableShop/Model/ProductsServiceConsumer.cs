using Microsoft.Extensions.DependencyInjection;
using RCS.PortableShop.Main;
using RCS.PortableShop.ServiceClients.Products.Wrappers;
using System;
using System.ServiceModel;
using Xamarin.Forms;
using static RCS.PortableShop.Model.Settings;

namespace RCS.PortableShop.Model
{
    // TODO Move this to Repository.
    public abstract class ProductsServiceConsumer 
    {
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


        #region ServiceClient
        // TODO Find some way to truly inject IProductService and switch on ServiceType.

        protected static IProductService ServiceClient
        {
            get 
            {
                IProductService serviceClient = null;

                switch (ServiceTypeSelected)
                {
                    case ServiceType.WCF:
                        serviceClient = Startup.ServiceProvider.GetRequiredService<ServiceClients.Products.Wrappers.WcfClient>();
                        break;
                    case ServiceType.WebApi:
                        serviceClient= Startup.ServiceProvider.GetRequiredService<ServiceClients.Products.Wrappers.WebApiClient>();
                        break;
                }

                return serviceClient;
            }
        }
        #endregion
    }
}
