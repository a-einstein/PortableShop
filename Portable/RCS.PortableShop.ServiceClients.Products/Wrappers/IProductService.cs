using RCS.AdventureWorks.Common.DomainClasses;
using RCS.AdventureWorks.Common.Dtos;
using System.Threading.Tasks;

namespace RCS.PortableShop.ServiceClients.Products.Wrappers
{
    public interface IProductService
    {
        Task<ProductCategoryList> GetCategories();
        Task<ProductSubcategoryList> GetSubcategories();

        Task<ProductsOverviewList> GetProducts(ProductCategory category, ProductSubcategory subcategory, string namePart);
        Task<Product> GetProduct(int productId);
    }
}
