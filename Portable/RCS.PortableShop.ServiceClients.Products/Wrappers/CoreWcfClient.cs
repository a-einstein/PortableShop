using Microsoft.Extensions.Options;
using ProductsCoreWcf;
using RCS.AdventureWorks.Common.DomainClasses;
using RCS.AdventureWorks.Common.Dtos;
using System.Threading.Tasks;
using static ProductsCoreWcf.ProductsServiceClient;

namespace RCS.PortableShop.ServiceClients.Products.Wrappers
{
    public class CoreWcfClient : ServiceClient, IProductService
    {
        #region Construction
        public CoreWcfClient(IOptions<ServiceOptions> serviceOptions)
            : base(serviceOptions)
        {
            ProductsServiceClient = new ProductsServiceClient(EndpointConfiguration.BasicHttpBinding_IProductsService, ServiceOptions.RemoteAddress);
        }
        #endregion

        #region Interface
        public Task<ProductCategoryList> GetCategories()
        {
            return ProductsServiceClient.GetProductCategoriesAsync();
        }

        public Task<ProductSubcategoryList> GetSubcategories()
        {
            return ProductsServiceClient.GetProductSubcategoriesAsync();
        }

        public Task<ProductsOverviewList> GetProducts(ProductCategory category, ProductSubcategory subcategory, string namePart)
        {
            return ProductsServiceClient.GetProductsOverviewByAsync(category.Id, subcategory.Id, namePart);
        }

        public Task<Product> GetProduct(int productId)
        {
            return ProductsServiceClient.GetProductDetailsAsync(productId);
        }
        #endregion

        #region Utilities
        // TODO Check if this needs to become more sophisticated.
        private ProductsServiceClient ProductsServiceClient { get; set; }
        #endregion
    }
}
