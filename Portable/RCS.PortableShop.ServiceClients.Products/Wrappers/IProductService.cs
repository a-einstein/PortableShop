using RCS.AdventureWorks.Common.DomainClasses;
using RCS.AdventureWorks.Common.Dtos;
using System.Threading.Tasks;

namespace RCS.PortableShop.ServiceClients.Products.Wrappers
{
    public interface IProductService
    {
        public Task<ProductCategoryList> GetCategories();
        public Task<ProductSubcategoryList> GetSubcategories();

        public Task<ProductsOverviewList> GetProducts(ProductCategory category, ProductSubcategory subcategory, string namePart);
        public Task<Product> GetProduct(int productID);
    }
}
