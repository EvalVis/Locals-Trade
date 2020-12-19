using System;

namespace MSupportYourLocals.Models
{
    public class User
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        public string FullName => $"{Name} {Surname}";
        public DateTime BirthDate { get; set; }
        public string Email { get; set; }
    }
}
