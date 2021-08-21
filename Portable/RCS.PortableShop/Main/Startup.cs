using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using RCS.PortableShop.Model;
using RCS.PortableShop.ServiceClients.Products.Wrappers;
using RCS.PortableShop.ViewModels;
using System;
using Xamarin.Essentials;

namespace RCS.PortableShop.Main
{
    // This could be integrated in MainApplication.
    public static class Startup
    {
        public static IServiceProvider ServiceProvider { get; private set; }

        public static void Init()
        {
            /*
             * Note other peculiarities for deployment.
             * - Android can't handle too long development paths, like described here.
             *   https://stackoverflow.com/questions/57744837/could-not-find-a-part-of-the-path-accessibilitymanagercompat-accessibilitystate
             * - UWP seems not able to handle a mapped drive, like described here.
             *   https://stackoverflow.com/questions/42020845/error-dep0700-registration-of-the-app-failed-on-windows-10-on-a-macbook-dual
             * TODO Create one environment for both.
             * */

            IHostBuilder hostBuilder;
            Platform platform = default;

            // Make use of own enum to switch on. Astounding it did not already exist.
            Enum.TryParse(DeviceInfo.Platform.ToString(), out platform);

            // Had to differentiate for platforms, as described below.
            // https://github.com/dotnet/extensions/issues/2182
            // https://github.com/xamarin/MobileBlazorBindings/issues/41

            switch (platform)
            {
                case Platform.Android:
                    hostBuilder = Host.CreateDefaultBuilder();
                    break;
                case Platform.iOS:
                case Platform.UWP:
                    hostBuilder = new HostBuilder();
                    break;
                default:
                    // TODO Message instead. Note currently that sequentially is hard.
                    throw new PlatformNotSupportedException($"Platform = '{platform}'.");
            }

            hostBuilder.ConfigureServices((context, services) =>
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
                        services.AddSingleton<IProductService, WebApiClient>();
                        break;
                }

                // Note this does not work (yet) as true injection by an interface.
                services.AddSingleton<ProductCategoriesRepository>();
                services.AddSingleton<ProductSubcategoriesRepository>();
                services.AddSingleton<ProductsRepository>();
                services.AddSingleton<CartItemsRepository>();

                services.AddSingleton<ShoppingCartViewModel>();
            });

            var host = hostBuilder.Build();

            ServiceProvider = host.Services;
        }

        private enum Platform
        {
            Unknown = 0,
            Android,
            iOS,
            UWP
        }
    }
}
