using EasyPeasyAPP.Pages.Auth;
using EasyPeasyAPP.Services;

namespace EasyPeasyAPP.Pages
{
    public partial class SplashPage : ContentPage
    {
        public SplashPage()
        {
            InitializeComponent();
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
                // Animacija splash screen-a
                SplashGroup.Opacity = 0;
                await SplashGroup.FadeTo(1, 1000);
                await Task.Delay(1500);
                await SplashGroup.FadeTo(0, 500);

                // Pozovi NavigateToShell iz App.xaml.cs
                // On će inicijalizirati AuthService i odlučiti gdje ići
                (Application.Current as App)?.NavigateToShell();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Splash error: {ex.Message}");

                // Fallback na LoginPage ako nešto pukne
                Application.Current!.MainPage = new NavigationPage(new LoginPage());
            }
        }
    }
}