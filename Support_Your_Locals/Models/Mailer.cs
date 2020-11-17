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

        public void SendMail()
        {
            System.Diagnostics.Debug.WriteLine(this.GetHashCode() + " Sent");
        }

        public void Mute()
        {
            BusinessController.FeedbackEvent -= SendMail;
        }

    }
}
