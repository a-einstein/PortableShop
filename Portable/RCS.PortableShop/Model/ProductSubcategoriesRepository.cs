using RCS.AdventureWorks.Common.DomainClasses;
using RCS.AdventureWorks.Common.Dtos;
using RCS.PortableShop.ServiceClients.Products.Wrappers;

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
        protected override async Task<bool> Read(bool addEmptyElement = true)
        {
            ProductSubcategoryList subcategories;

            try
            {
                subcategories = await ServiceClient.GetSubcategories().ConfigureAwait(true);
            }
            //catch (FaultException<ExceptionDetail> exception)
            //{
            //    SendMessage(exception);
            //    return false;
            //}
            catch (Exception exception)
            {
                SendMessage(exception);
                return false;
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

            return true;
        }
        #endregion
    }
}
