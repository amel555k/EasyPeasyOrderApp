namespace EasyPeasyAPP.Pages
{
    public partial class AboutPage : ContentPage
    {
        public AboutPage()
        {
            InitializeComponent();
        }

        private void OnBackClicked(object sender, EventArgs e)
        {
            // Jednostavno vrati korisnika na MainPage
            Application.Current.MainPage = new MainPage();
        }
    }
}
