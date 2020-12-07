using System;
using System.Linq;
using MSupportYourLocals.Models;
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


        public void BusinessSelected(object sender, SelectionChangedEventArgs e)
        {
            Object chosen = e.CurrentSelection.FirstOrDefault();
            if (chosen is Business)
            {
                Business business = chosen as Business;
                System.Diagnostics.Debug.WriteLine(business.User.Name);
            }
        }

    }
}
