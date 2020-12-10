using System;
using MSupportYourLocals.Models;
using MSupportYourLocals.Services;
using MSupportYourLocals.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MSupportYourLocals.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AddAdvertisementView : ContentPage
    {

        private IBusinessService businessService = DependencyService.Get<IBusinessService>();
        private JsonWebTokenHolder tokenService = DependencyService.Get<JsonWebTokenHolder>();
        private Business business;

        public AddAdvertisementView(BusinessViewModel businessViewModel)
        {
            InitializeComponent();
            BindingContext = businessViewModel;
            business = businessViewModel?.Business;
        }

        public async void Submit(object sender, EventArgs e)
        {
            if (business == null)
            {
                business = new Business {Header = HeaderEntry.Text, Description = DescriptionEntry.Text, PhoneNumber = PhoneEntry.Text};
                await businessService.CreateBusiness(business);
            }
            else
            {
                await businessService.EditBusiness(business);
            }
            await Navigation.PopAsync();
        }
    }
}
