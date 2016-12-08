using RCS.AdventureWorks.Common.DomainClasses;
using RCS.WpfShop.ServiceClients.Products.ProductsService;
using System;
using System.Threading.Tasks;

namespace RCS.WpfShop.Modules.Products.Model
{
    public class ProductSubcategoriesRepository : Repository<ProductSubcategory>
    {
        private ProductSubcategoriesRepository()
        { }

        private static volatile ProductSubcategoriesRepository instance;
        private static object syncRoot = new Object();

        public static ProductSubcategoriesRepository Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (syncRoot)
                    {
                        if (instance == null)
                            instance = new ProductSubcategoriesRepository();
                    }
                }

                return instance;
            }
        }

        public async Task ReadList(bool addEmptyElement = true)
        {
            Clear();

            var task = Task.Run(async () =>
            {
                var subcategories = await ProductsServiceClient.GetProductSubcategoriesAsync();

                if (addEmptyElement)
                {
                    var subcategory = new ProductSubcategory();
                    List.Add(subcategory);
                }

                foreach (var subcategory in subcategories)
                {
                    List.Add(subcategory);
                }
            });

            await task;
        }
    }
}
