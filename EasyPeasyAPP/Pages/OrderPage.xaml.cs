using EasyPeasyAPP.Pages.Auth;

namespace EasyPeasyAPP.Pages
{
    public partial class OrderPage : ContentPage
    {
        public OrderPage()
        {
            InitializeComponent();
        }

        private async void OnBackClicked(object sender, EventArgs e)
        {
            Application.Current.MainPage = new MainPage();
        }

        private async void OnBurgeriClicked(object sender, EventArgs e)
        {
            Application.Current!.MainPage = new NavigationPage(new BurgerPage());
        }

        private async void OnDogsClicked(object sender, EventArgs e)
        {
            // TODO: Navigate to dogs list
            await DisplayAlert("Easy Dogs", "Otvara se lista hot dogova...", "OK");
        }

        private async void OnSendviciClicked(object sender, EventArgs e)
        {
            // TODO: Navigate to sendviči list
            await DisplayAlert("Sendviči", "Otvara se lista sendviča...", "OK");
        }

        private async void OnSokoviClicked(object sender, EventArgs e)
        {
            // TODO: Navigate to sokovi list
            await DisplayAlert("Sokovi", "Otvara se lista sokova...", "OK");
        }

        private async void OnPriloziClicked(object sender, EventArgs e)
        {
            // TODO: Navigate to prilozi list
            await DisplayAlert("Prilozi", "Otvara se lista priloga...", "OK");
        }
    }
}