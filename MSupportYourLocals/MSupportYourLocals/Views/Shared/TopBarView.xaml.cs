using System;
using MSupportYourLocals.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MSupportYourLocals.Views.Shared
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class TopBarView : ContentPage
    {
        public TopBarView()
        {
            InitializeComponent();
        }

        public async void BusinessList(Object sender, EventArgs e)
        {
            await Navigation.PushAsync(new BusinessesView(new BusinessesViewModel()));
        }

        public async void UserBusinessList(Object sender, EventArgs e)
        {
            await Navigation.PushAsync(new BusinessesView(new UserBusinessViewModel()));
        }

    }
}
