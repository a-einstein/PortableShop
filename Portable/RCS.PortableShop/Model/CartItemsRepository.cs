using RCS.AdventureWorks.Common.DomainClasses;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using Xamarin.Forms;

namespace RCS.PortableShop.Model
{
    public class CartItemsRepository : Repository<ObservableCollection<CartItem>, CartItem>
    {
        #region Construction
        private CartItemsRepository()
        { }

        private static volatile CartItemsRepository instance;
        private static object syncRoot = new Object();

        public static CartItemsRepository Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (syncRoot)
                    {
                        if (instance == null)
                            instance = new CartItemsRepository();
                    }
                }

                return instance;
            }
        }
        #endregion

        #region Constants
        public new enum Errors
        {
            CartError
        }
        #endregion

        #region CRUD

        // Note that the cart is only kept in memory and is not preserved. 
        // It is anticipated that only real orders would be preserved and stored on the server.
        public CartItem AddProduct(IShoppingProduct product)
        {
            var existingCartItems = List.Where(cartItem => cartItem.ProductID == product.Id);
            var existingCartItemsCount = existingCartItems.Count();

            CartItem productCartItem = null;

            if (existingCartItemsCount == 0)
            {
                productCartItem = new CartItem()
                {
                    ProductID = product.Id.Value,
                    Name = product.Name,
                    ProductSize = product.Size,
                    ProductSizeUnitMeasureCode = product.SizeUnitMeasureCode,
                    ProductColor = product.Color,
                    ProductListPrice = product.ListPrice,
                    Quantity = 1,
                };

                List.Add(productCartItem);
            }
            else if (existingCartItemsCount == 1)
            {
                productCartItem = existingCartItems.First();

                productCartItem.Quantity += 1;
                productCartItem.Value = productCartItem.ProductListPrice * productCartItem.Quantity;
            }
            else
            {
                MessagingCenter.Send<CartItemsRepository>(this, CartItemsRepository.Errors.CartError.ToString());
            }

            return productCartItem;
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

        public Decimal CartValue()
        {
            return List.Count > 0
                ? List.Sum(cartItem => cartItem.Value)
                : 0;
        }
        #endregion
    }
}
