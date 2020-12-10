using System;
using MSupportYourLocals.Services;
using MSupportYourLocals.ViewModels;
using MSupportYourLocals.Views.Shared;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MSupportYourLocals.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class UserPanelView : TopBarView
    {
        private JsonWebTokenHolder tokenService = DependencyService.Get<JsonWebTokenHolder>();

        public UserPanelView(UserViewModel userViewModel)
        {
            InitializeComponent();
            BindingContext = userViewModel;
            LoginOptions.IsVisible = false;
            setContent(userViewModel);
        }

        private void setContent(UserViewModel userViewModel)
        {
            string name = userViewModel?.User?.Name;
            string surname = userViewModel?.User?.Surname;
            string birthDate = userViewModel?.User?.BirthDate.ToShortDateString();
            string email = userViewModel?.User?.Email;
            Content = new StackLayout() {Children =
            {
                new Label {Text = "Name"},
                new Label {Text = name},
                new Label {Text = "Surname"},
                new Label {Text = surname},
                new Label {Text = "Birth date"},
                new Label {Text = birthDate},
                new Label {Text = "Email"},
                new Label {Text = email}
            }
            };
        }

        public async void Login(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new MainPage());
        }

        public async void SignUp(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new RegisterUserView());
        }

        public async void ChangeEmail(object sender, EventArgs e)
        {

        }

        public async void ChangePassword(object sender, EventArgs e)
        {

        }

    }
}
