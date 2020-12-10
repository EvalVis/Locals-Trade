using System;
using MSupportYourLocals.Models;
using MSupportYourLocals.Services;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MSupportYourLocals.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AddAdvertisementView : ContentPage
    {

        private IBusinessService businessService = DependencyService.Get<IBusinessService>();
        private JsonWebTokenHolder tokenService = DependencyService.Get<JsonWebTokenHolder>();

        public AddAdvertisementView()
        {
            InitializeComponent();
        }

        public async void Submit(object sender, EventArgs e)
        {
            Business business = new Business {Header = HeaderEntry.Text, Description = DescriptionEntry.Text, PhoneNumber = PhoneEntry.Text};
            await businessService.CreateBusiness(business);
            await Navigation.PopAsync();
        }
    }
}
