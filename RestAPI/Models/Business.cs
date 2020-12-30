using System;
using System.Collections.Generic;

namespace RestAPI.Models
{

    public class Business
    {
        public long BusinessID { get; set; }
        public long UserID { get; set; }
        public User User { get; set; }
        public string Description { get; set; }
        public string Longitude { get; set; }
        public string Latitude { get; set; }
        public string PhoneNumber { get; set; }
        public string Header { get; set; }
        public byte[] PictureData { get; set; }
        public List<TimeSheet> Workdays { get; set; } = new List<TimeSheet>();
        public List<Product> Products { get; set; } = new List<Product>();
        public List<Feedback> Feedbacks { get; set; } = new List<Feedback>();

        public void EliminateDepth()
        {
            if (User != null)
            {
                User.Email = null;
                User.BirthDate = new DateTime(1999, 12, 06);
                User.Passhash = null;
                User.Businesses = null;
            }

            foreach (var w in Workdays) w.Business = null;
            foreach (var p in Products)
            {
                p.Business = null;
                p.EliminateDepth();
            }
            foreach (var f in Feedbacks) f.Business = null;
        }

    }
}
