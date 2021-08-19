using RCS.AdventureWorks.Common.DomainClasses;
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
        // Note IProductService is currently not used.
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
                items.Add(proxy.Copy());
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
            var current = items.FirstOrDefault(item => item.ProductId == proxy.ProductId);

            if (current != default)
            {
                // Just replace the item instead of updating it internally.
                await Delete(current).ConfigureAwait(true);
                await Create(proxy).ConfigureAwait(true);
            }
            else
            {
                MessagingCenter.Send<CartItemsRepository>(this, Message.CartError.ToString());
            }
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
                    MessagingCenter.Send<CartItemsRepository>(this, Message.CartError.ToString());
                }
            });
        }
    }
    #endregion
}