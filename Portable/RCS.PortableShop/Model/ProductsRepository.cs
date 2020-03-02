using RCS.AdventureWorks.Common.DomainClasses;
using RCS.AdventureWorks.Common.Dtos;
using RCS.PortableShop.ServiceClients.Products.Wrappers;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ServiceModel;
using System.Threading.Tasks;

namespace RCS.PortableShop.Model
{
    public class ProductsRepository : Repository<ObservableCollection<ProductsOverviewObject>, ProductsOverviewObject>
    {
        #region Construction
        public ProductsRepository(IProductService productService)
            : base(productService)
        { }
        #endregion

        #region CRUD
        // TODO This should get paged with an optional pagesize.
        public async Task<IList<ProductsOverviewObject>> ReadList(ProductCategory category, ProductSubcategory subcategory, string namePart)
        {
            var productsOverview = new ProductsOverviewList();

            try
            {
                productsOverview = await ServiceClient.GetProducts(category, subcategory, namePart).ConfigureAwait(true);
            }
            catch (FaultException<ExceptionDetail> exception)
            {
                SendMessage(exception);
                return null;
            }
            catch (Exception exception)
            {
                SendMessage(exception);
                return null;
            }

            return productsOverview;
        }

        public async Task<Product> ReadDetails(int productID)
        {
            Product product = null;

            try
            {
                product = await ServiceClient.GetProduct(productID).ConfigureAwait(true);
            }
            catch (FaultException<ExceptionDetail> exception)
            {
                SendMessage(exception);
            }
            catch (Exception exception)
            {
                SendMessage(exception);
            }

            return product;
        }
        #endregion
    }
}