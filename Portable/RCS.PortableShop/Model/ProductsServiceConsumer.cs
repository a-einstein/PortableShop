using CommunityToolkit.Mvvm.Messaging;
using RCS.PortableShop.ServiceClients.Products.Wrappers;
using System.ServiceModel;

namespace RCS.PortableShop.Model
{
    public abstract class ProductsServiceConsumer
    {
        protected ProductsServiceConsumer(IProductService productService)
        {
            ServiceClient = productService;
        }

        #region Messaging
        private enum MessageType
        {
            ServiceError
        }

        public class ServiceMessage
        {
            public ServiceMessage(string messageType)
            {
                MessageType = messageType;
            }

            public ServiceMessage(string messageType, string details)
                : this(messageType)
            {
                Details = details;
            }

            public string MessageType { get; private set; }
            public string Details { get; private set; }
        }

        protected void SendMessage(FaultException<ExceptionDetail> exception)
        {
            var innerMessage = exception?.Detail?.InnerException?.Message;

            if (innerMessage?.Length > 10)
                // Trim trailing details like user name.
                innerMessage = $"{innerMessage.Remove(11)}...";

            SendMessage(exception, innerMessage);
        }

        protected void SendMessage(Exception exception)
        {
            var innerMessage = exception?.InnerException?.Message;

            SendMessage(exception, innerMessage);
        }

        private void SendMessage(Exception exception, string innerMessage)
        {
            var details = $"{exception?.Message}{Environment.NewLine}{innerMessage}";

            WeakReferenceMessenger.Default.Send(new ServiceMessage(MessageType.ServiceError.ToString(), details));
        }
        #endregion

        #region ServiceClient
        protected IProductService ServiceClient { get; }
        #endregion
    }
}
