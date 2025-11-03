using EasyPeasyAPP.Models;
using EasyPeasyAPP.Services;

namespace EasyPeasyAPP.Pages.Auth;

public partial class RegisterPage : ContentPage
{
    private readonly IAuthService _authService;

    public RegisterPage()
    {
        InitializeComponent();
        _authService = new AuthService();
    }

    private async void OnRegisterClicked(object sender, EventArgs e)
    {
        ErrorLabel.IsVisible = false;

        if (string.IsNullOrWhiteSpace(DisplayNameEntry.Text) ||
            string.IsNullOrWhiteSpace(EmailEntry.Text) ||
            string.IsNullOrWhiteSpace(PasswordEntry.Text) ||
            string.IsNullOrWhiteSpace(ConfirmPasswordEntry.Text))
        {
            ShowError("Please fill in all fields");
            return;
        }

        if (PasswordEntry.Text != ConfirmPasswordEntry.Text)
        {
            ShowError("Passwords do not match");
            return;
        }

        if (PasswordEntry.Text.Length < 6)
        {
            ShowError("Password must be at least 6 characters");
            return;
        }

        try
        {
            var user = await _authService.RegisterWithEmailAsync(
                EmailEntry.Text.Trim(),
                PasswordEntry.Text.Trim(),
                DisplayNameEntry.Text.Trim(),
                TelefonEntry.Text?.Trim(),
                AdresaEntry.Text?.Trim()
            );

            // Idi na MainPage unutar Shell-a
            Application.Current.MainPage = new AppShell();
        }
        catch (Exception ex)
        {
            ShowError(ex.Message);
        }
    }

    private async void OnLoginTapped(object sender, EventArgs e)
    {
        await Navigation.PopAsync();
    }

    private void ShowError(string message)
    {
        ErrorLabel.Text = message;
        ErrorLabel.IsVisible = true;
    }
}
