using EasyPeasyAPP.Models;
using EasyPeasyAPP.Services;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Devices;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace EasyPeasyAPP.Pages
{
    public partial class DogsPage : ContentPage
    {
        private bool isDetailVisible = false;
        private double floatingStartY;
        private ArtikalModel _trenutniArtikal;

        private IKorpaService _korpaService => (Application.Current as App)?.KorpaService;

        public DogsPage()
        {
            InitializeComponent();
        }

        // === Klik na hot dog karticu ===
        private async void OnDogsClicked(object sender, EventArgs e)
        {
            if (isDetailVisible) return;

            if (sender is Grid clickedGrid)
            {
                var dogImage = clickedGrid.Children.OfType<Image>()
                    .FirstOrDefault(i => !i.Source.ToString().Contains("kartica"));

                if (dogImage != null)
                {
                    FloatingDogs.Source = dogImage.Source;

                    var label = clickedGrid.Children
                        .OfType<Grid>()
                        .SelectMany(g => g.Children.OfType<Label>())
                        .FirstOrDefault();

                    if (label != null)
                    {
                        DogsNameLabel.Text = label.Text;

                        switch (label.Text)
                        {
                            case "Easy Peasy Chilli Dog":
                                _trenutniArtikal = new ArtikalModel
                                {
                                    Id = "dog1",
                                    Naziv = label.Text,
                                    Sastojci = "Handmade hrenovka, Easy Peasy sos od chilli-a, senf, kupus",
                                    Cijena = 6.5,
                                    Slika = "hotdog1.png",
                                    Kategorija = "hotdogs"
                                };
                                DogsIngredientsLabel.Text = "Sastojci: " + _trenutniArtikal.Sastojci;
                                UpdatePrice("6.5KM");
                                break;

                            case "Easy Peasy Classic Dog":
                                _trenutniArtikal = new ArtikalModel
                                {
                                    Id = "dog2",
                                    Naziv = label.Text,
                                    Sastojci = "Hrenovka, senf, ketchup, kupus, luk",
                                    Cijena = 5.5,
                                    Slika = "hotdog2.png",
                                    Kategorija = "hotdogs"
                                };
                                DogsIngredientsLabel.Text = "Sastojci: " + _trenutniArtikal.Sastojci;
                                UpdatePrice("5.5KM");
                                break;

                            case "Easy Peasy Cheese Dog":
                                _trenutniArtikal = new ArtikalModel
                                {
                                    Id = "dog3",
                                    Naziv = label.Text,
                                    Sastojci = "Hrenovka, cheddar sir, senf, majoneza, luk",
                                    Cijena = 6.0,
                                    Slika = "hotdog3.png",
                                    Kategorija = "hotdogs"
                                };
                                DogsIngredientsLabel.Text = "Sastojci: " + _trenutniArtikal.Sastojci;
                                UpdatePrice("6KM");
                                break;

                            case "Easy Peasy BBQ Dog":
                                _trenutniArtikal = new ArtikalModel
                                {
                                    Id = "dog4",
                                    Naziv = label.Text,
                                    Sastojci = "Hrenovka, BBQ sos, bacon, luk, kupus",
                                    Cijena = 7.0,
                                    Slika = "hotdog4.png",
                                    Kategorija = "hotdogs"
                                };
                                DogsIngredientsLabel.Text = "Sastojci: " + _trenutniArtikal.Sastojci;
                                UpdatePrice("7KM");
                                break;

                            case "Easy Peasy Vege Dog":
                                _trenutniArtikal = new ArtikalModel
                                {
                                    Id = "dog5",
                                    Naziv = label.Text,
                                    Sastojci = "Vegetarijanska kobasica, senf, ketchup, svježe povrće",
                                    Cijena = 6.0,
                                    Slika = "hotdog5.png",
                                    Kategorija = "hotdogs"
                                };
                                DogsIngredientsLabel.Text = "Sastojci: " + _trenutniArtikal.Sastojci;
                                UpdatePrice("6KM");
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
            double dogsHeight = 250;
            double targetY = panelTop - (dogsHeight / 2);

            DetailPanel.IsVisible = true;
            await DetailPanel.TranslateTo(0, 0, 400, Easing.CubicOut);

            FloatingDogs.TranslationY = floatingStartY;
            FloatingDogs.IsVisible = true;
            FloatingDogs.Opacity = 1;

            await FloatingDogs.TranslateTo(0, targetY, 400, Easing.CubicOut);

            isDetailVisible = true;
        }

        // === Ažuriranje cijene u detaljnom panelu ===
        private void UpdatePrice(string price)
        {
            DogsPriceLabel.FormattedText = new FormattedString
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
            await FloatingDogs.TranslateTo(0, floatingStartY, 400, Easing.CubicIn);
            FloatingDogs.IsVisible = false;

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