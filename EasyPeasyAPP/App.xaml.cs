using EasyPeasyAPP.Pages;
using EasyPeasyAPP.Pages.Auth;
using EasyPeasyAPP.Services;

namespace EasyPeasyAPP
{
    public partial class App : Application
    {
        private readonly IAuthService _authService;
        private readonly IKorpaService _korpaService;
        private readonly INarudzbaService _narudzbaService;

        // Public property da drugi page-ovi mogu pristupiti servisima
        public IAuthService AuthService => _authService;
        public IKorpaService KorpaService => _korpaService;
        public INarudzbaService NarudzbaService => _narudzbaService;

        // Konstruktor koristi Dependency Injection (DI)
        public App(IAuthService authService, IKorpaService korpaService, INarudzbaService narudzbaService)
        {
            InitializeComponent();

            _authService = authService;
            _korpaService = korpaService;
            _narudzbaService = narudzbaService;

            // Početni ekran (Splash)
            MainPage = new SplashPage();
        }

        // Metoda koja se poziva nakon Splash-a da odredi što dalje
        public async void NavigateToShell()
        {
            // Inicijalizuj AuthService (učitava token i usera)
            await _authService.InitializeAsync();

            if (_authService.IsAuthenticated)
            {
                // Ako je korisnik već prijavljen, ide direktno u AppShell
                MainPage = new AppShell();
            }
            else
            {
                // Inače prikaži login ekran
                MainPage = new NavigationPage(new LoginPage());
            }
        }
    }
}
