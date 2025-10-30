using EasyPeasyAPP.Pages;
using EasyPeasyAPP.Pages.Auth;
using EasyPeasyAPP.Services;

namespace EasyPeasyAPP
{
    public partial class MainPage : ContentPage
    {
        private readonly IAuthService _authService;

        public MainPage()
        {
            InitializeComponent();
            _authService = new AuthService();
        }

        private async void OnNaruciClicked(object sender, EventArgs e)
        {
            Application.Current!.MainPage = new OrderPage();
        }


        private void OnONamaClicked(object sender, EventArgs e)
        {
            // Direktno zamijeni trenutni ekran sa AboutPage
            Application.Current!.MainPage = new AboutPage();
        }

        private async void OnProfilClicked(object sender, EventArgs e)
        {
            await DisplayAlert("Profil", "Otvara se profil...", "OK");
        }

        private async void OnLogoutClicked(object sender, EventArgs e)
        {
            bool confirm = await DisplayAlert("Logout", "Da li želite da se odjavite?", "Da", "Ne");
            if (confirm)
            {
                await _authService.LogoutAsync();
                Application.Current!.MainPage = new NavigationPage(new LoginPage());
            }
        }
    }
}
