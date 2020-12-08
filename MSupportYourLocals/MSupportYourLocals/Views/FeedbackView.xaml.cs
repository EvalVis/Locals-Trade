using MSupportYourLocals.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MSupportYourLocals.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class FeedbackView : ContentPage
    {
        public FeedbackView(FeedbackViewModel feedbackViewModel)
        {
            InitializeComponent();
            BindingContext = feedbackViewModel;
        }
    }
}
