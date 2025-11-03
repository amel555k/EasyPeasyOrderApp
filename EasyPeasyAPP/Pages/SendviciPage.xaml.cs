using Microsoft.Maui.Controls;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace EasyPeasyAPP.Pages
{
    public partial class SendviciPage : ContentPage
    {
        private bool isDetailVisible = false;
        private double floatingStartY;

        public SendviciPage()
        {
            InitializeComponent();
        }

        protected override void OnSizeAllocated(double width, double height)
        {
            base.OnSizeAllocated(width, height);
        }

        private async void OnSendvicClicked(object sender, EventArgs e)
        {
            // Blokiraj klik ako je detail panel već otvoren
            if (isDetailVisible)
                return;

            if (sender is Grid clickedGrid)
            {
                // Pronađi prvu sliku koja NIJE kartica.png (to je slika sendviča)
                var sendvicImage = clickedGrid.Children.OfType<Image>()
                    .FirstOrDefault(i => !i.Source.ToString().Contains("kartica"));

                if (sendvicImage != null)
                {
                    // Postavi istu sliku na FloatingSendvic
                    FloatingSendvic.Source = sendvicImage.Source;

                    // Ažuriraj naziv sendviča
                    var label = clickedGrid.Children
                        .OfType<Grid>()
                        .SelectMany(g => g.Children.OfType<Label>())
                        .FirstOrDefault();

                    if (label != null)
                    {
                        SendvicNameLabel.Text = label.Text;

                        // Postavi odgovarajući tekst sastojaka i cijenu na osnovu naziva
                        switch (label.Text)
                        {
                            case "Easy Peasy školski sendvič":
                                SendvicIngredientsLabel.Text = "Sastojci: Pureća premium šunka, majoneza, kechup, krastavice, sos";
                                UpdatePrice("4KM");
                                break;

                            case "Easy Peasy chicken sendvič":
                                SendvicIngredientsLabel.Text = "Sastojci: Hrskavo pržena piletina, BBQ sos, svježa salata, paradajz, crveni luk";
                                UpdatePrice("5KM");
                                break;

                            case "Easy Peasy chilli sendvič":
                                SendvicIngredientsLabel.Text = "Sastojci: Pikantna ljuta kobasica, chilli sos, cheddar sir, jalapeño papričice, kajmak";
                                UpdatePrice("5KM");
                                break;

                            default:
                                SendvicIngredientsLabel.Text = "Sastojci: Premium sastojci";
                                UpdatePrice("4KM");
                                break;
                        }
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

            FloatingSendvic.TranslationY = floatingStartY;
            FloatingSendvic.TranslationX = 0;
            FloatingSendvic.IsVisible = true;
            FloatingSendvic.Opacity = 1;

            await FloatingSendvic.TranslateTo(0, targetY, 400, Easing.CubicOut);

            isDetailVisible = true;
        }

        private void UpdatePrice(string price)
        {
            SendvicPriceLabel.FormattedText = new FormattedString
            {
                Spans =
                {
                    new Span
                    {
                        Text = "CIJENA: ",
                        TextColor = Color.FromArgb("#FFFFFD")
                    },
                    new Span
                    {
                        Text = price,
                        TextColor = Color.FromArgb("#2E2E2C")
                    }
                }
            };
        }

        private async void OnBackClicked(object sender, EventArgs e)
        {
            if (isDetailVisible)
            {
                await FloatingSendvic.TranslateTo(0, floatingStartY, 400, Easing.CubicIn);
                FloatingSendvic.IsVisible = false;

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
            DisplayAlert("Korpa", "Sendvič dodan u korpu!", "OK");
        }
    }
}