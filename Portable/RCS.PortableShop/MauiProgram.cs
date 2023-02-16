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
                 // Needed this explicit reference instead of using, or even using =, because of nameconflict on the Android platform.
                .UseMauiApp<RCS.PortableShop.Main.MainApplication>()
                .UseMauiCommunityToolkit();

            Startup.RegisterAppServices(builder);

            return builder.Build();
        }
    }
}
