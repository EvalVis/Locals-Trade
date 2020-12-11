using System;
using MSupportYourLocals.Models;
using MSupportYourLocals.Services;
using MSupportYourLocals.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MSupportYourLocals.Views.Shared
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class TopBarView : ContentPage
    {

        private IUserService userService = DependencyService.Get<IUserService>();
        private JsonWebTokenHolder tokenService = DependencyService.Get<JsonWebTokenHolder>();

        public TopBarView()
        {
            InitializeComponent();
        }

        public async void BusinessList(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new BusinessesView(new BusinessesViewModel()));
        }

        public async void UserBusinessList(object sender, EventArgs e)
        {
            if (tokenService.Token != null)
            {
                await Navigation.PushAsync(new BusinessesView(new UserBusinessViewModel()));
            }
            else
            {
                showAlert();
            }
        }

        public async void UserPanel(object sender, EventArgs e)
        {
            User user = await userService.GetUser();
            await Navigation.PushAsync(new UserPanelView(new UserViewModel(user)));
        }

        public async void AddAdvertisement(Object sender, EventArgs e)
        {
            if (tokenService.Token != null)
            {
                await Navigation.PushAsync(new AdvertisementView(new BusinessViewModel(null)));
            }
            else
            {
                showAlert();
            }
        }

        private async void showAlert()
        {
            await DisplayAlert("You are not signed in",
                "Please login or create an account to add a business advertisement", "OK");
        }

    }
}
