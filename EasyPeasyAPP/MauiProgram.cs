using Microsoft.Extensions.Logging;
using EasyPeasyAPP.Services;
using EasyPeasyAPP.Pages.Auth;
using EasyPeasyAPP.Pages;

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

            // Registruj servise kao Singleton (jedna instanca za cijelu app)
            builder.Services.AddSingleton<IAuthService, AuthService>();
            builder.Services.AddSingleton<IKorpaService, KorpaService>();
            builder.Services.AddSingleton<INarudzbaService, NarudzbaService>();

            // Registruj sve page-ove kao Transient (nova instanca svaki put)
            builder.Services.AddTransient<LoginPage>();
            builder.Services.AddTransient<RegisterPage>();
            builder.Services.AddTransient<MainPage>();
            builder.Services.AddTransient<ProfilePage>();
            builder.Services.AddTransient<OrderPage>();
            builder.Services.AddTransient<AboutPage>();
            builder.Services.AddTransient<KorpaPage>();

#if DEBUG
            builder.Logging.AddDebug();
#endif
            return builder.Build();
        }
    }
}