using CommunityToolkit.Maui;
using RCS.PortableShop.Main;

namespace RCS.PortableShop
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();

            builder
                .UseMauiApp<App>()
                .UseMauiCommunityToolkit();

            Startup.RegisterAppServices(builder);

            return builder.Build();
        }
    }
}
