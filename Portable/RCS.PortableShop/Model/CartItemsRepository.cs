using RCS.AdventureWorks.Common.DomainClasses;
using RCS.AdventureWorks.Common.Interfaces;
using RCS.PortableShop.ServiceClients.Products.Wrappers;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace RCS.PortableShop.Model
{
    public class CartItemsRepository : Repository<List<CartItem>, CartItem>
    {
        #region Construction
        public CartItemsRepository(IProductService productService)
            : base(productService)
        { }
        #endregion

        #region Constants
        public new enum Message
        {
            CartError
        }
        #endregion

        #region CRUD
        // Note that the cart is only kept in memory and is not preserved. 
        // It is anticipated that only real orders would be preserved and stored on the server.

        public override async Task Create(CartItem proxy)
        {
            await Task.Run(() =>
            {
                items.Add(proxy);
            });
        }

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
                MessagingCenter.Send(this, Message.CartError.ToString());
        }

        public override async Task Delete(CartItem proxy)
        {
            await Task.Run(() =>
            {
                var current = items.FirstOrDefault(item => item.ProductId == proxy.ProductId);

                if (current != default)
                {
                    items.Remove(current);
                }
                else
                {
                    MessagingCenter.Send(this, Message.CartError.ToString());
                }
            });
        }
    }
    #endregion
}