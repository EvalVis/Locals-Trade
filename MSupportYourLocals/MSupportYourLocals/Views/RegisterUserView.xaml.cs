﻿using System;
using MSupportYourLocals.Models;
using MSupportYourLocals.Services;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MSupportYourLocals.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class RegisterUserView : ContentPage
    {

        private IRegisterUserService registerUserService = DependencyService.Get<IRegisterUserService>();

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
            await registerUserService.Register(target);
            // await Navigation.PushAsync(new RegisterUserView());
        }
    }
}
