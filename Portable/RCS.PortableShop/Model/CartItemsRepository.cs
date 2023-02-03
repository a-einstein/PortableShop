using CommunityToolkit.Mvvm.Messaging;
using RCS.AdventureWorks.Common.DomainClasses;
using RCS.AdventureWorks.Common.Interfaces;
using RCS.PortableShop.ServiceClients.Products.Wrappers;

namespace RCS.PortableShop.Model
{
    public class CartItemsRepository : Repository<List<CartItem>, CartItem>
    {
        #region Construction
        public CartItemsRepository(IProductService productService)
            : base(productService)
        { }
        #endregion

        #region Messaging
        private enum MessageType
        {
            CartError
        }

        public class CartMessage : ServiceMessage
        {
            public CartMessage(string messageType) 
                : base(messageType)
            { }
        }
        #endregion

        #region CRUD
        // Note that the cart is only kept in memory and is not preserved. 
        // It is anticipated that only real orders would be preserved and stored on the server.

        public async Task Create(IShoppingProduct product)
        {
            await Task.Run(() =>
            {
                items.Add(new CartItem(product));
            });
        }

        public override async Task Update(CartItem proxy)
        {
            // Use a simple function instead of CancellationToken .
            var task = Task.Run(() =>
            {
                // TODO This could be generalized by applying something like IEqualityComparer on ProductId or even Id.
                var foundItem = items.FirstOrDefault(item => item.ProductId == proxy.ProductId);

                if (foundItem != default)
                {
                    foundItem.Update(proxy);
                    return true;
                }
                else
                {
                    return false;
                }
            });

            await task;

            if (!task.Result)
                WeakReferenceMessenger.Default.Send(new CartMessage(MessageType.CartError.ToString()));
        }

        public override async Task Delete(CartItem proxy)
        {
            await Task.Run(() =>
            {
                // TODO This could be generalized by applying something like IEqualityComparer on ProductId or even Id.
                var current = items.FirstOrDefault(item => item.ProductId == proxy.ProductId);

                if (current != default)
                {
                    items.Remove(current);
                }
                else
                {
                    WeakReferenceMessenger.Default.Send(new CartMessage(MessageType.CartError.ToString()));
                }
            });
        }
    }
    #endregion
}