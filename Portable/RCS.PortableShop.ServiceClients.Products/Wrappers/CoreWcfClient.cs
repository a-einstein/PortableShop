using Microsoft.Extensions.Options;
using ProductsCoreWcf;
using RCS.AdventureWorks.Common.DomainClasses;
using RCS.AdventureWorks.Common.Dtos;
using System.ServiceModel;
using System.Threading.Tasks;

namespace RCS.PortableShop.ServiceClients.Products.Wrappers
{
    public class CoreWcfClient : WcfClientBase, IProductService
    {
        #region Construction
        public CoreWcfClient(IOptions<ServiceOptions> serviceOptions)
            : base(serviceOptions)
        {
            ProductsServiceClient = new ProductsServiceClient(Binding(), new EndpointAddress(ServiceOptions.RemoteAddress));
        }
        #endregion

        #region IProductService
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

        #region ProductsServiceClient
        private ProductsServiceClient ProductsServiceClient { get; set; }
        #endregion
    }
}
