using System.Collections.Generic;
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
        public string Picture { get; set; }
        public List<TimeSheet> Workdays { get; set; } = new List<TimeSheet>();
        public List<Product> Products { get; set; } = new List<Product>();
        public List<Feedback> Feedbacks { get; set; } = new List<Feedback>();

        public Business()
        {
            
        }

        public Business(BusinessRegisterModel registerModel, long userID)
        {
            BusinessID = registerModel.BusinessId; // danger zone.
            UserID = userID;
            Description = registerModel.Description;
            Longitude = registerModel.Longitude;
            Latitude = registerModel.Latitude;
            PhoneNumber = registerModel.PhoneNumber;
            Header = registerModel.Header;
            Picture = registerModel.Picture;
            Workdays = registerModel.Workdays.ToList();
            Products = registerModel.Products;
        }

        public void UpdateBusiness(BusinessRegisterModel registerModel)
        {
            Description = registerModel.Description;
            Longitude = registerModel.Longitude;
            Latitude = registerModel.Latitude;
            PhoneNumber = registerModel.PhoneNumber;
            Header = registerModel.Header;
            Picture = registerModel.Picture;
            Workdays = registerModel.Workdays.ToList();
            Products = registerModel.Products;
        }

    }
}
