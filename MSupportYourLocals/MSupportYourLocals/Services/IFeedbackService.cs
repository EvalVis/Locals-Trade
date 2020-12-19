using System.Collections.ObjectModel;
using System.Threading.Tasks;
using MSupportYourLocals.Models;

namespace MSupportYourLocals.Services
{
    public interface IFeedbackService
    {
        Task<ObservableCollection<Feedback>> GetFeedbacks(long businessId);
        Task<bool> SendFeedback(string senderName, string text, long businessId);
        Task<bool> DeleteAllFeedbacks(long businessId);
        Task<bool> DeleteFeedback(long feedbackId);
    }
}
