namespace RestAPI.Models
{
    public class Question
    {
        public long QuestionId { get; set; }
        public string Email { get; set; }
        public string Text { get; set; }
        public string Response { get; set; }
        public bool IsAnswered { get; set; }
    }

}

     

