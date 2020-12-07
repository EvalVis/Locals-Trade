using System;
using System.Linq;
using MSupportYourLocals.Models;
using MSupportYourLocals.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MSupportYourLocals.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class BusinessesView : ContentPage
    {
        public BusinessesView()
        {
            InitializeComponent();
        }


        public async void BusinessSelected(object sender, SelectionChangedEventArgs e)
        {
            Object chosen = e.CurrentSelection.FirstOrDefault();
            if (chosen is Business)
            {
                Business business = chosen as Business;
                await Navigation.PushAsync(new BusinessView(new BusinessViewModel(business)));
            }
        }

    }
}
