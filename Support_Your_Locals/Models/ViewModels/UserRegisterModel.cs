using System;
using System.ComponentModel.DataAnnotations;

namespace Support_Your_Locals.Models.ViewModels
{
    public class UserRegisterModel
    {
        [Required(ErrorMessage = "Please enter your name")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Please enter your surname")]
        public string Surname { get; set; }
        [Required(ErrorMessage = "Please enter your birth date")]
        public DateTime BirthDate { get; set; }
        [Required(ErrorMessage = "Please enter your email address")]
        public string Email { get; set; }

    }
}
