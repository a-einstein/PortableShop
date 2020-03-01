using RCS.AdventureWorks.Common.DomainClasses;
using RCS.AdventureWorks.Common.Dtos;
using System;
using System.Collections.ObjectModel;
using System.ServiceModel;
using System.Threading.Tasks;
using static RCS.PortableShop.Model.Settings;

namespace RCS.PortableShop.Model
{
    public class ProductSubcategoriesRepository : Repository<ObservableCollection<ProductSubcategory>, ProductSubcategory>
    {
        #region Construction
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
        #endregion

        #region CRUD
        public async Task<bool> ReadList(bool addEmptyElement = true)
        {
            Clear();

            var subcategories = new ProductSubcategoryList();

            try
            {
                // TODO Create some sort of injection somewhere?
                switch (ServiceTypeSelected)
                {
                    case ServiceType.WCF:
                        subcategories = await WcfClient.GetSubcategories().ConfigureAwait(true);
                        break;
                    case ServiceType.WebApi:
                        subcategories = await WebApiClient.GetSubcategories().ConfigureAwait(true);
                        break;
                    default:
                        throw new NotImplementedException($"Unknown {nameof(ServiceType)}");
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
