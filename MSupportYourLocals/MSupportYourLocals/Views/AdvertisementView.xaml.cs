using System;
using System.Collections.Generic;
using System.Linq;
using MSupportYourLocals.Models;
using MSupportYourLocals.Services;
using MSupportYourLocals.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MSupportYourLocals.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AdvertisementView : ContentPage
    {

        private IBusinessService businessService = DependencyService.Get<IBusinessService>();
        private JsonWebTokenHolder tokenService = DependencyService.Get<JsonWebTokenHolder>();
        private Business business;
        public List<Workday> Workdays = new List<Workday>();

        public AdvertisementView(BusinessViewModel businessViewModel)
        {
            InitializeComponent();
            BindingContext = businessViewModel;
            business = businessViewModel?.Business;
            renderWorkdays();
        }

        private void renderWorkdays()
        {
            WorkdayCollection.ItemsSource = Workdays;
            for (int i = 1; i < 8; i++)
            {
                DateTime? from = business?.Workdays?.FirstOrDefault(w => w.Weekday == i)?.From;
                DateTime? to = business?.Workdays?.FirstOrDefault(w => w.Weekday == i)?.To;
                Workdays.Add(new Workday {From = from, To = to, Weekday = i});
            }
        }

        public async void Submit(object sender, EventArgs e)
        {
            if (business == null)
            {
                business = new Business {Header = HeaderEntry.Text, Description = DescriptionEntry.Text, PhoneNumber = PhoneEntry.Text};
                await businessService.CreateBusiness(business);
                await Navigation.PopAsync();
            }
            else
            {
                Verification.IsVisible = true;
            }
        }

        public async void Confirm(object sender, EventArgs e)
        {
            await businessService.UpdateBusiness(PasswordEntry.Text, business);
            await Navigation.PopAsync();
        }

        public void Cancel(object sender, EventArgs e)
        {
            Verification.IsVisible = false;
        }

    }
}
