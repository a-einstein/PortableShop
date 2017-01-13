﻿using RCS.AdventureWorks.Common.DomainClasses;
using RCS.PortableShop.ServiceClients.Products.ProductsService;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RCS.PortableShop.Model
{
    public class ProductsRepository : Repository<ProductsOverviewObject>
    {
        private ProductsRepository()
        { }

        private static volatile ProductsRepository instance;
        private static object syncRoot = new Object();

        public static ProductsRepository Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (syncRoot)
                    {
                        if (instance == null)
                            instance = new ProductsRepository();
                    }
                }

                return instance;
            }
        }

        // TODO This should get paged with an optional pagesize.
        public async Task<IList<ProductsOverviewObject>> ReadList(ProductCategory category, ProductSubcategory subcategory, string namePart)
        {
            var productsOverview = await Task.Factory.FromAsync<int?, int?, string, ProductsOverviewList>(
                ProductsServiceClient.BeginGetProductsOverviewBy,
                ProductsServiceClient.EndGetProductsOverviewBy,
                category?.Id, subcategory?.Id, namePart,
                null);

            return productsOverview;
        }

        public async Task<Product> ReadDetails(int productID)
        {
            var product = await Task.Factory.FromAsync<int, Product>(
                ProductsServiceClient.BeginGetProductDetails,
                ProductsServiceClient.EndGetProductDetails,
                productID,
                null);

            return product;

        }
    }
}