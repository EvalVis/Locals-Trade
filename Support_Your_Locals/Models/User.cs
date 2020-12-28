using System;
using System.Collections.Generic;

namespace Support_Your_Locals.Models
{
    public class User
    {
        public long UserID { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public DateTime BirthDate { get; set; }
        public string Email { get; set; }
        public string Passhash { get; set; }
        public List<Business> Businesses { get; set; } = new List<Business>();
        public List<Order> Orders { get; set; }

        public override bool Equals(object obj)
        {
            return obj is User user &&
                   UserID == user.UserID;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(UserID);
        }
    }
}
