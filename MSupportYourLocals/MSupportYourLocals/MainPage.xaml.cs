using System;
using MSupportYourLocals.Infrastructure;
using MSupportYourLocals.Services;
using MSupportYourLocals.ViewModels;
using MSupportYourLocals.Views;
using Xamarin.Forms;

namespace MSupportYourLocals
{
    public partial class MainPage : ContentPage
    {

        private IUserService userService = DependencyService.Get<IUserService>();
        private JsonWebTokenHolder tokenService = DependencyService.Get<JsonWebTokenHolder>();

        public MainPage()
        {
            InitializeComponent();
            Setup();
        }

        public void Setup()
        {
            if (tokenService.Token != null)
            {
                Stack.Children.Remove(LoginLabel);
                Stack.Children.Remove(EmailEntry);
                Stack.Children.Remove(PasswordEntry);
                Stack.Children.Remove(SignInButton);
                Stack.Children.Remove(SignUpButton);
            }
            else
            {
                Stack.Children.Remove(SignedInLabel);
                Stack.Children.Remove(LogoutButton);
            }
        }

        public async void Login(object sender, EventArgs e)
        {
            bool success = await userService.Login(EmailEntry.Text, PasswordEntry.Text);
            if (success)
            {
                await this.DisplaySuccess("Successfully logged in.");
                await Navigation.PushAsync(new BusinessesView(new BusinessesViewModel(1)));
            }
            else
            {
                await this.DisplayFailure();
            }
        }

        public async void SignUp(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new RegisterUserView());
        }

        public async void Continue(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new BusinessesView(new BusinessesViewModel(1)));
        }

        public async void Logout(object sender, EventArgs e)
        {
            tokenService.Logout();
            await Navigation.PushAsync(new MainPage());
        }

    }
}
