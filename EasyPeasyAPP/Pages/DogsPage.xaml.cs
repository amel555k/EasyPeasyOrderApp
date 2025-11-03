using Microsoft.Maui.Controls;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace EasyPeasyAPP.Pages
{
    public partial class DogsPage : ContentPage
    {
        private bool isDetailVisible = false;
        private double floatingStartY;

        public DogsPage()
        {
            InitializeComponent();
        }

        protected override void OnSizeAllocated(double width, double height)
        {
            base.OnSizeAllocated(width, height);
        }

        private async void OnDogsClicked(object sender, EventArgs e)
        {
            if (sender is Grid clickedGrid)
            {
                var img = clickedGrid.Children.OfType<Image>().FirstOrDefault(i => i.Source.ToString().Contains("dogs"));
                if (img != null)
                    FloatingDogs.Source = img.Source;
            }

            await MainContent.FadeTo(0.3, 300);

            double screenHeight = this.Height;
            double panelHeight = DeviceInfo.Platform == DevicePlatform.Android ? 450 :
                                DeviceInfo.Platform == DevicePlatform.iOS ? 430 : 490;
            double panelTop = screenHeight - panelHeight;
            floatingStartY = screenHeight + 100;
            double dogsHeight = 250;
            double targetY = panelTop - (dogsHeight / 2);

            DetailPanel.IsVisible = true;
            await DetailPanel.TranslateTo(0, 0, 400, Easing.CubicOut);

            FloatingDogs.TranslationY = floatingStartY;
            FloatingDogs.TranslationX = 0;
            FloatingDogs.IsVisible = true;
            FloatingDogs.Opacity = 1;

            await FloatingDogs.TranslateTo(0, targetY, 400, Easing.CubicOut);

            isDetailVisible = true;
        }

        private async void OnBackClicked(object sender, EventArgs e)
        {
            if (isDetailVisible)
            {
                await FloatingDogs.TranslateTo(0, floatingStartY, 400, Easing.CubicIn);
                FloatingDogs.IsVisible = false;

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
            DisplayAlert("Korpa", "HotDog dodan u korpu!", "OK");
        }
    }
}