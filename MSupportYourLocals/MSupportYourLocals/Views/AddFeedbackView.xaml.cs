using System;
using MSupportYourLocals.Infrastructure;
using MSupportYourLocals.Services;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MSupportYourLocals.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AddFeedbackView : ContentPage
    {

        private long businessId;

        private IFeedbackService feedbackService = DependencyService.Get<IFeedbackService>();

        public AddFeedbackView(long businessId)
        {
            InitializeComponent();
            this.businessId = businessId;
        }

        public async void Send(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(FeedbackTextEntry.Text))
            {
                bool success = await feedbackService.SendFeedback(NameEntry.Text, FeedbackTextEntry.Text, businessId);
                if (success)
                {
                    await this.DisplaySuccess("Feedback successfully sent.");
                    await Navigation.PopAsync();
                }
                else
                {
                    await this.DisplayFailure();
                }
            }
        }

    }
}
