using System;
using Microsoft.Maui.Controls;

namespace Camera.Pages
{
    public partial class MainPage : ContentPage
    {
        int count = 0;

        public MainPage()
        {
            InitializeComponent();
        }

        //private void OnCounterClicked(object sender, EventArgs e)
        //{
        //    count++;
        //    CounterBtn.Text = $"Clicked {count} time{(count > 1 ? "s" : "")}";
        //    SemanticScreenReader.Announce(CounterBtn.Text);
        //}

        private async void OnSettingsClicked(object sender, EventArgs e)
        {
            // Updated to Shell-based navigation
            await Shell.Current.GoToAsync("CameraSettingsPage");
        }
    }
}
