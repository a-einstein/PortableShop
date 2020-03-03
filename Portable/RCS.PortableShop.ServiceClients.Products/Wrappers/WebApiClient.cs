using Newtonsoft.Json;
using RCS.AdventureWorks.Common.DomainClasses;
using RCS.AdventureWorks.Common.Dtos;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace RCS.PortableShop.ServiceClients.Products.Wrappers
{
    public class WebApiClient : ServiceClient, IProductService
    {
        #region Construction        
        private readonly IHttpClientFactory httpClientFactory;

        public WebApiClient(IHttpClientFactory httpClientFactory)
        {
            this.httpClientFactory = httpClientFactory;
        }
        #endregion

        #region Interface
        public async Task<ProductCategoryList> GetCategories()
        {
            const string entityName = "ProductCategories";

            var result = await ReadApi<ProductCategoryList>(entityName).ConfigureAwait(true);

            return result;
        }

        public async Task<ProductSubcategoryList> GetSubcategories()
        {
            const string entityName = "ProductSubcategories";

            var result = await ReadApi<ProductSubcategoryList>(entityName).ConfigureAwait(true);

            return result;
        }

        private const string ProductEntityName = "Products";

        public async Task<ProductsOverviewList> GetProducts(ProductCategory category, ProductSubcategory subcategory, string namePart)
        {
            // Note the parameternames have to mach those of the web API. 
            var categoryParameter = category != null ? $"category={category.Id}" : null;
            var subcategoryParameter = subcategory != null ? $"subcategory={subcategory.Id}" : null;
            var wordParameter = namePart != null ? $"word={namePart}" : null;

            // Note that extra occurrences of # are acceptable and order does not matter.
            var parameters = $"{categoryParameter}&{subcategoryParameter}&{wordParameter}";

            var result = await ReadApi<ProductsOverviewList>(ProductEntityName, "overview", parameters).ConfigureAwait(true);

            return result;
        }

        public async Task<Product> GetProduct(int productId)
        {
            var parameters = $"id={productId}";

            var result = await ReadApi<Product>(ProductEntityName, "details", parameters).ConfigureAwait(true);

            return result;
        }
        #endregion

        #region Utilities
        private static string productsApi = $"{serviceDomain}/ProductsApi";

        private HttpClient HttpClient => httpClientFactory.CreateClient();

        private async Task<TResult> ReadApi<TResult>(string entityName)
        {
            var uri = new Uri($"{productsApi}/{entityName}");

            return await ReadApi<TResult>(uri).ConfigureAwait(true);
        }

        private async Task<TResult> ReadApi<TResult>(string entityName, string action, string parameters)
        {
            // Note that entityName has to be a plural.
            var uri = new Uri($"{productsApi}/{entityName}/{action}?{parameters}");

            return await ReadApi<TResult>(uri).ConfigureAwait(true);
        }

        private async Task<TResult> ReadApi<TResult>(Uri uri)
        {
            // TODO Further improve as described here, as far as applicable 
            // https://josefottosson.se/you-are-probably-still-using-httpclient-wrong-and-it-is-destabilizing-your-software/

            var response = await HttpClient.GetAsync(uri).ConfigureAwait(true);

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync().ConfigureAwait(true);
                return JsonConvert.DeserializeObject<TResult>(content);
            }
            else
            {
                return default;
            }
        }
        #endregion
    }
}