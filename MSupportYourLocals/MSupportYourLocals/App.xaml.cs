using MSupportYourLocals.Services;
using Xamarin.Forms;

namespace MSupportYourLocals {
    public partial class App : Application {
        public App() {
            InitializeComponent();
            DependencyService.Register<ILoginService, LoginService>();
            DependencyService.Register<IBusinessService, BusinessService>();
            MainPage = new NavigationPage(new MainPage());
        }

        protected override void OnStart() {
        }

        protected override void OnSleep() {
        }

        protected override void OnResume() {
        }
    }
}
