using RCS.PortableShop.Common.Interfaces;
using RCS.PortableShop.ServiceClients.Products.Wrappers;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading.Tasks;

namespace RCS.PortableShop.Model
{
    [DebuggerDisplay("Count = {Items.Count}")]
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
        public ReadOnlyCollection<TElement> Items => items.AsReadOnly();

        // Async for future use, though currently not .
        public async Task Clear()
        {
            await Task.Run(() =>
            {
                items.Clear();
            }).ConfigureAwait(true);
        }

        public async Task<bool> Refresh(bool addEmptyElement = true)
        {
            try
            {
                await Clear().ConfigureAwait(true);
                await Read(addEmptyElement).ConfigureAwait(true);

                return true;
            }
            catch (Exception exception)
            {
                SendMessage(exception);
                return false;
            }
        }
        #endregion

        #region CRUD
        public async Task Create(TElement element)
        {
            await Task.Run(() =>
            {
                items.Add(element);
            });
        }

        protected virtual async Task<bool> Read(bool addEmptyElement = true)
        {
            await VoidTask();
            return true;
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