using EasyPeasyAPP.Models;
using EasyPeasyAPP.Services;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Devices;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace EasyPeasyAPP.Pages
{
    public partial class SendviciPage : ContentPage
    {
        private bool isDetailVisible = false;
        private double floatingStartY;
        private ArtikalModel _trenutniArtikal;


    private IKorpaService _korpaService => (Application.Current as App)?.KorpaService;

        public SendviciPage()
        {
            InitializeComponent();
        }

        // === Klik na sendvič karticu ===
        private async void OnSendvicClicked(object sender, EventArgs e)
        {
            if (isDetailVisible) return;

            if (sender is Grid clickedGrid)
            {
                var sendvicImage = clickedGrid.Children.OfType<Image>()
                    .FirstOrDefault(i => !i.Source.ToString().Contains("kartica"));

                if (sendvicImage != null)
                {
                    FloatingSendvic.Source = sendvicImage.Source;

                    var label = clickedGrid.Children
                        .OfType<Grid>()
                        .SelectMany(g => g.Children.OfType<Label>())
                        .FirstOrDefault();

                    if (label != null)
                    {
                        SendvicNameLabel.Text = label.Text;

                        switch (label.Text)
                        {
                            case "Easy Peasy školski sendvič":
                                _trenutniArtikal = new ArtikalModel
                                {
                                    Id = "sendvic1",
                                    Naziv = label.Text,
                                    Sastojci = "Pureća premium šunka, majoneza, ketchup, krastavice, sos",
                                    Cijena = 4.0,
                                    Slika = "sendvic1.png",
                                    Kategorija = "sendvici"
                                };
                                SendvicIngredientsLabel.Text = "Sastojci: " + _trenutniArtikal.Sastojci;
                                UpdatePrice("4KM");
                                break;

                            case "Easy Peasy chicken sendvič":
                                _trenutniArtikal = new ArtikalModel
                                {
                                    Id = "sendvic2",
                                    Naziv = label.Text,
                                    Sastojci = "Piletina, zelena salata, paradajz, majoneza",
                                    Cijena = 4.5,
                                    Slika = "sendvic2.png",
                                    Kategorija = "sendvici"
                                };
                                SendvicIngredientsLabel.Text = "Sastojci: " + _trenutniArtikal.Sastojci;
                                UpdatePrice("4.5KM");
                                break;

                            case "Easy Peasy chilli sendvič":
                                _trenutniArtikal = new ArtikalModel
                                {
                                    Id = "sendvic3",
                                    Naziv = label.Text,
                                    Sastojci = "Piletina, ljuti sos, paprika, sir, majoneza",
                                    Cijena = 5.0,
                                    Slika = "sendvic3.png",
                                    Kategorija = "sendvici"
                                };
                                SendvicIngredientsLabel.Text = "Sastojci: " + _trenutniArtikal.Sastojci;
                                UpdatePrice("5KM");
                                break;

                            default:
                                _trenutniArtikal = null;
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
            double sendvicHeight = 250;
            double targetY = panelTop - (sendvicHeight / 2);

            DetailPanel.IsVisible = true;
            await DetailPanel.TranslateTo(0, 0, 400, Easing.CubicOut);

            FloatingSendvic.TranslationY = floatingStartY;
            FloatingSendvic.IsVisible = true;
            FloatingSendvic.Opacity = 1;

            await FloatingSendvic.TranslateTo(0, targetY, 400, Easing.CubicOut);

            isDetailVisible = true;
        }

        // === Ažuriranje cijene u detaljnom panelu ===
        private void UpdatePrice(string price)
        {
            SendvicPriceLabel.FormattedText = new FormattedString
            {
                Spans =
        {
            new Span { Text = "CIJENA: ", TextColor = Color.FromArgb("#FFFFFD") },
            new Span { Text = price, TextColor = Color.FromArgb("#2E2E2C") }
        }
            };
        }

        // === Klik na Back dugme ===
        private async void OnBackClicked(object sender, EventArgs e)
        {
            if (isDetailVisible)
            {
                await CloseDetailPanel();
            }
            else
            {
                await NavigateBackToOrderPage();
            }
        }

        // === Zatvaranje detaljnog panela ===
        private async Task CloseDetailPanel()
        {
            await FloatingSendvic.TranslateTo(0, floatingStartY, 400, Easing.CubicIn);
            FloatingSendvic.IsVisible = false;

            await DetailPanel.TranslateTo(0, 800, 400, Easing.CubicIn);
            DetailPanel.IsVisible = false;

            await MainContent.FadeTo(1.0, 300);
            isDetailVisible = false;
        }

        // === Navigacija nazad na OrderPage ===
        private async Task NavigateBackToOrderPage()
        {
            try
            {
                // Singleton servis čuva stanje korpe
                await Shell.Current.GoToAsync("//OrderPage");
            }
            catch (Exception ex)
            {
                await DisplayAlert("Greška", $"Neuspješna navigacija: {ex.Message}", "OK");
            }
        }

        // === Klik na "Dodaj u korpu" ===
        private async void OnAddToCartClicked(object sender, EventArgs e)
        {
            if (_trenutniArtikal == null)
            {
                await DisplayAlert("Greška", "Artikal nije pronađen.", "OK");
                return;
            }

            _korpaService?.DodajUKorpu(_trenutniArtikal);
            await DisplayAlert("Uspjeh", $"{_trenutniArtikal.Naziv} dodan u korpu!", "OK");

            if (isDetailVisible)
                await CloseDetailPanel();
        }

        // === Klik na "Korpa" button ===
        private async void OnKorpaClicked(object sender, EventArgs e)
        {
            try
            {
                await Shell.Current.GoToAsync("KorpaPage");
            }
            catch (Exception ex)
            {
                await DisplayAlert("Greška", $"Neuspješna navigacija: {ex.Message}", "OK");
            }
        }
    }

}
