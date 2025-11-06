using EasyPeasyAPP.Models;
using EasyPeasyAPP.Services;
using Microsoft.Maui.Controls;
using System;
using System.Linq;

namespace EasyPeasyAPP.Pages
{
    public partial class KorpaPage : ContentPage
    {
        private IKorpaService _korpaService => (Application.Current as App)?.KorpaService;
        private IAuthService _authService => (Application.Current as App)?.AuthService;
        private INarudzbaService _narudzbaService => (Application.Current as App)?.NarudzbaService;

        public KorpaPage()
        {
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            OsveziKorpu();
        }

        private void OsveziKorpu()
        {
            var stavke = _korpaService?.DohvatiKorpu();

            if (stavke == null || !stavke.Any())
            {
                PraznaKorpaLabel.IsVisible = true;
                KorpaCollectionView.IsVisible = false;
                FooterGrid.IsVisible = false;
            }
            else
            {
                PraznaKorpaLabel.IsVisible = false;
                KorpaCollectionView.IsVisible = true;
                FooterGrid.IsVisible = true;

                // Resetuj ItemsSource da bi se UI osvježio
                KorpaCollectionView.ItemsSource = null;
                KorpaCollectionView.ItemsSource = stavke;

                var ukupno = _korpaService.UkupnaCijena();
                UkupnoCijenaLabel.FormattedText = new FormattedString
                {
                    Spans =
                    {
                        new Span { Text = "UKUPNO: ", TextColor = Color.FromArgb("#FFFFFD") },
                        new Span { Text = $"{ukupno:F2} KM", TextColor = Color.FromArgb("#EFCD5E") }
                    }
                };
            }
        }

        private void OnSmanjiKolicinu(object sender, EventArgs e)
        {
            if (sender is Button button && button.CommandParameter is string artikalId)
            {
                var stavka = _korpaService?.DohvatiKorpu().FirstOrDefault(s => s.ArtikalId == artikalId);
                if (stavka != null && stavka.Kolicina > 1)
                {
                    _korpaService?.PromijeniKolicinu(artikalId, stavka.Kolicina - 1);
                    OsveziKorpu();
                }
            }
        }

        private void OnPovecajKolicinu(object sender, EventArgs e)
        {
            if (sender is Button button && button.CommandParameter is string artikalId)
            {
                var stavka = _korpaService?.DohvatiKorpu().FirstOrDefault(s => s.ArtikalId == artikalId);
                if (stavka != null)
                {
                    _korpaService?.PromijeniKolicinu(artikalId, stavka.Kolicina + 1);
                    OsveziKorpu();
                }
            }
        }

        private void OnUkloniStavku(object sender, EventArgs e)
        {
            if (sender is Button button && button.CommandParameter is string artikalId)
            {
                _korpaService?.UkloniIzKorpe(artikalId);
                OsveziKorpu();
            }
        }

        private async void OnPotvrdiNarudzbu(object sender, EventArgs e)
        {
            var stavke = _korpaService?.DohvatiKorpu();
            if (stavke == null || !stavke.Any())
            {
                await DisplayAlert("Greška", "Korpa je prazna.", "OK");
                return;
            }

            var korisnik = _authService?.CurrentUser;
            if (korisnik == null)
            {
                await DisplayAlert("Greška", "Niste prijavljeni.", "OK");
                return;
            }

            try
            {
                var narudzba = await _narudzbaService.KreirajNarudzbuAsync(korisnik, stavke);

                _korpaService?.OcistiKorpu();

                await DisplayAlert("Uspjeh",
                    $"Narudžba #{narudzba.Id.Substring(0, 8)} je uspješno kreirana!\nUkupno: {narudzba.Ukupno:F2} KM",
                    "OK");

                await Shell.Current.GoToAsync("//MainPage");
            }
            catch (Exception ex)
            {
                await DisplayAlert("Greška", $"Nije moguće kreirati narudžbu: {ex.Message}", "OK");
            }
        }

        private async void OnBackClicked(object sender, EventArgs e)
        {
            if (Navigation.NavigationStack.Count > 1)
            {
                await Navigation.PopAsync();
            }
            else
            {
                await Shell.Current.GoToAsync("//MainPage");
            }
        }
    }
}