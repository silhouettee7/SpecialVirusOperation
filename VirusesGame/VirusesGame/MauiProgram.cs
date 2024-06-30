using Microsoft.Extensions.Logging;

namespace VirusesGame
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("ZeroCool.ttf","ZeroCool");
                    fonts.AddFont("Inkverse-BWDRx.ttf","Inkverse");
                });

#if DEBUG
    		builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}
