using Microsoft.Extensions.Options;
using RCS.AdventureWorks.Common.DomainClasses;
using RCS.AdventureWorks.Common.Dtos;
using RCS.PortableShop.ServiceClients.Products.ProductsService;
using System;
using System.ServiceModel;
using System.Threading.Tasks;

namespace RCS.PortableShop.ServiceClients.Products.Wrappers
{
    public class WcfClient : WcfClientBase, IProductService, IDisposable
    {
        #region Construction
        public WcfClient(IOptions<ServiceOptions> serviceOptions)
            : base(serviceOptions)
        { }
        #endregion

        #region IProductService
        // Be aware that using FromAsync on a shared WcfClient can cause threading problems
        // when the begin and end calls get interwoven, like when started from Task.WhenAll.
        // This finally became clear from:
        // https://social.msdn.microsoft.com/Forums/windows/en-US/a26d9697-25b2-4ee0-82b5-a1fe08d722ec/the-iasyncresult-object-provided-does-not-match-this-delegate

        public async Task<ProductCategoryList> GetCategories()
        {
            var result =
                await Task.Factory.FromAsync(
                    ProductsServiceClient.BeginGetProductCategories,
                    ProductsServiceClient.EndGetProductCategories,
                    null).ConfigureAwait(true) ??
                // Guarantee a list, even with failing service.
                new ProductCategoryList();

            return result;
        }

        public async Task<ProductSubcategoryList> GetSubcategories()
        {
            var result =
                await Task.Factory.FromAsync(
                    ProductsServiceClient.BeginGetProductSubcategories,
                    ProductsServiceClient.EndGetProductSubcategories,
                    null).ConfigureAwait(true) ??
               // Guarantee a list, even with failing service.
               new ProductSubcategoryList();

            return result;
        }

        public async Task<ProductsOverviewList> GetProducts(ProductCategory category, ProductSubcategory subcategory, string namePart)
        {
            var result =
               await Task.Factory.FromAsync(
                    ProductsServiceClient.BeginGetProductsOverviewBy,
                    ProductsServiceClient.EndGetProductsOverviewBy,
                    category?.Id, subcategory?.Id, namePart,
                    null).ConfigureAwait(true) ??
               // Guarantee a list, even with failing service.
               new ProductsOverviewList();

            return result;
        }

        public async Task<Product> GetProduct(int productId)
        {
            var result = await Task.Factory.FromAsync(
                  ProductsServiceClient.BeginGetProductDetails,
                  ProductsServiceClient.EndGetProductDetails,
                  productId,
                  null).ConfigureAwait(true);

            return result;
        }
        #endregion

        #region ProductsServiceClient
        // Note Need both types for various parts.
        private ProductsServiceClient productsServiceClient;

        private IProductsService ProductsServiceClient
        {
            get
            {
                // TODO >> Does not work as the intended singleton. Is that useful & necessary? Think to have seen not.
                if (productsServiceClient == null)
                {
                    productsServiceClient = new ProductsServiceClient(Binding(), new EndpointAddress(ServiceOptions.RemoteAddress));
                }

                return productsServiceClient;
            }
        }
        #endregion

        // TODO Check the necessity and redundancy of the various Disposables.
        #region IDisposable
        // Check out the IDisposable documentation for details on the pattern applied here.
        // Note that it can have implications on derived classes too.

        private bool disposed;

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private void Dispose(bool disposing)
        {
            if (disposed)
                return;

            if (disposing)
            {
                productsServiceClient?.Dispose();
            }

            disposed = true;
        }

        ~WcfClient()
        {
            Dispose(false);
        }
        #endregion
    }
}
