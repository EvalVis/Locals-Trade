using System.Collections.ObjectModel;
using System.Threading.Tasks;
using MSupportYourLocals.Models;

namespace MSupportYourLocals.Services
{
    public interface IFeedbackService
    {
        Task<ObservableCollection<Feedback>> GetFeedbacks(long businessId);
        Task SendFeedback(string sendersName, string text, long businessId);
    }
}
