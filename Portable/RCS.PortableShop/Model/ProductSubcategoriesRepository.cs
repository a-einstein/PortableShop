﻿using RCS.AdventureWorks.Common.DomainClasses;
using RCS.AdventureWorks.Common.Dtos;
using RCS.PortableShop.ServiceClients.Products.Wrappers;
using System;
using System.Collections.ObjectModel;
using System.ServiceModel;
using System.Threading.Tasks;

namespace RCS.PortableShop.Model
{
    public class ProductSubcategoriesRepository : Repository<ObservableCollection<ProductSubcategory>, ProductSubcategory>
    {
        #region Construction
        public ProductSubcategoriesRepository(IProductService productService)
            : base(productService)
        { }
        #endregion

        #region CRUD
        public async Task<bool> ReadList(bool addEmptyElement = true)
        {
            Clear();

            ProductSubcategoryList subcategories;

            try
            {
                subcategories = await ServiceClient.GetSubcategories().ConfigureAwait(true);
            }
            catch (FaultException<ExceptionDetail> exception)
            {
                SendMessage(exception);
                return false;
            }
            catch (Exception exception)
            {
                SendMessage(exception);
                return false;
            }

            if (addEmptyElement)
            {
                // Name is specifically needed on Android to avoid a NullPointerException.
                var subcategory = new ProductSubcategory() { Name = string.Empty };
                List.Add(subcategory);
            }

            foreach (var subcategory in subcategories)
            {
                List.Add(subcategory);
            }

            return true;
        }
        #endregion
    }
}
