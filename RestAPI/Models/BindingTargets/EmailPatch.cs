using System.ComponentModel.DataAnnotations;

namespace RestAPI.Models.BindingTargets
{
    public class EmailPatch
    {
        [Required (ErrorMessage = "Please enter your password")]
        public string Password { get; set; }
        [Required (ErrorMessage = "Please enter your password")]
        [EmailAddress (ErrorMessage = "Invalid email")]
        public string NewEmail { get; set; }
    }
}
