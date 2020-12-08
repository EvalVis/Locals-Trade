using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace MSupportYourLocals.Services
{
    public class FeedbackService : Service, IFeedbackService
    {

        public async Task<ObservableCollection<Feedback>> GetFeedbacks()
        {

        }

    }
}
