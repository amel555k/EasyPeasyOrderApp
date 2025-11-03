using EasyPeasyAPP.Pages;
using EasyPeasyAPP.Pages.Auth;
using EasyPeasyAPP.Services;

namespace EasyPeasyAPP
{
    public partial class App : Application
    {
        private readonly IAuthService _authService;

        // Dodaj ovu property da bi drugi page-ovi mogli pristupiti AuthService
        public IAuthService AuthService => _authService;

        public App(IAuthService authService)
        {
            InitializeComponent();
            _authService = authService;

            // Prvo prikaži SplashPage
            MainPage = new SplashPage();
        }

        // Metoda za prelazak nakon splash screen-a
        public async void NavigateToShell()
        {
            // Inicijaliziraj AuthService (učitaj token)
            await _authService.InitializeAsync();

            if (_authService.IsAuthenticated)
            {
                // Ako je logovan, idi na AppShell
                MainPage = new AppShell();
            }
            else
            {
                // Ako nije, prikaži LoginPage (bez argumenata!)
                MainPage = new NavigationPage(new LoginPage());
            }
        }
    }
}