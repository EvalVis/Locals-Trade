using System.ComponentModel.DataAnnotations;
using Support_Your_Locals.Models;

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
