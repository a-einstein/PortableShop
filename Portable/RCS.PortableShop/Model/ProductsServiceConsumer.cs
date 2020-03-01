using Microsoft.Extensions.DependencyInjection;
using RCS.PortableShop.Main;
using RCS.PortableShop.ServiceClients.Products.Wrappers;
using System;
using System.ServiceModel;
using Xamarin.Forms;

namespace RCS.PortableShop.Model
{
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

        // TODO Find some way to elegantly change injection on ServiceType, eliminating the switches in the derived classes.

        #region WCF Service
        protected static IProductService WcfClient
        {
            // HACK Temporary.
            get => Startup.ServiceProvider.GetRequiredService<ServiceClients.Products.Wrappers.WcfClient>();
        }
        #endregion

        #region Web API
        protected static IProductService WebApiClient
        {
            // HACK Temporary.
            get => Startup.ServiceProvider.GetRequiredService<WebApiClient>();
        }
        #endregion
    }
}
