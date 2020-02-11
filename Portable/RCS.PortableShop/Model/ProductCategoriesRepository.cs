using RCS.AdventureWorks.Common.DomainClasses;
using RCS.AdventureWorks.Common.Dtos;
using System;
using System.Collections.ObjectModel;
using System.ServiceModel;
using System.Threading.Tasks;

namespace RCS.PortableShop.Model
{
    public class ProductCategoriesRepository : Repository<ObservableCollection<ProductCategory>, ProductCategory>
    {
        #region Construction
        private ProductCategoriesRepository()
        { }

        private static volatile ProductCategoriesRepository instance;
        private static object syncRoot = new Object();

        public static ProductCategoriesRepository Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (syncRoot)
                    {
                        if (instance == null)
                            instance = new ProductCategoriesRepository();
                    }
                }

                return instance;
            }
        }
        #endregion

        #region CRUD

        protected override string EntitiesName => "ProductCategories";

        public async Task<bool> ReadList(bool addEmptyElement = true)
        {
            Clear();

            var categories = new ProductCategoryList();

            try
            {
                // TODO Create some sort of injection somewhere?
                switch (preferredServiceType)
                {
                    case ServiceType.WCF:
                        categories = await Task.Factory.FromAsync<ProductCategoryList>(
                            ProductsServiceClient.BeginGetProductCategories,
                            ProductsServiceClient.EndGetProductCategories,
                            null).ConfigureAwait(true);
                        break;
                    case ServiceType.WebApi:
                        categories = await ReadApi<ProductCategoryList>().ConfigureAwait(true);
                        break;
                    default:
                        throw new NotImplementedException($"Unknown {nameof(ServiceType)}");
                        break;
                }
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
                var category = new ProductCategory() { Name = string.Empty };
                List.Add(category);
            }

            foreach (var category in categories)
            {
                List.Add(category);
            }

            return true;
        }
        #endregion
    }
}
