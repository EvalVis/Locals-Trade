using System.ComponentModel.DataAnnotations;

namespace RestAPI.Models.BindingTargets
{
    public class FeedbackBindingTarget
    {
        public string SenderName { get; set; }
        [Required(ErrorMessage = "Please enter your feedback")]
        public string Text { get; set; }
        [Required(ErrorMessage = "No target business ID given")]
        [Range(1, long.MaxValue, ErrorMessage = "Bad business ID given")]
        public long BusinessID { get; set; }

        public Feedback ToFeedback() => new Feedback
        {
            SenderName = SenderName,
            Text = Text,
            BusinessID = BusinessID
        };

    }
}
