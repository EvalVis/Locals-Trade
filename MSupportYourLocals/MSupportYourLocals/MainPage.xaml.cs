using System;
using MSupportYourLocals.Infrastructure;
using MSupportYourLocals.Services;
using MSupportYourLocals.ViewModels;
using MSupportYourLocals.Views;
using Xamarin.Forms;

namespace MSupportYourLocals {
    public partial class MainPage : ContentPage {

        private IUserService userService = DependencyService.Get<IUserService>();

        public MainPage() {
            InitializeComponent();
        }

        public async void Login(Object sender, EventArgs e)
        {
            bool success = await userService.Login(EmailEntry.Text, PasswordEntry.Text);
            if (success)
            {
                await Navigation.PushAsync(new BusinessesView(new BusinessesViewModel(1)));
            }
            else
            {
                await this.DisplayFailure();
            }
        }

        public async void SignUp(Object sender, EventArgs e)
        {
            await Navigation.PushAsync(new RegisterUserView());
        }

        public async void Continue(Object sender, EventArgs e)
        {
            await Navigation.PushAsync(new BusinessesView(new BusinessesViewModel(1)));
        }

    }
}
