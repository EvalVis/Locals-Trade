using System;
using MSupportYourLocals.Infrastructure;
using MSupportYourLocals.Models;
using MSupportYourLocals.Services;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MSupportYourLocals.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class RegisterUserView : ContentPage
    {

        private IUserService userService = DependencyService.Get<IUserService>();

        public RegisterUserView()
        {
            InitializeComponent();
        }

        public async void SignUp(Object sender, EventArgs e)
        {
            UserBindingTarget target = new UserBindingTarget()
            {
                Name = NameEntry.Text,
                Surname = SurnameEntry.Text,
                BirthDate = BirthDate.Date,
                Email = EmailEntry.Text,
                Password = PasswordEntry.Text
            };
            bool success = await userService.Register(target);
            if (success)
            {
                await this.DisplaySuccess("Account successfully created. You can now log in.");
                await Navigation.PopAsync();
            }
            else
            {
                await this.DisplayFailure();
            }
        }
    }
}
