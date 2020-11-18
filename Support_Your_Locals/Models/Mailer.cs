using Support_Your_Locals.Controllers;

namespace Support_Your_Locals.Models
{
    public class Mailer
    {

        public Mailer()
        {
            System.Diagnostics.Debug.WriteLine(this + " Initialized.");
            BusinessController.FeedbackEvent += SendMail;
        }

        public void SendMail(Feedback feedback)
        {
            System.Diagnostics.Debug.WriteLine(feedback.Text);
        }

        public void Mute()
        {
            BusinessController.FeedbackEvent -= SendMail;
        }

    }
}
