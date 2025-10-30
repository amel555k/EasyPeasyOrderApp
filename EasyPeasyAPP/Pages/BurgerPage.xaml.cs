using Microsoft.Maui.Controls;
using System;
using System.Linq;

namespace EasyPeasyAPP.Pages
{
    public partial class BurgerPage : ContentPage
    {
        private bool isDetailVisible = false;
        private double floatingStartY;

        public BurgerPage()
        {
            InitializeComponent();
        }

        private async void OnBurgerClicked(object sender, EventArgs e)
        {
            if (sender is Grid clickedGrid)
            {
                var img = clickedGrid.Children.OfType<Image>().FirstOrDefault(i => i.Source.ToString().Contains("burger"));
                if (img != null)
                    FloatingBurger.Source = img.Source;
            }

            // Zatamni MainContent
            await MainContent.FadeTo(0.3, 300);

            // Prikaži detail panel
            DetailPanel.IsVisible = true;
            await DetailPanel.TranslateTo(0, 0, 400, Easing.CubicOut);

            // Floating burger animacija
            floatingStartY = DetailPanel.Y + DetailPanel.Height;
            FloatingBurger.TranslationY = floatingStartY;
            FloatingBurger.IsVisible = true;
            FloatingBurger.Opacity = 1;

            // Ciljna pozicija: iznad panela
            double targetY = DetailPanel.Y - FloatingBurger.Height / 2;
            await FloatingBurger.TranslateTo(FloatingBurger.TranslationX, targetY, 400, Easing.CubicOut);

            isDetailVisible = true;
        }

        private async void OnBackClicked(object sender, EventArgs e)
        {
            if (isDetailVisible)
            {
                // Reverzni FloatingBurger
                await FloatingBurger.TranslateTo(FloatingBurger.TranslationX, floatingStartY, 400, Easing.CubicIn);
                FloatingBurger.IsVisible = false;

                // Zatvori detail panel
                await DetailPanel.TranslateTo(0, 800, 400, Easing.CubicIn);
                DetailPanel.IsVisible = false;

                // Vrati opacity MainContent
                await MainContent.FadeTo(1.0, 300);

                isDetailVisible = false;
            }
            else
            {
                // Navigacija na OrderPage
                Application.Current!.MainPage = new NavigationPage(new OrderPage());
            }
        }
    }
}

