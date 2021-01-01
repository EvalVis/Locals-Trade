using System;
using System.Collections.ObjectModel;
using System.Linq;
using MSupportYourLocals.Infrastructure;
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
        private Business business;
        public ObservableCollection<Workday> Workdays = new ObservableCollection<Workday>();
        public ObservableCollection<Product> Products = new ObservableCollection<Product>();

        public AdvertisementView(BusinessViewModel businessViewModel)
        {
            InitializeComponent();
            BindingContext = businessViewModel;
            business = businessViewModel?.Business;
            if (business != null)
            {
                SubmitButton.Text = "Change";
                ProductButton.IsVisible = false;
                ProductCollection.IsVisible = false;
            }
            else
            {
                renderProducts();
            }
            renderWorkdays();
        }

        private void renderWorkdays()
        {
            WorkdayCollection.ItemsSource = Workdays;
            for (int i = 1; i < 8; i++)
            {
                DateTime? from = business?.Workdays?.FirstOrDefault(w => w.Weekday == i)?.From;
                DateTime? to = business?.Workdays?.FirstOrDefault(w => w.Weekday == i)?.To;
                Workdays.Add(new Workday { From = from, To = to, Weekday = i });
            }
        }

        private void renderProducts()
        {
            ProductCollection.ItemsSource = Products;
            if (business?.Products == null) return;
            foreach (var p in business.Products)
            {
                Products.Add(p);
            }
        }

        public void AddProduct(object sender, EventArgs e)
        {
            Products.Add(new Product());
        }

        public async void Submit(object sender, EventArgs e)
        {
            if (business == null)
            {
                business = new Business { Header = HeaderEntry.Text, Description = DescriptionEntry.Text, PhoneNumber = PhoneEntry.Text, Products = Products, Workdays = Workdays };
                bool success = await businessService.CreateBusiness(business);
                if (success)
                {
                    await this.DisplaySuccess("Business successfully created.");
                    await Navigation.PopAsync();
                }
                else
                {
                    await this.DisplayFailure();
                }
            }
            else
            {
                Verification.IsVisible = true;
            }
        }

        public async void Confirm(object sender, EventArgs e)
        {
            business.Products = Products;
            business.Workdays = Workdays;
            bool success = await businessService.UpdateBusiness(PasswordEntry.Text, business);
            if (success)
            {
                await this.DisplaySuccess("Business successfully updated.");
                await Navigation.PopAsync();
            }
            else
            {
                await this.DisplayFailure();
            }
        }

        public void Cancel(object sender, EventArgs e)
        {
            Verification.IsVisible = false;
        }

    }
}
