using System;
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

        private void Button_OnClicked(object sender, EventArgs e)
        {
            System.Diagnostics.Debug.WriteLine(sender.GetType());
        }
    }
}
