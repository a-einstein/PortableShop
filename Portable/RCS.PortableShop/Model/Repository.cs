using RCS.PortableShop.ServiceClients.Products.Wrappers;
using System.Collections.ObjectModel;

namespace RCS.PortableShop.Model
{
    public abstract class Repository<TCollection, TElement> : ProductsServiceConsumer
        where TCollection : Collection<TElement>, new()
    {
        #region Construction
        public Repository(IProductService productService)
            : base(productService)
        { }
        #endregion

        #region CRUD
        public TCollection List { get; } = new TCollection();

        public void Clear()
        {
            List.Clear();
        }
        #endregion
    }
}