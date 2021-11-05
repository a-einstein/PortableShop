using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace RCS.PortableShop.Common.Interfaces
{
    public interface IRepository<TCollection, TElement>
        where TCollection : List<TElement>, new()
    {
        ReadOnlyCollection<TElement> Items { get; }

        // CRUD.
        // TODO Make generally Task<bool>.
        Task Create(TElement element);
        Task<bool> Refresh(bool addEmptyElement = true);
        Task Update(TElement element);
        Task Delete(TElement element);
    }
}