using EasyPeasyAPP.Pages.Auth;
using System;

namespace EasyPeasyAPP.Pages
{
    public partial class OrderPage : ContentPage
    {
        public OrderPage()
        {
            InitializeComponent();
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

        private async void OnBurgeriClicked(object sender, EventArgs e)
        {
            try
            {
                if (Shell.Current != null)
                {
                    await Shell.Current.GoToAsync("//BurgerPage");
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", $"Navigation failed: {ex.Message}", "OK");
            }
        }

        private async void OnDogsClicked(object sender, EventArgs e)
        {
            try
            {
                if (Shell.Current != null)
                {
                    await Shell.Current.GoToAsync("//DogsPage");
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", $"Navigation failed: {ex.Message}", "OK");
            }
        }

        private async void OnSendviciClicked(object sender, EventArgs e)
        {
            try
            {
                if (Shell.Current != null)
                {
                    await Shell.Current.GoToAsync("//SendviciPage");
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", $"Navigation failed: {ex.Message}", "OK");
            }
        }

        private async void OnSokoviClicked(object sender, EventArgs e)
        {
            try
            {
                if (Shell.Current != null)
                {
                    await Shell.Current.GoToAsync("//SokoviPage");
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", $"Navigation failed: {ex.Message}", "OK");
            }
        }

        private async void OnPriloziClicked(object sender, EventArgs e)
        {
            try
            {
                if (Shell.Current != null)
                {
                    await Shell.Current.GoToAsync("//PriloziPage");
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", $"Navigation failed: {ex.Message}", "OK");
            }
        }
    }
}