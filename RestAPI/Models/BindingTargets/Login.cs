using System.ComponentModel.DataAnnotations;

namespace RestAPI.Models.BindingTargets
{
    public class Login
    {
        [Required(ErrorMessage = "Please enter your email")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Please enter your password")]
        public string Password { get; set; }
    }
}
