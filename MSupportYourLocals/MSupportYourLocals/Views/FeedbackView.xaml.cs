using System;
using System.Linq;
using MSupportYourLocals.Models;
using MSupportYourLocals.Services;
using MSupportYourLocals.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MSupportYourLocals.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class FeedbackView : ContentPage
    {

        private IFeedbackService feedbackService = DependencyService.Get<IFeedbackService>();
        private Feedback chosenFeedback;
        private long businessId;

        public FeedbackView(long businessId, FeedbackViewModel feedbackViewModel)
        {
            InitializeComponent();
            BindingContext = feedbackViewModel;
            this.businessId = businessId;
        }

        public async void ConfirmTotalDeletion(object sender, EventArgs e)
        {
            await feedbackService.DeleteAllFeedbacks(businessId);
            Verification.IsVisible = false;
        }

        public void CancelTotalDeletion(object sender, EventArgs e)
        {
            Verification.IsVisible = false;
        }

        public void DeleteAllFeedbacks(object sender, EventArgs e)
        {
            Verification.IsVisible = true;
        }

        public void CancelSingleDelete(object sender, EventArgs e)
        {
            Confirmation.IsVisible = false;
        }

        public void DeleteSingle(object sender, EventArgs e)
        {
            Controls.IsVisible = false;
            Confirmation.IsVisible = true;
        }

        public void CancelSingle(object sender, EventArgs e)
        {
            Controls.IsVisible = false;
        }

        public async void ConfirmSingleDelete(object sender, EventArgs e)
        {
            await feedbackService.DeleteFeedback(chosenFeedback.ID);
            Controls.IsVisible = false;
            Confirmation.IsVisible = false;
        }

        public void FeedbackSelected(object sender, SelectionChangedEventArgs e)
        {
            Object chosen = e.CurrentSelection.FirstOrDefault();
            if (chosen is Feedback)
            {
                chosenFeedback = chosen as Feedback;
                Controls.IsVisible = true;
            }
        }
    }
}
