using System.Collections.ObjectModel;

namespace RCS.WpfShop.Modules.Products.Model
{
    public abstract class Repository<T> : ProductsServiceConsumer
    {
        public Collection<T> List = new Collection<T>();

        public void Clear()
        {
            List.Clear();
        }
    }
}
