using System.ComponentModel.DataAnnotations;

namespace Support_Your_Locals.Models.ViewModels
{
    public class UserLoginModel
    {

        [Required(ErrorMessage = "Please enter your email address")]
        public string Email { get; set; }
    }
}
