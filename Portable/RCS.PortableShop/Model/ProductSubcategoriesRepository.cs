﻿using RCS.AdventureWorks.Common.DomainClasses;
using RCS.PortableShop.ServiceClients.Products.ProductsService;
using System;
using System.Threading.Tasks;

namespace RCS.PortableShop.Model
{
    public class ProductSubcategoriesRepository : Repository<ProductSubcategory>
    {
        private ProductSubcategoriesRepository()
        { }

        private static volatile ProductSubcategoriesRepository instance;
        private static object syncRoot = new Object();

        public static ProductSubcategoriesRepository Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (syncRoot)
                    {
                        if (instance == null)
                            instance = new ProductSubcategoriesRepository();
                    }
                }

                return instance;
            }
        }

        public async Task ReadList(bool addEmptyElement = true)
        {
            Clear();
            var subcategories = await Task.Factory.FromAsync<ProductSubcategoryList>(
                ProductsServiceClient.BeginGetProductSubcategories,
                ProductsServiceClient.EndGetProductSubcategories,
                null);

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
        }
    }
}
