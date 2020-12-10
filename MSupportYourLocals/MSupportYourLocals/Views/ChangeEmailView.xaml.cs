using System;
using MSupportYourLocals.Services;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MSupportYourLocals.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ChangeEmailView : ContentPage
    {

        private IUserService userService = DependencyService.Get<IUserService>();

        public ChangeEmailView()
        {
            InitializeComponent();
        }

        public async void Confirm(object sender, EventArgs e)
        {
            await userService.PatchEmail(PasswordEntry.Text, NewEmailEntry.Text);
            await Navigation.PopAsync();
        }

    }
}
