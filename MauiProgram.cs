using CommunityToolkit.Maui;
using Microsoft.Extensions.Logging;
using EcommerceApp.PageModels;
using EcommerceApp.Pages;
using EcommerceApp.Services;
using EcommerceApp.PageModels;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Maui.Storage;
using Syncfusion.Maui.Toolkit.Hosting;

namespace EcommerceApp
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .UseMauiCommunityToolkit()
                .ConfigureSyncfusionToolkit()
                .ConfigureMauiHandlers(handlers =>
                {
                })
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                    fonts.AddFont("SegoeUI-Semibold.ttf", "SegoeSemibold");
                    fonts.AddFont("FluentSystemIcons-Regular.ttf", FluentUI.FontFamily);
                });

#if DEBUG
    		builder.Logging.AddDebug();
    		builder.Services.AddLogging(configure => configure.AddDebug());

#endif


            builder.Services.AddTransient<MainPageModel>(sp =>
                new MainPageModel(sp.GetRequiredService<DatabaseContext>()));

            builder.Services.AddTransient<CartPageModel>(sp =>
                new CartPageModel(sp.GetRequiredService<DatabaseContext>()));

            builder.Services.AddTransient<ProfilePageModel>(sp =>
                new ProfilePageModel(
                    sp.GetRequiredService<DatabaseContext>(),
                    sp.GetRequiredService<IMediaPicker>(),
                    sp.GetRequiredService<IFileSystem>()
                ));

            builder.Services.AddSingleton<NavigationService>();
            // Register Database

            // Register Database
            builder.Services.AddSingleton<DatabaseContext>();



            builder.Services.AddTransient<MainPageModel>();
            builder.Services.AddTransient<ProfilePageModel>();
            builder.Services.AddTransient<CartPageModel>();

            // Register pages
            builder.Services.AddTransient<MainPage>();
            builder.Services.AddTransient<ProfilePage>();
            builder.Services.AddTransient<CartPage>();

            // Register System Services
            builder.Services.AddSingleton(FileSystem.Current);
            builder.Services.AddSingleton(MediaPicker.Default);

            return builder.Build();
        }
    }
}
