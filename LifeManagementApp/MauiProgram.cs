using Microsoft.Extensions.Logging;
using Microsoft.Maui.Controls.Hosting;
using Microsoft.Maui.Hosting;
using LifeManagementApp.Services;
using CommunityToolkit.Maui;

namespace LifeManagementApp
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>() // Этот вызов должен быть первым
                .UseMauiCommunityToolkitMediaElement() // Этот вызов идёт после UseMauiApp
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                });

            // Подключение WebView и отладочных инструментов для Blazor
            builder.Services.AddMauiBlazorWebView();

#if DEBUG
            builder.Services.AddBlazorWebViewDeveloperTools();
            builder.Logging.AddDebug();
#endif

            // Регистрация WeatherService как singleton
            builder.Services.AddSingleton<WeatherService>();

            return builder.Build();
        }
    }
}




