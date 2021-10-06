using RCS.AdventureWorks.Common.DomainClasses;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RCS.PortableShop.Common.Interfaces
{
    public interface IFilterRepository<TCollection, TElement, TCategory, TSubcategory, TId> :
        IRepository<TCollection, TElement>
        where TCollection : List<TElement>, new()
    {
        Task Refresh(TCategory category, TSubcategory subcategory, string searchString);
        Task<Product> Details(TId elementId);
    }
}
