using System;
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
        private ActionEnum action;
        private Business chosenBusiness;

        private IBusinessService businessService = DependencyService.Get<IBusinessService>();

        public BusinessesView(BusinessesViewModel businessesViewModel)
        {
            InitializeComponent();
            BindingContext = businessesViewModel;
            personal = false;
        }

        public BusinessesView(UserBusinessViewModel userBusinessViewModel)
        {
            InitializeComponent();
            BindingContext = userBusinessViewModel;
            personal = true;
        }


        public async void BusinessSelected(object sender, SelectionChangedEventArgs e)
        {
            Object chosen = e.CurrentSelection.FirstOrDefault();
            if (chosen is Business)
            {
                chosenBusiness = chosen as Business;
                if (!personal)
                {
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
            action = ActionEnum.Delete;
        }

        public async void BusinessEdit(object sender, EventArgs e)
        {
            PasswordEntry.Text = "";
            Controls.IsVisible = false;
            Verification.IsVisible = true;
            action = ActionEnum.Edit;
        }

        public void BusinessDetails(object sender, EventArgs e)
        {
            ShowDetails();
        }

        public void BusinessFeedbacks(object sender, EventArgs e)
        {
            GetFeedbacks();
        }

        public void ClosePopUp(object sender, EventArgs e)
        {
            Controls.IsVisible = false;
            Verification.IsVisible = false;
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
                if (action == ActionEnum.Delete)
                {
                    await DeleteBusiness(password);
                }
                else if (action == ActionEnum.Edit)
                {
                    await EditBusiness(password);
                }
            }
        }

        private async Task DeleteBusiness(string password)
        {
            await businessService.DeleteBusiness(password, chosenBusiness.Id);
        }

        private async Task EditBusiness(string password)
        {
        }

        private void GetFeedbacks()
        {
        }

        private void ShowDetails()
        {
        }

    }
}
