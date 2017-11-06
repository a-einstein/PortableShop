using RCS.AdventureWorks.Common.DomainClasses;
using RCS.PortableShop.ServiceClients.Products.ProductsService;
using System;
using System.ServiceModel;
using System.Threading.Tasks;

namespace RCS.PortableShop.Model
{
    public class ProductCategoriesRepository : Repository<ProductCategory>
    {
        #region Construction
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
        #endregion

        #region CRUD
        public async Task<bool> ReadList(bool addEmptyElement = true)
        {
            Clear();

            var categories = new ProductCategoryList();

            try
            {
                categories = await Task.Factory.FromAsync<ProductCategoryList>(
                    ProductsServiceClient.BeginGetProductCategories,
                    ProductsServiceClient.EndGetProductCategories,
                    null);
            }
            catch (FaultException<ExceptionDetail> exception)
            {
                Message(exception);
                return false;
            }
            catch (Exception exception)
            {
                Message(exception);
                return false;
            }

            if (addEmptyElement)
            {
                // Name is specifically needed on Android to avoid a NullPointerException.
                var category = new ProductCategory() { Name = string.Empty };
                List.Add(category);
            }

            foreach (var category in categories)
            {
                List.Add(category);
            }

            return true;
        }
        #endregion
    }
}
