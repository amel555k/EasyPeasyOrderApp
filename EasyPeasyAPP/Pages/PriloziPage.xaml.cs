using Microsoft.Maui.Controls;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace EasyPeasyAPP.Pages
{
    public partial class PriloziPage : ContentPage
    {
        private bool isDetailVisible = false;
        private double floatingStartY;

        public PriloziPage()
        {
            InitializeComponent();
        }

        protected override void OnSizeAllocated(double width, double height)
        {
            base.OnSizeAllocated(width, height);
        }

        private async void OnPrilogClicked(object sender, EventArgs e)
        {
            // Blokiraj klik ako je detail panel već otvoren
            if (isDetailVisible)
                return;

            if (sender is Grid clickedGrid)
            {
                // Pronađi prvu sliku koja NIJE kartica.png (to je slika priloga)
                var prilogImage = clickedGrid.Children.OfType<Image>()
                    .FirstOrDefault(i => !i.Source.ToString().Contains("kartica"));

                if (prilogImage != null)
                {
                    // Postavi istu sliku na FloatingPrilog
                    FloatingPrilog.Source = prilogImage.Source;

                    // Ažuriraj naziv priloga
                    var label = clickedGrid.Children
                        .OfType<Grid>()
                        .SelectMany(g => g.Children.OfType<Label>())
                        .FirstOrDefault();

                    if (label != null)
                    {
                        PrilogNameLabel.Text = label.Text;

                        // Postavi odgovarajući tekst opisa na osnovu naziva
                        string opisTekst = label.Text switch
                        {
                            "Pomfrit" => "Neizostavan prilog uz Vašu užinu od pomno odabranih kropmira iz našeg vrta",
                            "Onion rings" => "Ekscentričan ali i ukusan prilog, luk pohovan i detaljno oblikovan",
                            "Dodatni Ketchup" => "Nestalo Vam je kečapa i želite još? Stojimo Vam na raspolaganju",
                            "Dodatna majoneza" => "Nestalo Vam je majoneze i želite još? Stojimo Vam na raspolaganju",
                            _ => "Savršen prilog uz Vaš obrok"
                        };

                        PrilogIngredientsLabel.Text = opisTekst;
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

            FloatingPrilog.TranslationY = floatingStartY;
            FloatingPrilog.TranslationX = 0;
            FloatingPrilog.IsVisible = true;
            FloatingPrilog.Opacity = 1;

            await FloatingPrilog.TranslateTo(0, targetY, 400, Easing.CubicOut);

            isDetailVisible = true;
        }

        private async void OnBackClicked(object sender, EventArgs e)
        {
            if (isDetailVisible)
            {
                await FloatingPrilog.TranslateTo(0, floatingStartY, 400, Easing.CubicIn);
                FloatingPrilog.IsVisible = false;

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
            DisplayAlert("Korpa", "Prilog dodan u korpu!", "OK");
        }
    }
}