using EasyPeasyAPP.Models;
using EasyPeasyAPP.Services;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Devices;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace EasyPeasyAPP.Pages
{
    public partial class SokoviPage : ContentPage
    {
        private bool isDetailVisible = false;
        private double floatingStartY;
        private ArtikalModel _trenutniArtikal;

        private IKorpaService _korpaService => (Application.Current as App)?.KorpaService;

        public SokoviPage()
        {
            InitializeComponent();
        }

        // === Klik na sok karticu ===
        private async void OnSokClicked(object sender, EventArgs e)
        {
            if (isDetailVisible) return;

            if (sender is Grid clickedGrid)
            {
                var sokImage = clickedGrid.Children.OfType<Image>()
                    .FirstOrDefault(i => !i.Source.ToString().Contains("kartica"));

                if (sokImage != null)
                {
                    FloatingSok.Source = sokImage.Source;

                    var label = clickedGrid.Children
                        .OfType<Grid>()
                        .SelectMany(g => g.Children.OfType<Label>())
                        .FirstOrDefault();

                    if (label != null)
                    {
                        SokNameLabel.Text = label.Text;

                        switch (label.Text)
                        {
                            case "Coca Cola":
                                _trenutniArtikal = new ArtikalModel
                                {
                                    Id = "sok1",
                                    Naziv = label.Text,
                                    Sastojci = "Osvježenje koje je neminovno uz Easy Peasy produkte",
                                    Cijena = 2.5,
                                    Slika = "sok1.png",
                                    Kategorija = "sokovi"
                                };
                                SokIngredientsLabel.Text = _trenutniArtikal.Sastojci;
                                UpdatePrice("2.5KM");
                                break;

                            case "Sprite":
                                _trenutniArtikal = new ArtikalModel
                                {
                                    Id = "sok2",
                                    Naziv = label.Text,
                                    Sastojci = "Limunada koja osvježava",
                                    Cijena = 2.5,
                                    Slika = "sok2.png",
                                    Kategorija = "sokovi"
                                };
                                SokIngredientsLabel.Text = _trenutniArtikal.Sastojci;
                                UpdatePrice("2.5KM");
                                break;

                            case "Fanta":
                                _trenutniArtikal = new ArtikalModel
                                {
                                    Id = "sok3",
                                    Naziv = label.Text,
                                    Sastojci = "Narandžasti osvježavajući sok",
                                    Cijena = 2.5,
                                    Slika = "sok3.png",
                                    Kategorija = "sokovi"
                                };
                                SokIngredientsLabel.Text = _trenutniArtikal.Sastojci;
                                UpdatePrice("2.5KM");
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
            double sokHeight = 250;
            double targetY = panelTop - (sokHeight / 2);

            DetailPanel.IsVisible = true;
            await DetailPanel.TranslateTo(0, 0, 400, Easing.CubicOut);

            FloatingSok.TranslationY = floatingStartY;
            FloatingSok.IsVisible = true;
            FloatingSok.Opacity = 1;

            await FloatingSok.TranslateTo(0, targetY, 400, Easing.CubicOut);

            isDetailVisible = true;
        }

        // === Ažuriranje cijene u detaljnom panelu ===
        private void UpdatePrice(string price)
        {
            SokPriceLabel.FormattedText = new FormattedString
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
            await FloatingSok.TranslateTo(0, floatingStartY, 400, Easing.CubicIn);
            FloatingSok.IsVisible = false;

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