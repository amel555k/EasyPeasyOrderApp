using EasyPeasyAPP.Pages;

namespace EasyPeasyAPP
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
            MainPage = new SplashPage();
        }
    }
}