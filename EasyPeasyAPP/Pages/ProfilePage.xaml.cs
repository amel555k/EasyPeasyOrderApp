using EasyPeasyAPP.Models;
using EasyPeasyAPP.Services;

namespace EasyPeasyAPP.Pages
{
    public partial class ProfilePage : ContentPage
    {
        private IAuthService _authService => (Application.Current as App)?.AuthService;
        private UserModel _currentUser;

        public ProfilePage()
        {
            InitializeComponent();
        }
        private async void OnBackClicked(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync("//MainPage");
        }
        protected override async void OnAppearing()
        {
            base.OnAppearing();

            try
            {
                // Dohvati trenutnog korisnika iz AuthService
                _currentUser = _authService?.CurrentUser;

                if (_currentUser == null)
                {
                    await DisplayAlert("Error", "Niste prijavljeni.", "OK");
                    await Shell.Current.GoToAsync("//MainPage");
                    return;
                }

                // Popuni formu sa trenutnim podacima
                DisplayNameEntry.Text = _currentUser.Ime ?? "";
                EmailEntry.Text = _currentUser.Email ?? "";
                TelefonEntry.Text = _currentUser.Telefon ?? "";
                AdresaEntry.Text = _currentUser.Adresa ?? "";
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", $"Ne mogu dohvatiti korisnika: {ex.Message}", "OK");
            }
        }

        private async void OnSaveClicked(object sender, EventArgs e)
        {
            ErrorLabel.IsVisible = false;

            string newName = DisplayNameEntry.Text?.Trim() ?? "";
            string newEmail = EmailEntry.Text?.Trim() ?? "";
            string newPhone = TelefonEntry.Text?.Trim() ?? "";
            string newAddress = AdresaEntry.Text?.Trim() ?? "";

            if (string.IsNullOrWhiteSpace(newName) ||
                string.IsNullOrWhiteSpace(newEmail))
            {
                ShowError("Ime i email su obavezni.");
                return;
            }

            try
            {
                // Ažuriraj lokalni user objekat
                _currentUser.Ime = newName;
                _currentUser.Email = newEmail;
                _currentUser.Telefon = newPhone;
                _currentUser.Adresa = newAddress;

                // Sačuvaj u Firebase
                await _authService.UpdateUserAsync(_currentUser);

                await DisplayAlert("Success", "Informacije su uspješno ažurirane.", "OK");
            }
            catch (Exception ex)
            {
                ShowError($"Greška pri čuvanju: {ex.Message}");
            }
        }

        private void ShowError(string message)
        {
            ErrorLabel.Text = message;
            ErrorLabel.IsVisible = true;
        }
    }
}