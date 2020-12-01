using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.IO;

namespace Support_Your_Locals.Models
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
        [NotMapped]
        public string Picture
        {
            get
            {
                var file = File.ReadAllBytes("Content/Images/business-icon.png");
                string imageBase64Data = Convert.ToBase64String(PictureData ?? file);
                return string.Format("data:image/jpg;base64,{0}", imageBase64Data);
            }
        }
        public byte[] PictureData { get; set; }
        public List<TimeSheet> Workdays { get; set; } = new List<TimeSheet>();
        public List<Product> Products { get; set; } = new List<Product>();
        public List<Feedback> Feedbacks { get; set; } = new List<Feedback>();
    }
}
