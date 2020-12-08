using System.Collections.ObjectModel;
using MSupportYourLocals.Models;

namespace MSupportYourLocals.ViewModels
{
    public class FeedbackViewModel
    {

        public ObservableCollection<Feedback> Feedbacks { get; set; }

        public FeedbackViewModel(ObservableCollection<Feedback> feedbacks)
        {
            Feedbacks = feedbacks;
        }

    }
}
