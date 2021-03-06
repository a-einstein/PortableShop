﻿using RCS.PortableShop.ServiceClients.Products.Wrappers;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace RCS.PortableShop.Model
{
    // TODO Simplify this definition, remove TCollection.
    public abstract class Repository<TCollection, TElement> : ProductsServiceConsumer
        where TCollection : List<TElement>, new()
    {
        #region Construction

        protected Repository(IProductService productService)
            : base(productService)
        { }
        #endregion

        #region Refresh
        protected TCollection items = new TCollection();

        // Note this is directly accesible but not amendable.
        public ReadOnlyCollection<TElement> Items
        {
            get { return items.AsReadOnly(); }
        }

        public async Task Clear()
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
                return;
            }
        }
        #endregion

        #region CRUD
        public virtual async Task Create(TElement element)
        {
            items.Add(element);
        }

        protected virtual async Task Read(bool addEmptyElement = true)
        { }

        public virtual async Task Update(TElement element)
        { }

        public virtual async Task Delete(TElement element)
        {
            items.Remove(element);
        }
        #endregion
    }
}