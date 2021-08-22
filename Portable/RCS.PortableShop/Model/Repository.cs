using RCS.PortableShop.Interfaces;
using RCS.PortableShop.ServiceClients.Products.Wrappers;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace RCS.PortableShop.Model
{
    // TODO Simplify this definition, remove TCollection.
    public abstract class Repository<TCollection, TElement> :
        ProductsServiceConsumer,
        IRepository<TCollection, TElement>
        where TCollection : List<TElement>, new()
    {
        #region Construction

        protected Repository(IProductService productService)
            : base(productService)
        { }
        #endregion

        #region Refresh
        protected readonly TCollection items = new TCollection();

        // Note this is directly accesible but not amendable.
        public ReadOnlyCollection<TElement> Items
        {
            get { return items.AsReadOnly(); }
        }

        protected async Task Clear()
        {
            await Task.Run(() =>
            {
                // Use ToArray to prevent iteration problems in the original list.
                foreach (var item in items.ToArray())
                {
                    // Remove separately to enable Items_CollectionChanged.
                    items.Remove(item);
                }
            }).ConfigureAwait(true);
        }

        public async Task Refresh(bool addEmptyElement = true)
        {
            try
            {
                await Clear().ConfigureAwait(true);
                await Read(addEmptyElement).ConfigureAwait(true);
            }
            catch (Exception exception)
            {
                SendMessage(exception);
            }
        }
        #endregion

        #region CRUD

        public virtual async Task Create(TElement element)
        {
            await Task.Run(() =>
            {
                items.Add(element);
            });
        }

        protected virtual async Task Read(bool addEmptyElement = true)
        {
            await VoidTask();
        }

        public virtual async Task Update(TElement element)
        {
            await VoidTask();
        }

        public virtual async Task Delete(TElement element)
        {
            await Task.Run(() =>
            {
                items.Remove(element);
            });
        }
        #endregion

        #region Utility
        private static Task VoidTask()
        {
            // HACK.
            return Task.Run(() => { });
        }
        #endregion
    }
}