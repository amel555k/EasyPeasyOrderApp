using EasyPeasyAPP.Pages;

namespace EasyPeasyAPP;

public partial class AppShell : Shell
{
    public AppShell()
    {
        InitializeComponent();

        // Registracija ruta
        Routing.RegisterRoute("MainPage", typeof(MainPage));
        Routing.RegisterRoute("OrderPage", typeof(OrderPage));
        Routing.RegisterRoute("AboutPage", typeof(AboutPage));
        Routing.RegisterRoute("ProfilePage", typeof(ProfilePage)); // dodano
    }
}
