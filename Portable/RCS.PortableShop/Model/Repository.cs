using System.Collections.ObjectModel;

namespace RCS.PortableShop.Model
{
    public abstract class Repository<TCollection, TElement> : ProductsServiceConsumer
        where TCollection : Collection<TElement>, new()
    {
        // Note Derived singletons duplicate construction code, but it it does not seem feasible to share that here.

        #region CRUD
        public TCollection List { get; } = new TCollection();

        public void Clear()
        {
            List.Clear();
        }
        #endregion
    }
}