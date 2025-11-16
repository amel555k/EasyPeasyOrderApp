using EasyPeasyAPP.Models;
using EasyPeasyAPP.Services;
using Microsoft.Maui.Controls;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Input;

namespace EasyPeasyAPP.Pages
{
    public partial class OrderPage : ContentPage
    {
        private bool isDetailVisible = false;
        private double floatingStartY;
        private ArtikalModel _trenutniArtikal;
        private KategorijaModel _trenutnaKategorija;

        private IKorpaService _korpaService => (Application.Current as App)?.KorpaService;

        public List<KategorijaModel> Kategorije { get; set; }
        public ICommand OtvoriKategorijuCommand { get; set; }

        public OrderPage()
        {
            InitializeComponent();

            Kategorije = MenuData.SveKategorije;
            OtvoriKategorijuCommand = new Command<KategorijaModel>(async (kat) => await OtvoriKategoriju(kat));

            BindingContext = this;
        }

        private async Task OtvoriKategoriju(KategorijaModel kategorija)
        {
            _trenutnaKategorija = kategorija;

            await MainMenuContent.FadeTo(0, 200);
            MainMenuContent.IsVisible = false;

            KategorijaNaslov.Text = kategorija.NazivPrikaz;
            GenerisiArtikle(kategorija.Artikli);

            KategorijaContent.Opacity = 0;
            KategorijaContent.IsVisible = true;
            await KategorijaContent.FadeTo(1, 300);
        }

        private void GenerisiArtikle(List<ArtikalModel> artikli)
        {
            ArtikliGrid.Children.Clear();
            PetiArtikalContainer.Children.Clear();

            int ukupnoArtikala = artikli.Count;
            bool jeParan = ukupnoArtikala % 2 == 0;

            // Koliko artikala ide u grid (svi osim poslednjeg ako je neparan)
            int artikalauGridu = jeParan ? ukupnoArtikala : ukupnoArtikala - 1;

            // Dodaj potrebne redove u grid
            int potrebnoRedova = (int)Math.Ceiling(artikalauGridu / 2.0);
            ArtikliGrid.RowDefinitions.Clear();
            for (int i = 0; i < potrebnoRedova; i++)
            {
                ArtikliGrid.RowDefinitions.Add(new RowDefinition(GridLength.Auto));
            }

            // Popuni grid sa parnim brojem artikala
            for (int i = 0; i < artikalauGridu; i++)
            {
                var artikal = artikli[i];
                var kartica = KreirajKarticu(artikal);

                int row = i / 2;
                int col = i % 2;

                Grid.SetRow(kartica, row);
                Grid.SetColumn(kartica, col);

                ArtikliGrid.Children.Add(kartica);
            }

            // Ako je neparan broj, stavi poslednji artikal u centriran container
            if (!jeParan)
            {
                var poslednji = artikli[ukupnoArtikala - 1];
                var kartica = KreirajKarticu(poslednji);
                kartica.WidthRequest = 165;
                kartica.HorizontalOptions = LayoutOptions.Center;
                PetiArtikalContainer.Children.Add(kartica);
            }
        }

