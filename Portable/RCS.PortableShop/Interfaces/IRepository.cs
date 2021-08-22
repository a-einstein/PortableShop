using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace RCS.PortableShop.Interfaces
{
    public interface IRepository<TCollection, TElement>
        where TCollection : List<TElement>, new()
    {
        ReadOnlyCollection<TElement> Items { get; }

        // CRUD.
        Task Create(TElement element);
        Task Refresh(bool addEmptyElement = true);
        Task Update(TElement element);
        Task Delete(TElement element);
    }
}