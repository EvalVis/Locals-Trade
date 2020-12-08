namespace MSupportYourLocals.Services
{
    public interface IFeedbackService
    {
        Task<ObservableCollection<Feedback> GetFeedbacks();
    }
}