        private Grid KreirajKarticu(ArtikalModel artikal)
        {
            // Glavni grid - IDENTIČNO kao u originalu
            var mainGrid = new Grid
            {
                HorizontalOptions = LayoutOptions.Fill
            };

            // 1. SLIKA ARTIKLA - pozadina, puna visina
            var image = new Image
            {
                Source = artikal.Slika,
                Aspect = Aspect.AspectFill,
                HeightRequest = 180,
                VerticalOptions = LayoutOptions.Start
            };
            mainGrid.Children.Add(image);

            // 2. DONJI DIO - Grid sa kartica.png i nazivom (OVERLAY)
            var bottomGrid = new Grid
            {
                VerticalOptions = LayoutOptions.End,
                HeightRequest = 65
            };

            // kartica.png - overlay preko slike
            var karticaImage = new Image
            {
                Source = "kartica.png",
                Aspect = Aspect.Fill
            };
            bottomGrid.Children.Add(karticaImage);

            // Naziv artikla - preko kartica.png
            var label = new Label
            {
                Text = artikal.Naziv,
                FontFamily = "LeagueSpartan",
                FontSize = 22,
                TextColor = Color.FromArgb("#FFFFFD"),
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.Center,
                HorizontalTextAlignment = TextAlignment.Center
            };
            bottomGrid.Children.Add(label);

            mainGrid.Children.Add(bottomGrid);

            // Tap gesture
            var tapGesture = new TapGestureRecognizer();
            tapGesture.Tapped += async (s, e) => await PrikaziDetalje(artikal);
            mainGrid.GestureRecognizers.Add(tapGesture);

            return mainGrid;
        }

        private async Task PrikaziDetalje(ArtikalModel artikal)
        {
            if (isDetailVisible) return;

            _trenutniArtikal = artikal;

            FloatingItem.Source = artikal.Slika;
            ItemNameLabel.Text = artikal.Naziv;

            if (artikal.Kategorija == "sokovi" || artikal.Kategorija == "prilozi")
            {
                ItemIngredientsLabel.Text = artikal.Sastojci;
            }
            else
            {
                ItemIngredientsLabel.Text = "Sastojci: " + artikal.Sastojci;
            }

            UpdatePrice($"{artikal.Cijena}KM");

            await ShowDetailPanel();
        }

        private async Task ShowDetailPanel()
        {
            await KategorijaContent.FadeTo(0.3, 300);

            double screenHeight = this.Height;
            double panelHeight = DeviceInfo.Platform == DevicePlatform.Android ? 450 :
                                DeviceInfo.Platform == DevicePlatform.iOS ? 430 : 490;
            double panelTop = screenHeight - panelHeight;

            floatingStartY = screenHeight + 100;
            double itemHeight = 250;
            double targetY = panelTop - (itemHeight / 2);

            DetailPanel.IsVisible = true;
            await DetailPanel.TranslateTo(0, 0, 400, Easing.CubicOut);

            FloatingItem.TranslationY = floatingStartY;
            FloatingItem.IsVisible = true;
            FloatingItem.Opacity = 1;
            await FloatingItem.TranslateTo(0, targetY, 400, Easing.CubicOut);

            isDetailVisible = true;
        }

        private void UpdatePrice(string price)
        {
            ItemPriceLabel.FormattedText = new FormattedString
            {
                Spans =
                {
                    new Span { Text = "CIJENA: ", TextColor = Color.FromArgb("#FFFFFD") },
                    new Span { Text = price, TextColor = Color.FromArgb("#2E2E2C") }
                }
            };
        }

        private async Task CloseDetailPanel()
        {
            await FloatingItem.TranslateTo(0, floatingStartY, 400, Easing.CubicIn);
            FloatingItem.IsVisible = false;

            await DetailPanel.TranslateTo(0, 800, 400, Easing.CubicIn);
            DetailPanel.IsVisible = false;

            await KategorijaContent.FadeTo(1.0, 300);

            isDetailVisible = false;
        }

        private async void OnBackClicked(object sender, EventArgs e)
        {
            try
            {
                if (Shell.Current != null)
                {
                    await Shell.Current.GoToAsync("//MainPage");
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", $"Navigation failed: {ex.Message}", "OK");
            }
        }

        private async void OnKategorijaBackClicked(object sender, EventArgs e)
        {
            if (isDetailVisible)
            {
                await CloseDetailPanel();
            }
            else
            {
                await KategorijaContent.FadeTo(0, 200);
                KategorijaContent.IsVisible = false;

                MainMenuContent.Opacity = 0;
                MainMenuContent.IsVisible = true;
                await MainMenuContent.FadeTo(1, 300);
            }
        }

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