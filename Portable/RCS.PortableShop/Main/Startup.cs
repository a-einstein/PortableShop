using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using RCS.PortableShop.ServiceClients.Products.Wrappers;
using System;

namespace RCS.PortableShop.Main
{
    // This could be integrated in MainApplication.
    public static class Startup
    {
        public static IServiceProvider ServiceProvider { get; set; }

        public static void Init()
        {
            // Had to use this way of instantiation as new HostBuilder() gave exceptions.
            // https://github.com/dotnet/extensions/issues/2182

            var hostBuilder = Host.CreateDefaultBuilder().
                ConfigureServices((context, services) =>
                {
                    services.AddHttpClient();
                    services.AddSingleton<WebApiClient>();
                });

            var host = hostBuilder.Build();

            ServiceProvider = host.Services;
        }
    }
}
