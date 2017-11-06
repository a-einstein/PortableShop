using RCS.AdventureWorks.Common.DomainClasses;
using RCS.PortableShop.ServiceClients.Products.ProductsService;
using System;
using System.Collections.Generic;
using System.ServiceModel;
using System.Threading.Tasks;

namespace RCS.PortableShop.Model
{
    public class ProductsRepository : Repository<ProductsOverviewObject>
    {
        #region Construction
        private ProductsRepository()
        { }

        private static volatile ProductsRepository instance;
        private static object syncRoot = new Object();

        public static ProductsRepository Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (syncRoot)
                    {
                        if (instance == null)
                            instance = new ProductsRepository();
                    }
                }

                return instance;
            }
        }
        #endregion

        #region CRUD

        // TODO This should get paged with an optional pagesize.
        public async Task<IList<ProductsOverviewObject>> ReadList(ProductCategory category, ProductSubcategory subcategory, string namePart)
        {
            var productsOverview = new ProductsOverviewList();

            try
            {
                productsOverview = await Task.Factory.FromAsync<int?, int?, string, ProductsOverviewList>(
                     ProductsServiceClient.BeginGetProductsOverviewBy,
                     ProductsServiceClient.EndGetProductsOverviewBy,
                     category?.Id, subcategory?.Id, namePart,
                     null);
            }
            catch (FaultException<ExceptionDetail> exception)
            {
                Message(exception);
                return null;
            }
            catch (Exception exception)
            {
                Message(exception);
                return null;
            }

            return productsOverview;
        }

        public async Task<Product> ReadDetails(int productID)
        {
            Product product = null;

            try
            {
                product = await Task.Factory.FromAsync<int, Product>(
                  ProductsServiceClient.BeginGetProductDetails,
                  ProductsServiceClient.EndGetProductDetails,
                  productID,
                  null);
            }
            catch (FaultException<ExceptionDetail> exception)
            {
                Message(exception);
            }
            catch (Exception exception)
            {
                Message(exception);
            }

            return product;
        }
        #endregion
    }
}