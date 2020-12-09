namespace RestAPI.Models
{
    public class Feedback
    {

        public long ID { get; set; }
        public string SendersName { get; set; }
        public string Text { get; set; }
        public long BusinessID { get; set; }
        public Business Business { get; set; }
    }
}
