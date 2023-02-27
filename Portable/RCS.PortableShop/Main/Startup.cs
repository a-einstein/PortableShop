using RCS.AdventureWorks.Common.DomainClasses;
using RCS.PortableShop.Common.Interfaces;
using RCS.PortableShop.Model;
using RCS.PortableShop.ServiceClients.Products.Wrappers;
using RCS.PortableShop.ViewModels;

namespace RCS.PortableShop.Main
{
    // This could be integrated in MainApplication.
    public static class Startup
    {
        public static IServiceProvider ServiceProvider { get; private set; }

        public static MauiAppBuilder RegisterAppServices(this MauiAppBuilder mauiAppBuilder)
        {
            var services = mauiAppBuilder.Services;

            services.AddHttpClient();

            // Note a restart is needed to actually switch IProductService,
            // as there does not seem to be a feasible way to do that while running.
            // https://stackoverflow.com/questions/69004937/how-to-use-servicecollection-replace-in-dependency-injection
            // TODO Apply some proxy as suggested?
            switch (Settings.ServiceType)
            {
                case ServiceType.WCF:
                    services.AddSingleton<IProductService, WcfClient>();
                    break;
                case ServiceType.CoreWcf:
                    services.AddSingleton<IProductService, CoreWcfClient>();
                    break;
                case ServiceType.WebApi:
                default:
                    services.AddSingleton<IProductService, WebApiClient>();
                    break;
            }

            // Use interfaces for constructor injections.
            services.AddSingleton<IRepository<List<ProductCategory>, ProductCategory>, ProductCategoriesRepository>();
            services.AddSingleton<IRepository<List<ProductSubcategory>, ProductSubcategory>, ProductSubcategoriesRepository>();
            services.AddSingleton<IFilterRepository<List<ProductsOverviewObject>, ProductsOverviewObject, ProductCategory, ProductSubcategory, int>, ProductsRepository>();
            services.AddSingleton<IRepository<List<CartItem>, CartItem>, CartItemsRepository>();

            // Use types for explicit requests, implicitly using repositories.
            services.AddSingleton<ProductsViewModel>();
            services.AddSingleton<ProductViewModel>();
            services.AddSingleton<CartViewModel>();

            // Notes
            // - Apparently no ResourcesPath needed.
            // - Apparently no MarkupExtension needed for XAML.
            services.AddLocalization();

            ServiceProvider = services.BuildServiceProvider();

            return mauiAppBuilder;
        }
    }
}
