using RCS.AdventureWorks.Common.DomainClasses;
using RCS.AdventureWorks.Common.Dtos;
using RCS.PortableShop.Common.Interfaces;
using RCS.PortableShop.ServiceClients.Products.Wrappers;
using System;
using System.Collections.Generic;
using System.ServiceModel;
using System.Threading.Tasks;

namespace RCS.PortableShop.Model
{
    public class ProductsRepository :
        Repository<List<ProductsOverviewObject>, ProductsOverviewObject>,
        IFilterRepository<List<ProductsOverviewObject>, ProductsOverviewObject, ProductCategory, ProductSubcategory, int>
    {
        #region Construction
        public ProductsRepository(IProductService productService)
            : base(productService)
        { }
        #endregion

        #region Refresh
        public async Task Refresh(ProductCategory category, ProductSubcategory subcategory, string namePart)
        {
            await Clear().ConfigureAwait(true);
            await Read(category, subcategory, namePart).ConfigureAwait(true);
        }
        #endregion

        #region CRUD
        // TODO This should get paged with an optional pagesize.
        private async Task Read(ProductCategory category, ProductSubcategory subcategory, string namePart)
        {
            ProductsOverviewList productsOverview;

            try
            {
                productsOverview = await ServiceClient.GetProducts(category, subcategory, namePart).ConfigureAwait(true);
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

            foreach (var product in productsOverview)
            {
                items.Add(product);
            }
        }

        // This creates a wrapper, besides the repository-like concept.
        public async Task<Product> Details(int productId)
        {
            Product product = null;

            try
            {
                product = await ServiceClient.GetProduct(productId).ConfigureAwait(true);
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