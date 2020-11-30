using System;
using RestAPI.Cryptography;

namespace RestAPI.Models.BindingTargets
{
    public class UserBindingTarget
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        public DateTime BirthDate { get; set; }
        public string Email { get; set; }
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
