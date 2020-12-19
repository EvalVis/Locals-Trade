using System;
using MSupportYourLocals.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MSupportYourLocals.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class BusinessView : ContentPage
    {

        private long businessId;

        public BusinessView(BusinessViewModel businessesViewModel)
        {
            InitializeComponent();
            BindingContext = businessesViewModel;
            businessId = businessesViewModel.Business.BusinessId;
        }

        public async void OpenFeedbackWindow(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new AddFeedbackView(businessId));
        }

    }
}
