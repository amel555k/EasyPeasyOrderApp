using EasyPeasyAPP.Models;
using EasyPeasyAPP.Services;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Devices;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace EasyPeasyAPP.Pages
{
    public partial class PriloziPage : ContentPage
    {
        private bool isDetailVisible = false;
        private double floatingStartY;
        private ArtikalModel _trenutniArtikal;

        private IKorpaService _korpaService => (Application.Current as App)?.KorpaService;

        public PriloziPage()
        {
            InitializeComponent();
        }

        // === Klik na prilog karticu ===
        private async void OnPrilogClicked(object sender, EventArgs e)
        {
            if (isDetailVisible) return;

            if (sender is Grid clickedGrid)
            {
                var prilogImage = clickedGrid.Children.OfType<Image>()
                    .FirstOrDefault(i => !i.Source.ToString().Contains("kartica"));

                if (prilogImage != null)
                {
                    FloatingPrilog.Source = prilogImage.Source;

                    var label = clickedGrid.Children
                        .OfType<Grid>()
                        .SelectMany(g => g.Children.OfType<Label>())
                        .FirstOrDefault();

                    if (label != null)
                    {
                        PrilogNameLabel.Text = label.Text;

                        switch (label.Text)
                        {
                            case "Pomfrit":
                                _trenutniArtikal = new ArtikalModel
                                {
                                    Id = "prilog1",
                                    Naziv = label.Text,
                                    Sastojci = "Neizostavan prilog uz Vašu užinu od pomno odabranih kropmira iz našeg vrta",
                                    Cijena = 2.0,
                                    Slika = "pomfrit.png",
                                    Kategorija = "prilozi"
                                };
                                PrilogIngredientsLabel.Text = _trenutniArtikal.Sastojci;
                                UpdatePrice("2KM");
                                break;

                            case "Onion rings":
                                _trenutniArtikal = new ArtikalModel
                                {
                                    Id = "prilog2",
                                    Naziv = label.Text,
                                    Sastojci = "Ekscentričan ali i ukusan prilog, luk pohovan i detaljno oblikovan",
                                    Cijena = 2.5,
                                    Slika = "onion.png",
                                    Kategorija = "prilozi"
                                };
                                PrilogIngredientsLabel.Text = _trenutniArtikal.Sastojci;
                                UpdatePrice("2.5KM");
                                break;

                            case "Dodatni Ketchup":
                                _trenutniArtikal = new ArtikalModel
                                {
                                    Id = "prilog3",
                                    Naziv = label.Text,
                                    Sastojci = "Nestalo Vam je kečapa i želite još? Stojimo Vam na raspolaganju",
                                    Cijena = 0.5,
                                    Slika = "kecap.png",
                                    Kategorija = "prilozi"
                                };
                                PrilogIngredientsLabel.Text = _trenutniArtikal.Sastojci;
                                UpdatePrice("0.5KM");
                                break;

                            case "Dodatna majoneza":
                                _trenutniArtikal = new ArtikalModel
                                {
                                    Id = "prilog4",
                                    Naziv = label.Text,
                                    Sastojci = "Nestalo Vam je majoneze i želite još? Stojimo Vam na raspolaganju",
                                    Cijena = 0.5,
                                    Slika = "majoneza.png",
                                    Kategorija = "prilozi"
                                };
                                PrilogIngredientsLabel.Text = _trenutniArtikal.Sastojci;
                                UpdatePrice("0.5KM");
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
            double prilogHeight = 250;
            double targetY = panelTop - (prilogHeight / 2);

            DetailPanel.IsVisible = true;
            await DetailPanel.TranslateTo(0, 0, 400, Easing.CubicOut);

            FloatingPrilog.TranslationY = floatingStartY;
            FloatingPrilog.IsVisible = true;
            FloatingPrilog.Opacity = 1;

            await FloatingPrilog.TranslateTo(0, targetY, 400, Easing.CubicOut);

            isDetailVisible = true;
        }

        // === Ažuriranje cijene u detaljnom panelu ===
        private void UpdatePrice(string price)
        {
            PrilogPriceLabel.FormattedText = new FormattedString
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
            await FloatingPrilog.TranslateTo(0, floatingStartY, 400, Easing.CubicIn);
            FloatingPrilog.IsVisible = false;

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