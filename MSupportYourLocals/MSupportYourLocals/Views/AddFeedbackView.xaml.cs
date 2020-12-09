using System;
using MSupportYourLocals.Models;
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
                await feedbackService.SendFeedback(NameEntry.Text, FeedbackTextEntry.Text, businessId);
            }
        }

    }
}
