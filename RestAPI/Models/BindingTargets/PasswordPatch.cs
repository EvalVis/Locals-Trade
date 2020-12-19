using System.ComponentModel.DataAnnotations;

namespace RestAPI.Models.BindingTargets
{
    public class PasswordPatch
    {
        [Required(ErrorMessage = "Please enter your current password")]
        public string CurrentPassword { get; set; }
        [Required (ErrorMessage = "Please enter a new password")]
        public string NewPassword { get; set; }
    }
}
