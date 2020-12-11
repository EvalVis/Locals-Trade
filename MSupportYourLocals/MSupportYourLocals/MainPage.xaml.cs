using System;
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
            await userService.Login(EmailEntry.Text, PasswordEntry.Text);
            //await Navigation.PushAsync(new BusinessesView());
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
