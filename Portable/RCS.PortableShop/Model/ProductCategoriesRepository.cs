using RCS.AdventureWorks.Common.DomainClasses;
using RCS.PortableShop.ServiceClients.Products.ProductsService;
using System;
using System.Threading.Tasks;

namespace RCS.PortableShop.Model
{
    public class ProductCategoriesRepository : Repository<ProductCategory>
    {
        private ProductCategoriesRepository()
        { }

        private static volatile ProductCategoriesRepository instance;
        private static object syncRoot = new Object();

        public static ProductCategoriesRepository Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (syncRoot)
                    {
                        if (instance == null)
                            instance = new ProductCategoriesRepository();
                    }
                }

                return instance;
            }
        }

        public async Task ReadList(bool addEmptyElement = true)
        {
            Clear();

            var categories = await Task.Factory.FromAsync<ProductCategoryList>(
                ProductsServiceClient.BeginGetProductCategories,
                ProductsServiceClient.EndGetProductCategories,
                null);

            if (addEmptyElement)
            {
                var category = new ProductCategory() { Name = string.Empty };
                List.Add(category);
            }

            foreach (var category in categories)
            {
                List.Add(category);
            }
        }
    }
}
