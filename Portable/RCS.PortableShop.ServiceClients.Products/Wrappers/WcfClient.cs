﻿using RCS.AdventureWorks.Common.DomainClasses;
using RCS.AdventureWorks.Common.Dtos;
using RCS.PortableShop.ServiceClients.Products.ProductsService;
using System;
using System.ServiceModel;
using System.Threading.Tasks;

namespace RCS.PortableShop.ServiceClients.Products.Wrappers
{
    public class WcfClient : ServiceClient, IProductService, IDisposable
    {
        #region Interface
        public async Task<ProductCategoryList> GetCategories()
        {
            var result = await Task.Factory.FromAsync<ProductCategoryList>(
                ProductsServiceClient.BeginGetProductCategories,
                ProductsServiceClient.EndGetProductCategories,
                null).ConfigureAwait(true);

            return result;
        }

        public async Task<ProductSubcategoryList> GetSubcategories()
        {
            var result = await Task.Factory.FromAsync<ProductSubcategoryList>(
               ProductsServiceClient.BeginGetProductSubcategories,
               ProductsServiceClient.EndGetProductSubcategories,
               null).ConfigureAwait(true);

            return result;
        }

        public async Task<ProductsOverviewList> GetProducts(ProductCategory category, ProductSubcategory subcategory, string namePart)
        {
            var result = await Task.Factory.FromAsync<int?, int?, string, ProductsOverviewList>(
               ProductsServiceClient.BeginGetProductsOverviewBy,
               ProductsServiceClient.EndGetProductsOverviewBy,
               category?.Id, subcategory?.Id, namePart,
               null).ConfigureAwait(true);

            return result;
        }

        public async Task<Product> GetProduct(int productID)
        {
            var result = await Task.Factory.FromAsync<int, Product>(
                  ProductsServiceClient.BeginGetProductDetails,
                  ProductsServiceClient.EndGetProductDetails,
                  productID,
                  null).ConfigureAwait(true);

            return result;
        }
        #endregion

        #region Utilities

        private ProductsServiceClient productsServiceClient;

        protected IProductsService ProductsServiceClient
        {
            get
            {
                try
                {
                    // TODO >> Does not work as the intended singleton. Is that useful & necessary? Think to have seen not.
                    if (productsServiceClient == null)
                    {
                        // TODO Make this better configurable. There does not seem to be a config file like on WPF.
                        // TODO If possible get transformation on configs. 

                        // Note that currently wsHttpBinding is not supported, but should be as it is part of System.ServiceModel 4.0.0.0.
                        var binding = new BasicHttpBinding(BasicHttpSecurityMode.Transport) { OpenTimeout = Timeout, SendTimeout = Timeout, ReceiveTimeout = Timeout, CloseTimeout = Timeout };

                        // Note this points to a BasicHttpBinding variant on the server.
                        string endpointAddress = $"{serviceDomain}/ProductsServicePub/ProductsService.svc/ProductsServiceB";

                        // Note the example bindings in ProductsServiceClient which could also be applied here by using EndpointConfiguration
                        productsServiceClient = new ProductsServiceClient(binding, new EndpointAddress(endpointAddress));
                    }

                }
                catch (Exception)
                {
                    throw;
                }

                return productsServiceClient;
            }
        }
        #endregion

        #region IDisposable
        // Check out the IDisposable documentation for details on the pattern applied here.
        // Note that it can have implications on derived classes too.

        private bool disposed = false;

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private void Dispose(bool disposing)
        {
            if (disposed)
                return;

            if (disposing)
            {
                productsServiceClient?.Dispose();
            }

            disposed = true;
        }

        ~WcfClient()
        {
            Dispose(false);
        }
        #endregion
    }
}
