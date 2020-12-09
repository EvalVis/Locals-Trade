namespace RestAPI.Models.BindingTargets
{
    public class FeedbackBindingTarget
    {
        public string SenderName { get; set; }
        public string Text { get; set; }
        public long BusinessID { get; set; }

        public Feedback ToFeedback() => new Feedback
        {
            SenderName = SenderName,
            Text = Text,
            BusinessID = BusinessID
        };

    }
}
