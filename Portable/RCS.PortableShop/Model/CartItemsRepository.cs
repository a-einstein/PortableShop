using RCS.AdventureWorks.Common.DomainClasses;
using System;
using System.Collections.ObjectModel;
using System.Linq;

namespace RCS.WpfShop.Modules.Products.Model
{
    public class CartItemsRepository : Repository<CartItem>
    {
        private CartItemsRepository()
        {
            List = new ObservableCollection<CartItem>();
        }

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

        private const string cartItemsNumberExceptionMessage = "Unexpected number of found ShoppingCartItems.";

        // Note that the cart is only kept in memory and is not preserved. 
        // It is anticipated that only real orders would be preserved and stored on the server.
        public CartItem AddProduct(IShoppingProduct product)
        {
            var existingCartItems = List.Where(cartItem => cartItem.ProductID == product.Id);
            var existingCartItemsCount = existingCartItems.Count();

            CartItem productCartItem;

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
                throw new Exception(cartItemsNumberExceptionMessage);
            }

            return productCartItem;
        }

        public void DeleteProduct(CartItem cartItem)
        {
            List.Remove(cartItem);
        }

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
    }
}
