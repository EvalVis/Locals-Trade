﻿using System;
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
                System.Diagnostics.Debug.WriteLine(businessId + " Id yra toks");
                bool success = await feedbackService.SendFeedback(NameEntry.Text, FeedbackTextEntry.Text, businessId);
                if (success)
                {
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
