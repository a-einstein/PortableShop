using RCS.AdventureWorks.Common.DomainClasses;
using RCS.AdventureWorks.Common.Dtos;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ServiceModel;
using System.Threading.Tasks;

namespace RCS.PortableShop.Model
{
    public class ProductsRepository : Repository<ObservableCollection<ProductsOverviewObject>, ProductsOverviewObject>
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
        protected override string EntitiesName => "Products";

        // TODO This should get paged with an optional pagesize.
        public async Task<IList<ProductsOverviewObject>> ReadList(ProductCategory category, ProductSubcategory subcategory, string namePart)
        {
            var productsOverview = new ProductsOverviewList();

            try
            {
                // TODO Create some sort of injection somewhere?
                switch (preferredServiceType)
                {
                    case ServiceType.WCF:
                        productsOverview = await Task.Factory.FromAsync<int?, int?, string, ProductsOverviewList>(
                            ProductsServiceClient.BeginGetProductsOverviewBy,
                            ProductsServiceClient.EndGetProductsOverviewBy,
                            category?.Id, subcategory?.Id, namePart,
                            null).ConfigureAwait(true);
                        break;
                    case ServiceType.WebApi:
                        // Note the parameternames have to mach those of the web API. 
                        string categoryParameter = category != null ? $"category={category.Id}" : null;
                        string subcategoryParameter = subcategory != null ? $"subcategory={subcategory.Id}" : null;
                        string wordParameter = namePart != null ? $"word={namePart}" : null;

                        // Note that extra occurrences of # are acceptable and order does not matter.
                        var parameters = $"{categoryParameter}&{subcategoryParameter}&{wordParameter}";

                        productsOverview = await ReadApi<ProductsOverviewList>("overview", parameters).ConfigureAwait(true);
                        break;
                    default:
                        throw new NotImplementedException($"Unknown {nameof(ServiceType)}");
                }
            }
            catch (FaultException<ExceptionDetail> exception)
            {
                SendMessage(exception);
                return null;
            }
            catch (Exception exception)
            {
                SendMessage(exception);
                return null;
            }

            return productsOverview;
        }

        public async Task<Product> ReadDetails(int productID)
        {
            Product product = null;

            try
            {
                // TODO Create some sort of injection somewhere?
                switch (preferredServiceType)
                {
                    case ServiceType.WCF:
                        product = await Task.Factory.FromAsync<int, Product>(
                            ProductsServiceClient.BeginGetProductDetails,
                            ProductsServiceClient.EndGetProductDetails,
                            productID,
                            null).ConfigureAwait(true);
                        break;
                    case ServiceType.WebApi:
                        var parameters = $"id={productID}";

                        product = await ReadApi<Product>("details", parameters).ConfigureAwait(true);
                        break;
                    default:
                        throw new NotImplementedException($"Unknown {nameof(ServiceType)}");
                }
            }
            catch (FaultException<ExceptionDetail> exception)
            {
                SendMessage(exception);
            }
            catch (Exception exception)
            {
                SendMessage(exception);
            }

            return product;
        }
        #endregion
    }
}