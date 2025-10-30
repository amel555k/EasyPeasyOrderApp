using EasyPeasyAPP.Pages.Auth;
using EasyPeasyAPP.Services;

namespace EasyPeasyAPP.Pages
{
    public partial class SplashPage : ContentPage
    {
        private readonly IAuthService _authService;

        public SplashPage()
        {
            InitializeComponent();
            _authService = new AuthService();
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            await Task.Delay(100);
            await StartSplashSequence();
        }

        private async Task StartSplashSequence()
        {
            try
            {
                SplashGroup.Opacity = 0;
                await SplashGroup.FadeTo(1, 1000);
                await Task.Delay(1500);
                await SplashGroup.FadeTo(0, 500);

                if (_authService.IsAuthenticated)
                {
                    Application.Current!.MainPage = new MainPage();
                }
                else
                {
                    Application.Current!.MainPage = new NavigationPage(new LoginPage());
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Splash error: {ex.Message}");
                Application.Current!.MainPage = new NavigationPage(new LoginPage());
            }
        }
    }
}