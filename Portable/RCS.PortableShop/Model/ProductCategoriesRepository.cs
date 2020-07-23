using RCS.AdventureWorks.Common.DomainClasses;
using RCS.AdventureWorks.Common.Dtos;
using RCS.PortableShop.ServiceClients.Products.Wrappers;
using System;
using System.Collections.Generic;
using System.ServiceModel;
using System.Threading.Tasks;

namespace RCS.PortableShop.Model
{
    public class ProductCategoriesRepository : Repository<List<ProductCategory>, ProductCategory>
    {
        #region Construction
        public ProductCategoriesRepository(IProductService productService)
            : base(productService)
        { }
        #endregion

        #region CRUD
        protected override async Task Read(bool addEmptyElement)
        {
            ProductCategoryList categories;

            try
            {
                categories = await ServiceClient.GetCategories().ConfigureAwait(true);
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
                var category = new ProductCategory() { Name = string.Empty };
                items.Add(category);
            }

            foreach (var category in categories)
            {
                items.Add(category);
            }
        }
        #endregion
    }
}
