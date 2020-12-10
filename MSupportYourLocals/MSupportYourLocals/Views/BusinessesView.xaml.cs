﻿using System;
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
        private ActionEnum action;
        private Business chosenBusiness;

        private IBusinessService businessService = DependencyService.Get<IBusinessService>();
        private IFeedbackService feedbackService = DependencyService.Get<IFeedbackService>();

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
            action = ActionEnum.Delete;
        }

        public async void BusinessEdit(object sender, EventArgs e)
        {
            PasswordEntry.Text = "";
            Controls.IsVisible = false;
            Verification.IsVisible = true;
            action = ActionEnum.Edit;
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
            BusinessList.SelectedItem = null;
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
            Controls.IsVisible = false;
            Verification.IsVisible = false;
        }

        private async Task DeleteBusiness(string password)
        {
            await businessService.DeleteBusiness(password, chosenBusiness.BusinessId);
        }

        private async Task EditBusiness(string password)
        {
            await Navigation.PushAsync(new AdvertisementView(new BusinessViewModel(chosenBusiness)));
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