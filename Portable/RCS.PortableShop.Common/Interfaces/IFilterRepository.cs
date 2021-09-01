using RCS.AdventureWorks.Common.DomainClasses;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RCS.PortableShop.Common.Interfaces
{
    public interface IFilterRepository<TCollection, TElement> :
        IRepository<TCollection, TElement>
        where TCollection : List<TElement>, new()
    {
        Task Refresh(ProductCategory category, ProductSubcategory subcategory, string namePart);
        Task<Product> Details(int productId);
    }
}
