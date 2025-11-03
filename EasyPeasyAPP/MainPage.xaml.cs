using EasyPeasyAPP.Pages.Auth;
using EasyPeasyAPP.Services;

namespace EasyPeasyAPP
{
    public partial class MainPage : ContentPage
    {
        private IAuthService _authService => (Application.Current as App)?.AuthService;

        public MainPage()
        {
            InitializeComponent();
        }

        private async void OnNaruciClicked(object sender, EventArgs e)
        {
            if (_authService?.IsAuthenticated != true)
            {
                await DisplayAlert("Greška", "Morate biti prijavljeni da naručite.", "OK");
                return;
            }
            await Shell.Current.GoToAsync("//OrderPage");
        }

        private async void OnONamaClicked(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync("//AboutPage");
        }

        private async void OnProfilClicked(object sender, EventArgs e)
        {
            if (_authService?.IsAuthenticated != true)
            {
                await DisplayAlert("Greška", "Morate biti prijavljeni da vidite profil.", "OK");
                return;
            }
            await Shell.Current.GoToAsync("ProfilePage");
        }

        private async void OnLogoutClicked(object sender, EventArgs e)
        {
            if (_authService?.IsAuthenticated != true)
            {
                await DisplayAlert("Info", "Niste prijavljeni.", "OK");
                return;
            }

            bool confirm = await DisplayAlert("Logout", "Da li želite da se odjavite?", "Da", "Ne");
            if (confirm)
            {
                await _authService.LogoutAsync();
                Application.Current.MainPage = new NavigationPage(new LoginPage());
            }
        }
    }
}