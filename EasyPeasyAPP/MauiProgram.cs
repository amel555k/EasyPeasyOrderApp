using Microsoft.Extensions.Logging;

namespace EasyPeasyAPP
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
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                    fonts.AddFont("BebasNeue-Regular.ttf", "BebasNeue");
                    fonts.AddFont("LeagueSpartan-Regular.ttf", "LeagueSpartan");
                    fonts.AddFont("LeagueSpartan-Bold.ttf", "LeagueSpartanBold");
                });

#if DEBUG
            builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}