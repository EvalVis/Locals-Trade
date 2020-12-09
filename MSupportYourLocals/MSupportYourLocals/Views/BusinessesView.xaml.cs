using System;
using System.Linq;
using MSupportYourLocals.Models;
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
                Business business = chosen as Business;
                if (!personal)
                {
                    await Navigation.PushAsync(new BusinessView(new BusinessViewModel(business)));
                }
                else
                {
                    Controls.IsVisible = true;
                }
            }
        }

    }
}
