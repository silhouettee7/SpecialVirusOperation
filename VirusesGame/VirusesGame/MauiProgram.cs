using CommunityToolkit.Maui;
using Microsoft.Extensions.Logging;

namespace VirusesGame
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiCommunityToolkit()
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("ZeroCool.ttf","ZeroCool");
                    fonts.AddFont("Inkverse-BWDRx.ttf","Inkverse");
                    fonts.AddFont("Mateur.ttf", "Mateur");
                });

#if DEBUG
    		builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}
