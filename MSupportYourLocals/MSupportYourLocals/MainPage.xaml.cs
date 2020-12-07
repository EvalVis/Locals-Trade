using System;
using MSupportYourLocals.Services;
using MSupportYourLocals.Views;
using Xamarin.Forms;

namespace MSupportYourLocals {
    public partial class MainPage : ContentPage {

        private ILoginService loginService = DependencyService.Get<ILoginService>();

        public MainPage() {
            InitializeComponent();
        }

        public async void Login(Object sender, EventArgs e)
        {
            await loginService.Login(EmailEntry.Text, PasswordEntry.Text);
            //await Navigation.PushAsync(new BusinessesView());
        }

        public async void Skipped(Object sender, EventArgs e)
        {
            await Navigation.PushAsync(new BusinessesView());
        }

    }
}
