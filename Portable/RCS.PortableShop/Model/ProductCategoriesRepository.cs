using Newtonsoft.Json;
using RCS.AdventureWorks.Common.DomainClasses;
using RCS.PortableShop.ServiceClients.Products.ProductsService;
using System;
using System.Collections.ObjectModel;
using System.Net.Http;
using System.ServiceModel;
using System.Threading.Tasks;

namespace RCS.PortableShop.Model
{
    public class ProductCategoriesRepository : Repository<ObservableCollection<ProductCategory>, ProductCategory>
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
                // TODO Create some sort of injection somewhere?
                switch (preferredServiceType)
                {
                    case ServiceType.WCF:
                        categories = await Task.Factory.FromAsync<ProductCategoryList>(
                            ProductsServiceClient.BeginGetProductCategories,
                            ProductsServiceClient.EndGetProductCategories,
                            null);
                        break;
                    case ServiceType.WebApi:
                        // TODO Should be instantiated once. (Or Disposed of?)
                        var httpClient = new HttpClient();
                        var uri = new Uri($"{productsApi}/ProductCategories");

                        var response = await httpClient.GetAsync(uri);

                        if (response.IsSuccessStatusCode)
                        {
                            var content = await response.Content.ReadAsStringAsync();
                            categories = JsonConvert.DeserializeObject<ProductCategoryList>(content);
                        }
                        break;
                    default:
                        break;
                }
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
