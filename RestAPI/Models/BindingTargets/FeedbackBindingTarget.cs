namespace RestAPI.Models.BindingTargets
{
    public class FeedbackBindingTarget
    {
        public string SendersName { get; set; }
        public string Text { get; set; }
        public long BusinessID { get; set; }

        public Feedback ToFeedback() => new Feedback
        {
            SendersName = SendersName,
            Text = Text,
            BusinessID = BusinessID
        };

    }
}
