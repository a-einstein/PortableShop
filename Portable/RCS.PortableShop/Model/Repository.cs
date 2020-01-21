using Newtonsoft.Json;
using System;
using System.Collections.ObjectModel;
using System.Net.Http;
using System.Threading.Tasks;

namespace RCS.PortableShop.Model
{
    public abstract class Repository<TCollection, TElement> : ProductsServiceConsumer
        where TCollection : Collection<TElement>, new()
    {
        // Note Derived singletons duplicate construction code, but it it does not seem feasible to share that here.

        #region CRUD
        public TCollection List { get; } = new TCollection();

        public void Clear()
        {
            List.Clear();
        }

        protected static async Task<TList> ReadListApi<TList>(string function, TList result)
        {
            // TODO Should be instantiated once. (Or Disposed of?)
            var httpClient = new HttpClient();
            var uri = new Uri($"{productsApi}/{function}");

            var response = await httpClient.GetAsync(uri);

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                result = JsonConvert.DeserializeObject<TList>(content);
            }

            return result;
        }
        #endregion
    }
}