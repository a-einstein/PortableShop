using RCS.AdventureWorks.Common.DomainClasses;
using RCS.PortableShop.ServiceClients.Products.Wrappers;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using Xamarin.Forms;

namespace RCS.PortableShop.Model
{
    public class CartItemsRepository : Repository<ObservableCollection<CartItem>, CartItem>
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
        public void AddProduct(IShoppingProduct product)
        {
            var existingCartItems = List.Where(cartItem => cartItem.ProductID == product.Id).ToList();

            CartItem productCartItem;

            if (existingCartItems.Count == 0)
            {
                productCartItem = new CartItem()
                {
                    ProductID = product.Id.Value,
                    Name = product.Name,
                    ProductSize = product.Size,
                    ProductSizeUnitMeasureCode = product.SizeUnitMeasureCode,
                    ProductColor = product.Color,
                    ProductListPrice = product.ListPrice,
                    Quantity = 1
                };

                List.Add(productCartItem);
            }
            else if (existingCartItems.Count == 1)
            {
                productCartItem = existingCartItems.First();

                productCartItem.Quantity += 1;
                productCartItem.Value = productCartItem.ProductListPrice * productCartItem.Quantity;
            }
            else
            {
                MessagingCenter.Send<CartItemsRepository>(this, Message.CartError.ToString());
            }
        }

        public void DeleteProduct(CartItem cartItem)
        {
            List.Remove(cartItem);
        }
        #endregion

        #region Aggregates
        public int ProductsCount()
        {
            return List.Count > 0
                ? List.Sum(cartItem => cartItem.Quantity)
                : 0;
        }

        public decimal CartValue()
        {
            return List.Count > 0
                ? List.Sum(cartItem => cartItem.Value)
                : 0;
        }
        #endregion
    }
}
