using EasyPeasyAPP.Models;
using EasyPeasyAPP.Services;
using Microsoft.Maui.Controls;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace EasyPeasyAPP.Pages
{
    public partial class NarudzbeHistoryPage : ContentPage
    {
        private INarudzbaService _narudzbaService => (Application.Current as App)?.NarudzbaService;
        private IAuthService _authService => (Application.Current as App)?.AuthService;
        private bool isDetailVisible = false;

        public NarudzbeHistoryPage()
        {
            InitializeComponent();
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            await UcitajNarudzbe();
        }

        private async Task UcitajNarudzbe()
        {
            var korisnik = _authService?.CurrentUser;
            if (korisnik == null)
            {
                await DisplayAlert("Greška", "Niste prijavljeni.", "OK");
                await Shell.Current.GoToAsync("//LoginPage");
                return;
            }

            try
            {
                var narudzbe = await _narudzbaService?.DohvatiNarudzbeKorisnikaAsync(korisnik.Email);

                if (narudzbe == null || !narudzbe.Any())
                {
                    PrazneNarudzbeLabel.IsVisible = true;
                    NarudzbeCollectionView.IsVisible = false;
                }
                else
                {
                    PrazneNarudzbeLabel.IsVisible = false;
                    NarudzbeCollectionView.IsVisible = true;

                    var sortirane = narudzbe.OrderByDescending(n => n.Datum).ToList();
                    NarudzbeCollectionView.ItemsSource = null;
                    NarudzbeCollectionView.ItemsSource = sortirane;
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Greška", $"Nije moguće učitati narudžbe: {ex.Message}", "OK");
            }
        }

        private void OnNarudzbaClicked(object sender, EventArgs e)
        {
            if (isDetailVisible) return;

            if (sender is TapGestureRecognizer tap && tap.BindingContext is NarudzbaModel narudzba)
            {
                PrikaziDetalje(narudzba);
            }
            else if (sender is Border border && border.BindingContext is NarudzbaModel narudzba2)
            {
                PrikaziDetalje(narudzba2);
            }
        }

        private void PrikaziDetalje(NarudzbaModel narudzba)
        {
            DetailNarudzbaLabel.Text = $"Narudžba #{narudzba.BrojNarudzbe}";
            DetailDatumLabel.Text = narudzba.Datum.ToString("dd.MM.yyyy HH:mm");

            DetailStavkeCollectionView.ItemsSource = null;
            DetailStavkeCollectionView.ItemsSource = narudzba.Stavke;

            DetailUkupnoLabel.FormattedText = new FormattedString
            {
                Spans =
                {
                    new Span { Text = "UKUPNO: ", TextColor = Color.FromArgb("#FFFFFD") },
                    new Span { Text = $"{narudzba.Ukupno:F2} KM", TextColor = Color.FromArgb("#EFCD5E") }
                }
            };

            // Animacija detalja
            DetailPanel.IsVisible = true;
            DetailPanel.TranslateTo(0, 0, 400, Easing.CubicOut);
            isDetailVisible = true;
        }

        private async void OnCloseDetailClicked(object sender, EventArgs e)
        {
            if (!isDetailVisible) return;

            await DetailPanel.TranslateTo(500, 0, 400, Easing.CubicIn);
            DetailPanel.IsVisible = false;
            isDetailVisible = false;
        }

        private async void OnBackClicked(object sender, EventArgs e)
        {
            if (isDetailVisible)
            {
                // zatvori detalje
                await DetailPanel.TranslateTo(500, 0, 400, Easing.CubicIn);
                DetailPanel.IsVisible = false;
                isDetailVisible = false;
            }
            else
            {
                // vraća na MainPage apsolutno
                await Shell.Current.GoToAsync("//MainPage");
            }
        }
    }
}
