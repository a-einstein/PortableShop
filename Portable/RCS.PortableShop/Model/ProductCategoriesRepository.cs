using Microsoft.Extensions.DependencyInjection;
using RCS.AdventureWorks.Common.DomainClasses;
using RCS.AdventureWorks.Common.Dtos;
using RCS.PortableShop.Main;
using RCS.PortableShop.ServiceClients.Products.Wrappers;
using System;
using System.Collections.ObjectModel;
using System.ServiceModel;
using System.Threading.Tasks;

namespace RCS.PortableShop.Model
{
    public class ProductCategoriesRepository : Repository<ObservableCollection<ProductCategory>, ProductCategory>
    {
        #region Construction
        public ProductCategoriesRepository(IProductService productService)
          : base(productService)
        {
            instance = this;
        }

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
                            instance = Startup.ServiceProvider.GetRequiredService<ProductCategoriesRepository>();
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
                categories = await ServiceClient.GetCategories().ConfigureAwait(true);
            }
            catch (FaultException<ExceptionDetail> exception)
            {
                SendMessage(exception);
                return false;
            }
            catch (Exception exception)
            {
                SendMessage(exception);
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
