using Microsoft.Maui.Controls;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace EasyPeasyAPP.Pages
{
    public partial class SokoviPage : ContentPage
    {
        private bool isDetailVisible = false;
        private double floatingStartY;

        public SokoviPage()
        {
            InitializeComponent();
        }

        protected override void OnSizeAllocated(double width, double height)
        {
            base.OnSizeAllocated(width, height);
        }

        private async void OnSokClicked(object sender, EventArgs e)
        {
            if (sender is Grid clickedGrid)
            {
                // Pronađi prvu sliku koja NIJE kartica.png (to je slika soka)
                var sokImage = clickedGrid.Children.OfType<Image>()
                    .FirstOrDefault(i => !i.Source.ToString().Contains("kartica"));

                if (sokImage != null)
                {
                    // Postavi istu sliku na FloatingSok
                    FloatingSok.Source = sokImage.Source;

                    // Opcionalno: ažuriraj naziv soka ako želiš
                    var label = clickedGrid.Children
                        .OfType<Grid>()
                        .SelectMany(g => g.Children.OfType<Label>())
                        .FirstOrDefault();

                    if (label != null)
                    {
                        SokNameLabel.Text = label.Text;
                    }
                }
            }

            await MainContent.FadeTo(0.3, 300);

            double screenHeight = this.Height;
            double panelHeight = DeviceInfo.Platform == DevicePlatform.Android ? 450 :
                                DeviceInfo.Platform == DevicePlatform.iOS ? 430 : 490;
            double panelTop = screenHeight - panelHeight;

            floatingStartY = screenHeight + 100;
            double burgerHeight = 250;
            double targetY = panelTop - (burgerHeight / 2);

            DetailPanel.IsVisible = true;
            await DetailPanel.TranslateTo(0, 0, 400, Easing.CubicOut);

            FloatingSok.TranslationY = floatingStartY;
            FloatingSok.TranslationX = 0;
            FloatingSok.IsVisible = true;
            FloatingSok.Opacity = 1;

            await FloatingSok.TranslateTo(0, targetY, 400, Easing.CubicOut);

            isDetailVisible = true;
        }

        private async void OnBackClicked(object sender, EventArgs e)
        {
            if (isDetailVisible)
            {
                await FloatingSok.TranslateTo(0, floatingStartY, 400, Easing.CubicIn);
                FloatingSok.IsVisible = false;

                await DetailPanel.TranslateTo(0, 800, 400, Easing.CubicIn);
                DetailPanel.IsVisible = false;

                await MainContent.FadeTo(1.0, 300);

                isDetailVisible = false;
            }
            else
            {
                try
                {
                    if (Shell.Current != null)
                    {
                        await Shell.Current.GoToAsync("//OrderPage");
                    }
                }
                catch (Exception ex)
                {
                    await DisplayAlert("Error", $"Navigation failed: {ex.Message}", "OK");
                }
            }
        }

        private void OnAddToCartClicked(object sender, EventArgs e)
        {
            DisplayAlert("Korpa", "Sok dodan u korpu!", "OK");
        }
    }
}