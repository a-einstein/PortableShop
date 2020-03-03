using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using RCS.PortableShop.Model;
using RCS.PortableShop.ServiceClients.Products.Wrappers;
using System;

namespace RCS.PortableShop.Main
{
    // This could be integrated in MainApplication.
    public static class Startup
    {
        public static IServiceProvider ServiceProvider { get; private set; }

        public static void Init()
        {
            // Had to use this way of instantiation as new HostBuilder() gave exceptions.
            // https://github.com/dotnet/extensions/issues/2182

            var hostBuilder = Host.CreateDefaultBuilder().
                ConfigureServices((context, services) =>
                {
                    services.AddHttpClient();

                    // Note a restart is needed to actually switch IProductService,
                    // as there does not seem to be a feasible way to do that while running.
                    switch (Settings.ServiceTypeSelected)
                    {
                        case Settings.ServiceType.WCF:
                            services.AddSingleton<IProductService, WcfClient>();
                            break;
                        case Settings.ServiceType.WebApi:
                            services.AddSingleton<IProductService,WebApiClient>();
                            break;
                    }

                    // Note this does not work (yet) as true injection by an interface.
                    services.AddSingleton<ProductCategoriesRepository>();
                    services.AddSingleton<ProductSubcategoriesRepository>();
                    services.AddSingleton<ProductsRepository>();
                    services.AddSingleton<CartItemsRepository>();
                });

            var host = hostBuilder.Build();

            ServiceProvider = host.Services;
        }
    }
}
