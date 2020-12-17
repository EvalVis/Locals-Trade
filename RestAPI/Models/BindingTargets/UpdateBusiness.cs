using System.ComponentModel.DataAnnotations;

namespace RestAPI.Models.BindingTargets
{
    public class UpdateBusiness
    {
        [Required(ErrorMessage = "Please enter a password")]
        public string Password { get; set; }
        [Required (ErrorMessage = "No information about business being updated given")]
        public Business Business { get; set; }
    }
}
