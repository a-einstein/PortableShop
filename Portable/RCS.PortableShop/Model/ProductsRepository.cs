using RCS.AdventureWorks.Common.DomainClasses;
using RCS.WpfShop.ServiceClients.Products.ProductsService;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RCS.WpfShop.Modules.Products.Model
{
    public class ProductsRepository : Repository<ProductsOverviewObject>
    {
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

        // TODO This should get paged with an optional pagesize.
        public async Task<IList<ProductsOverviewObject>> ReadList(ProductCategory category, ProductSubcategory subcategory, string namePart)
        {
            var task = Task.Run(async () =>
            {
                var productOverview = await ProductsServiceClient.GetProductsOverviewByAsync(
                    category?.Id,
                    subcategory?.Id,
                    namePart);

                return productOverview;
            });

            await task;

            return task.Result;
        }

        public async Task<Product> ReadDetails(int productID)
        {
            var task = Task.Factory.StartNew(async () =>
            {
                var product = await ProductsServiceClient.GetProductDetailsAsync(productID);

                return product;
            });

            return await task.Result;
        }
    }
}
