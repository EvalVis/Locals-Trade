using System;
using System.Collections.Generic;

namespace RestAPI.Models
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
        public List<Order> Orders { get; set; } = new List<Order>();

    }
}
