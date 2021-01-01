using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using MSupportYourLocals.Infrastructure;
using MSupportYourLocals.Models;
using MSupportYourLocals.Services;
using MSupportYourLocals.ViewModels;
using MSupportYourLocals.Views.Shared;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MSupportYourLocals.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class BusinessesView : TopBarView
    {

        private bool personal;
        private Business chosenBusiness;

        private IBusinessService businessService = DependencyService.Get<IBusinessService>();
        private IFeedbackService feedbackService = DependencyService.Get<IFeedbackService>();
        private UserBusinessViewModel userBusinessViewModel;

        public BusinessesView(BusinessesViewModel businessesViewModel)
        {
            InitializeComponent();
            businessesViewModel?.ConcatAllProducts();
            BindingContext = businessesViewModel;
            int currentPage = businessesViewModel?.CurrentPage ?? 1;
            int totalPages = businessesViewModel?.TotalPages ?? 1;
            SearchView searchView = new SearchView(businessesViewModel.OwnersSurname, businessesViewModel.BusinessInfo,
                businessesViewModel.SearchIn, businessesViewModel.WeekdaySelected, businessesViewModel.OpenFrom, businessesViewModel.OpenTo);
            Stack.Children.Add(searchView);
            Stack.Children.Add(new PageView(currentPage, totalPages, searchView));
        }

        public BusinessesView(UserBusinessViewModel userBusinessViewModel)
        {
            InitializeComponent();
            userBusinessViewModel?.ConcatAllProducts();
            BindingContext = userBusinessViewModel;
            this.userBusinessViewModel = userBusinessViewModel;
            personal = true;
        }


        public async void BusinessSelected(object sender, SelectionChangedEventArgs e)
        {
            object chosen = e.CurrentSelection.FirstOrDefault();
            if (chosen is Business)
            {
                chosenBusiness = chosen as Business;
                if (!personal)
                {
                    BusinessList.SelectedItem = null;
                    await Navigation.PushAsync(new BusinessView(new BusinessViewModel(chosenBusiness)));
                }
                else
                {
                    Controls.IsVisible = true;
                }
            }
        }

        public async void BusinessDelete(object sender, EventArgs e)
        {
            PasswordEntry.Text = "";
            Controls.IsVisible = false;
            Verification.IsVisible = true;
        }

        public async void BusinessDetails(object sender, EventArgs e)
        {
            Controls.IsVisible = false;
            BusinessList.SelectedItem = null;
            await ShowDetails();
        }

        public async void BusinessFeedbacks(object sender, EventArgs e)
        {
            Controls.IsVisible = false;
            BusinessList.SelectedItem = null;
            await GetFeedbacks();
        }

        public void ClosePopUp(object sender, EventArgs e)
        {
            Controls.IsVisible = false;
            Verification.IsVisible = false;
            BusinessList.SelectedItem = null;
        }

        public void CancelAction(object sender, EventArgs e)
        {
            Controls.IsVisible = true;
            Verification.IsVisible = false;
        }

        public async void VerifyAction(object sender, EventArgs e)
        {
            string password = PasswordEntry.Text;
            if (!string.IsNullOrEmpty(password))
            {
                bool success = await DeleteBusiness(password);
                if (success)
                {
                    BusinessList.SelectedItem = null;
                    Controls.IsVisible = false;
                    Verification.IsVisible = false;
                }
            }
            else
            {
                await DisplayAlert("Verification needed", "Please enter your password", "OK");
            }
        }

        private async Task<bool> DeleteBusiness(string password)
        {
            bool success = await businessService.DeleteBusiness(password, chosenBusiness.BusinessId);
            if (success)
            {
                userBusinessViewModel?.Businesses?.Remove(chosenBusiness);
                BusinessList.ItemsSource = userBusinessViewModel?.Businesses;
                await this.DisplaySuccess("Business successfully deleted.");
                return true;
            }
            else
            {
                await this.DisplayFailure();
                return false;
            }
        }

        private async Task GetFeedbacks()
        {
            ObservableCollection<Feedback> feedbacks = await feedbackService.GetFeedbacks(chosenBusiness.BusinessId);
            await Navigation.PushAsync(new FeedbackView(chosenBusiness.BusinessId, new FeedbackViewModel(feedbacks)));
        }

        private async Task ShowDetails()
        {
            await Navigation.PushAsync(new BusinessView(new BusinessViewModel(chosenBusiness)));
        }

    }
}
