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
            await Navigation.PushAsync(new BusinessesView(new UserBusinessViewModel()));
        }

        public async void UserPanel(object sender, EventArgs e)
        {
            User user = await userService.GetUser();
            await Navigation.PushAsync(new UserPanelView(new UserViewModel(user)));
        }

    }
}
