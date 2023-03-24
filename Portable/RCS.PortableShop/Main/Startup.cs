using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using RCS.AdventureWorks.Common.DomainClasses;
using RCS.PortableShop.Common.Interfaces;
using RCS.PortableShop.Model;
using RCS.PortableShop.ServiceClients.Products.Wrappers;
using RCS.PortableShop.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
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


            // Based on https://startdebugging.net/2020/11/how-to-use-appsettings-json-with-xamarin-forms/
            var assembly = Assembly.GetExecutingAssembly();
          
            // Note this file needs to be a resource.
            Stream resourceStream = assembly.GetManifestResourceStream($"{assembly.GetName().Name}.appsettings.json");

            var configuration = new ConfigurationBuilder().AddJsonStream(resourceStream).Build();

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

                // Note Configuration based on https://code-maze.com/dotnet-using-constructor-injection/

                // Note a restart is needed to actually switch IProductService,
                // as there does not seem to be a feasible way to do that while running.
                // https://stackoverflow.com/questions/69004937/how-to-use-servicecollection-replace-in-dependency-injection
                // TODO Apply some proxy as suggested?

                switch (Settings.ServiceType)
                {
                    // TODO Rename the clients, as indicated.
                    case ServiceType.WCF:
                        {
                            var options = configuration.GetSection("WcfOptions");
                            services.Configure<ServiceOptions>(options);

                            switch (platform)
                            {
                                case Platform.Android:
                                    // Reuse the old non Task oriented client because of https://github.com/dotnet/wcf/issues/2463
                                    services.AddSingleton<IProductService, WcfClient>();
                                    break;
                                case Platform.UWP:
                                    // Use the newly generated Task oriented client.
                                    services.AddSingleton<IProductService, CoreWcfClient>();
                                    break;
                                default:
                                    throw new PlatformNotSupportedException($"Platform = '{platform}'.");
                            }
                        }
                        break;
                    case ServiceType.CoreWcf:
                        {
                            var options = configuration.GetSection("CoreWcfOptions");
                            services.Configure<ServiceOptions>(options);

                            switch (platform)
                            {
                                case Platform.Android:
                                    // Reuse the old non Task oriented client because of https://github.com/dotnet/wcf/issues/2463
                                    services.AddSingleton<IProductService, WcfClient>();
                                    break;
                                case Platform.UWP:
                                    // Use the newly generated Task oriented client.
                                    services.AddSingleton<IProductService, CoreWcfClient>();
                                    break;
                                default:
                                    throw new PlatformNotSupportedException($"Platform = '{platform}'.");
                            }
                        }
                        break;
                    case ServiceType.WebApi:
                    default:
                        {
                            var options = configuration.GetSection("WebApiOptions");
                            services.Configure<ServiceOptions>(options);

                            services.AddSingleton<IProductService, WebApiClient>();
                        }
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
