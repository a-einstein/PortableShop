﻿using RCS.AdventureWorks.Common.DomainClasses;
using RCS.AdventureWorks.Common.Dtos;
using RCS.PortableShop.ServiceClients.Products.Wrappers;
using System;
using System.Collections.Generic;
using System.ServiceModel;
using System.Threading.Tasks;

namespace RCS.PortableShop.Model
{
    public class ProductSubcategoriesRepository : Repository<List<ProductSubcategory>, ProductSubcategory>
    {
        #region Construction
        public ProductSubcategoriesRepository(IProductService productService)
            : base(productService)
        { }
        #endregion

        #region CRUD
        protected override async Task Read(bool addEmptyElement = true)
        {
            ProductSubcategoryList subcategories;

            try
            {
                subcategories = await ServiceClient.GetSubcategories().ConfigureAwait(true);
            }
            catch (FaultException<ExceptionDetail> exception)
            {
                SendMessage(exception);
                return;
            }
            catch (Exception exception)
            {
                SendMessage(exception);
                return;
            }

            if (addEmptyElement)
            {
                // Name is specifically needed on Android to avoid a NullPointerException.
                var subcategory = new ProductSubcategory() { Name = string.Empty };
                items.Add(subcategory);
            }

            foreach (var subcategory in subcategories)
            {
                items.Add(subcategory);
            }
        }
        #endregion
    }
}
