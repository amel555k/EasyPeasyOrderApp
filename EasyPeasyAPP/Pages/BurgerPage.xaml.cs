using EasyPeasyAPP.Models;
using EasyPeasyAPP.Services;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Devices;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace EasyPeasyAPP.Pages
{
    public partial class BurgerPage : ContentPage
    {
        private bool isDetailVisible = false;
        private double floatingStartY;
        private ArtikalModel _trenutniArtikal;

        private IKorpaService _korpaService => (Application.Current as App)?.KorpaService;

        public BurgerPage()
        {
            InitializeComponent();
        }

        // === Klik na burger karticu ===
        private async void OnBurgerClicked(object sender, EventArgs e)
        {
            if (isDetailVisible) return;

            if (sender is Grid clickedGrid)
            {
                var burgerImage = clickedGrid.Children.OfType<Image>()
                    .FirstOrDefault(i => !i.Source.ToString().Contains("kartica"));

                if (burgerImage != null)
                {
                    FloatingBurger.Source = burgerImage.Source;

                    var label = clickedGrid.Children
                        .OfType<Grid>()
                        .SelectMany(g => g.Children.OfType<Label>())
                        .FirstOrDefault();

                    if (label != null)
                    {
                        BurgerNameLabel.Text = label.Text;

                        switch (label.Text)
                        {
                            case "Easy Crunchy Burger":
                                _trenutniArtikal = new ArtikalModel
                                {
                                    Id = "burger1",
                                    Naziv = label.Text,
                                    Sastojci = "Pljeskavica, zelena salata, Easy Peasy umak od luka, hrskavi luk",
                                    Cijena = 7.0,
                                    Slika = "burger1.png",
                                    Kategorija = "burgeri"
                                };
                                BurgerIngredientsLabel.Text = "Sastojci: " + _trenutniArtikal.Sastojci;
                                UpdatePrice("7KM");
                                break;

                            case "Easy Classic Burger":
                                _trenutniArtikal = new ArtikalModel
                                {
                                    Id = "burger2",
                                    Naziv = label.Text,
                                    Sastojci = "Pljeskavica, zelena salata, paradajz, ketchup, majoneza",
                                    Cijena = 6.5,
                                    Slika = "burger2.png",
                                    Kategorija = "burgeri"
                                };
                                BurgerIngredientsLabel.Text = "Sastojci: " + _trenutniArtikal.Sastojci;
                                UpdatePrice("6.5KM");
                                break;

                            case "Easy Cheese Burger":
                                _trenutniArtikal = new ArtikalModel
                                {
                                    Id = "burger3",
                                    Naziv = label.Text,
                                    Sastojci = "Pljeskavica, cheddar sir, zelena salata, paradajz, majoneza",
                                    Cijena = 7.5,
                                    Slika = "burger3.png",
                                    Kategorija = "burgeri"
                                };
                                BurgerIngredientsLabel.Text = "Sastojci: " + _trenutniArtikal.Sastojci;
                                UpdatePrice("7.5KM");
                                break;

                            case "Easy BBQ Burger":
                                _trenutniArtikal = new ArtikalModel
                                {
                                    Id = "burger4",
                                    Naziv = label.Text,
                                    Sastojci = "Pljeskavica, bacon, BBQ sos, cheddar sir, hrskavi luk",
                                    Cijena = 8.5,
                                    Slika = "burger4.png",
                                    Kategorija = "burgeri"
                                };
                                BurgerIngredientsLabel.Text = "Sastojci: " + _trenutniArtikal.Sastojci;
                                UpdatePrice("8.5KM");
                                break;

                            case "Easy Spicy Burger":
                                _trenutniArtikal = new ArtikalModel
                                {
                                    Id = "burger5",
                                    Naziv = label.Text,
                                    Sastojci = "Pljeskavica, ljuti sos, jalapeno papričice, cheddar sir, luk",
                                    Cijena = 8.0,
                                    Slika = "burger5.png",
                                    Kategorija = "burgeri"
                                };
                                BurgerIngredientsLabel.Text = "Sastojci: " + _trenutniArtikal.Sastojci;
                                UpdatePrice("8KM");
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
            double burgerHeight = 250;
            double targetY = panelTop - (burgerHeight / 2);

            DetailPanel.IsVisible = true;
            await DetailPanel.TranslateTo(0, 0, 400, Easing.CubicOut);

            FloatingBurger.TranslationY = floatingStartY;
            FloatingBurger.IsVisible = true;
            FloatingBurger.Opacity = 1;

            await FloatingBurger.TranslateTo(0, targetY, 400, Easing.CubicOut);

            isDetailVisible = true;
        }

        // === Ažuriranje cijene u detaljnom panelu ===
        private void UpdatePrice(string price)
        {
            BurgerPriceLabel.FormattedText = new FormattedString
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
            await FloatingBurger.TranslateTo(0, floatingStartY, 400, Easing.CubicIn);
            FloatingBurger.IsVisible = false;

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