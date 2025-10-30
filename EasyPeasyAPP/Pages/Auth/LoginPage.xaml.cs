using EasyPeasyAPP.Pages.Auth;
using EasyPeasyAPP.Services;

namespace EasyPeasyAPP.Pages.Auth
{
    public partial class LoginPage : ContentPage
    {
        private readonly IAuthService _authService;

        public LoginPage()
        {
            InitializeComponent();
            _authService = new AuthService();
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
                    PasswordEntry.Text);

                Application.Current!.MainPage = new MainPage();
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