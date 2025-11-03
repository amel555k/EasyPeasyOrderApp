using Microsoft.Extensions.Logging;
using EasyPeasyAPP.Services;
using EasyPeasyAPP.Pages.Auth;

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

            // Registruj AuthService kao Singleton (jedna instanca za cijelu app)
            builder.Services.AddSingleton<IAuthService, AuthService>();

            // Registruj sve page-ove kao Transient (nova instanca svaki put)
            builder.Services.AddTransient<LoginPage>();
            builder.Services.AddTransient<RegisterPage>();
            builder.Services.AddTransient<MainPage>();

            // Dodaj i ostale page-ove koje imaš (ProfilePage, OrderPage, AboutPage, itd.)

#if DEBUG
            builder.Logging.AddDebug();
#endif
            return builder.Build();
        }
    }
}