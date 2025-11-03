using EasyPeasyAPP.Models;
using EasyPeasyAPP.Services;

namespace EasyPeasyAPP.Pages.Auth
{
    public partial class LoginPage : ContentPage
    {
        private IAuthService _authService => (Application.Current as App)?.AuthService;

        public LoginPage()
        {
            InitializeComponent();
        }

        private async void OnLoginClicked(object sender, EventArgs e)
        {
            ErrorLabel.IsVisible = false;

            if (string.IsNullOrWhiteSpace(EmailEntry.Text) ||
                string.IsNullOrWhiteSpace(PasswordEntry.Text))
            {
                ShowError("Please fill in all fields");
                return;
            }

            try
            {
                var user = await _authService.LoginWithEmailAsync(
                    EmailEntry.Text.Trim(),
                    PasswordEntry.Text.Trim());

                // Idi na AppShell nakon uspješnog logina
                Application.Current.MainPage = new AppShell();
            }
            catch (Exception ex)
            {
                ShowError(ex.Message);
            }
        }

        private async void OnRegisterTapped(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new RegisterPage());
        }

        private void ShowError(string message)
        {
            ErrorLabel.Text = message;
            ErrorLabel.IsVisible = true;
        }
    }
}