using System.ComponentModel.DataAnnotations;

namespace RestAPI.Models
{
    public class HelpMessage
    {
        [Required]
        public string Email { get; set; }
        [Required]
        public string Message { get; set; }
        public Question ToQuestion() => new Question { Email = Email, Text = Message, IsAnswered = false };
    }
}
