using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.IO;
using System.Linq;
using Support_Your_Locals.Models.ViewModels;

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
                string imageBase64Data = Convert.ToBase64String(PictureData ?? File.ReadAllBytes("Content/Images/business-icon.png"));
                return string.Format("data:image/jpg;base64,{0}", imageBase64Data);
            }
        }
        public byte[] PictureData { get; set; }
        public List<TimeSheet> Workdays { get; set; } = new List<TimeSheet>();
        public List<Product> Products { get; set; } = new List<Product>();
        public List<Feedback> Feedbacks { get; set; } = new List<Feedback>();

        public void CreateBusiness(BusinessRegisterModel registerModel, long userID)
        {
            UserID = userID;
            Description = registerModel.Description;
            Longitude = registerModel.Longitude;
            Latitude = registerModel.Latitude;
            PhoneNumber = registerModel.PhoneNumber;
            Header = registerModel.Header;
            Workdays = registerModel.Workdays.ToList();
            Products = registerModel.Products;
            if(registerModel.Picture != null)
            {
                MemoryStream imageMemoryStream = new MemoryStream();
                registerModel?.Picture?.CopyTo(imageMemoryStream);
                PictureData = imageMemoryStream.ToArray();
            }
        }

    }
}
