using System;
using MSupportYourLocals.Infrastructure;
using MSupportYourLocals.Services;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MSupportYourLocals.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ChangePasswordView : ContentPage
    {

        private IUserService userService = DependencyService.Get<IUserService>();

        public ChangePasswordView()
        {
            InitializeComponent();
        }

        public async void Confirm(object sender, EventArgs e)
        {
            bool success = await userService.PatchPassword(CurrentPasswordEntry.Text, NewPasswordEntry.Text);
            if (success)
            {
                await this.DisplaySuccess("Successfully changed password.");
                await Navigation.PopAsync();
            }
            else
            {
                await this.DisplayFailure();
            }
        }


    }
}
