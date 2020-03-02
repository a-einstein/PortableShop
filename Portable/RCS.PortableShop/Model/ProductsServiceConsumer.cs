using RCS.PortableShop.ServiceClients.Products.Wrappers;
using System;
using System.ServiceModel;
using Xamarin.Forms;

namespace RCS.PortableShop.Model
{
    public abstract class ProductsServiceConsumer 
    {
        public ProductsServiceConsumer(IProductService productService)
        {
            ServiceClient = productService;
        }

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
        protected IProductService ServiceClient { get; }
        #endregion
    }
}
