using System;
using System.ComponentModel.DataAnnotations;
using RestAPI.Cryptography;

namespace RestAPI.Models.BindingTargets
{
    public class UserBindingTarget
    {
        [Required(ErrorMessage = "Please enter your name")]
        [StringLength(50, ErrorMessage = "Maximum length is 50 characters. Please contact support")]
        [RegularExpression(@"^[a-zA-Z\s]+$", ErrorMessage = "Your name should contain only letters. Please concat support")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Please enter your surname")]
        [StringLength(100, ErrorMessage = "Maximum length is 100 characters. Please contact support")]
        [RegularExpression(@"^[a-zA-Z\s]+$", ErrorMessage = "Your surname should contain only letters. Please concat support")]
        public string Surname { get; set; }
        public DateTime BirthDate { get; set; }
        [EmailAddress(ErrorMessage = "Please enter a valid email address")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Please enter your password")]
        public string Password { get; set; }
        public User ToUser() => new User
        {
            Name = Name,
            Surname = Surname,
            BirthDate = BirthDate,
            Email = Email,
            Passhash = new HashCalculator().PassHash(Password)
        };
    }
}
