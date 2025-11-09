using EasyPeasyAPP.Models;
using EasyPeasyAPP.Services;
using Microsoft.Maui.Controls;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace EasyPeasyAPP.Pages
{
    public partial class KorpaPage : ContentPage
    {
        private IKorpaService _korpaService => (Application.Current as App)?.KorpaService;
        private IAuthService _authService => (Application.Current as App)?.AuthService;
        private INarudzbaService _narudzbaService => (Application.Current as App)?.NarudzbaService;

        private bool _dostavaChecked = false;

        public KorpaPage()
        {
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            OsveziKorpu();
            ResetujFormu();
        }

        private void OsveziKorpu()
        {
            var stavke = _korpaService?.DohvatiKorpu();

            if (stavke == null || !stavke.Any())
            {
                PraznaKorpaLabel.IsVisible = true;
                KorpaStavkeContainer.IsVisible = false;
                FooterGrid.IsVisible = false;
            }
            else
            {
                PraznaKorpaLabel.IsVisible = false;
                KorpaStavkeContainer.IsVisible = true;
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

        private void ResetujFormu()
        {
            // Vrati na prvi korak
            KorpaStavkeContainer.IsVisible = true;
            DostavaFormaContainer.IsVisible = false;
            DaljeButton.IsVisible = true;
            PotvrdiButton.IsVisible = false;
            SuccessOverlay.IsVisible = false;

            // Resetuj polja
            _dostavaChecked = false;
            CheckImage.IsVisible = false;
            AdresaEditor.Text = string.Empty;
            AdresaEditor.IsEnabled = false;
            NapomenaEditor.Text = string.Empty;
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

        private void OnCheckboxTapped(object sender, EventArgs e)
        {
            _dostavaChecked = !_dostavaChecked;
            CheckImage.IsVisible = _dostavaChecked;
            AdresaEditor.IsEnabled = _dostavaChecked;

            if (!_dostavaChecked)
            {
                AdresaEditor.Text = string.Empty;
            }
        }

        private void OnDaljeClicked(object sender, EventArgs e)
        {
            // Sakrij stavke i prikaži formu za dostavu
            KorpaStavkeContainer.IsVisible = false;
            DostavaFormaContainer.IsVisible = true;
            DaljeButton.IsVisible = false;
            PotvrdiButton.IsVisible = true;
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

            // Provjeri da li je dostava potrebna i da li je adresa unesena
            if (_dostavaChecked && string.IsNullOrWhiteSpace(AdresaEditor.Text))
            {
                await DisplayAlert("Greška", "Molimo unesite adresu dostave.", "OK");
                return;
            }

            try
            {
                var narudzba = await _narudzbaService.KreirajNarudzbuAsync(
                    korisnik,
                    stavke,
                    _dostavaChecked ? AdresaEditor.Text?.Trim() : null,
                    NapomenaEditor.Text?.Trim()
                );

                _korpaService?.OcistiKorpu();

                // Prikaži success screen
                await PrikaziSuccessScreen();
            }
            catch (Exception ex)
            {
                await DisplayAlert("Greška", $"Nije moguće kreirati narudžbu: {ex.Message}", "OK");
            }
        }

        private async Task PrikaziSuccessScreen()
        {
            // Sakrij sve osim success overlay-a
            HeaderGrid.IsVisible = false;
            MainScrollView.IsVisible = false;
            FooterGrid.IsVisible = false;

            // Prikaži success screen
            SuccessOverlay.IsVisible = true;

            // Čekaj 3 sekunde
            await Task.Delay(3000);

            // Navigiraj na MainPage
            await Shell.Current.GoToAsync("//MainPage");
        }

        private async void OnBackClicked(object sender, EventArgs e)
        {
            // Ako smo na drugom koraku, vrati se na prvi
            if (DostavaFormaContainer.IsVisible)
            {
                DostavaFormaContainer.IsVisible = false;
                KorpaStavkeContainer.IsVisible = true;
                PotvrdiButton.IsVisible = false;
                DaljeButton.IsVisible = true;
                return;
            }

            // Inače izađi iz stranice
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