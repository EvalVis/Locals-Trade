using System;
using MSupportYourLocals.Views;
using Xamarin.Forms;

namespace MSupportYourLocals {
    public partial class MainPage : ContentPage {
        public MainPage() {
            InitializeComponent();
        }

        public async void Skipped(Object sender, EventArgs e)
        {
            await Navigation.PushAsync(new BusinessesView());
        }

    }
}
