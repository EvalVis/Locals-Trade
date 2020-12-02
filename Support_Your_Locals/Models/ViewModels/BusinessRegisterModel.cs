using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Support_Your_Locals.Models.ViewModels
{
    public class BusinessRegisterModel
    {


        public long BusinessId { get; set; } //Danger zone.
        [Required(ErrorMessage = "Please add description for your business")]
        public string Description {get; set;}
        [RegularExpression(@"^[+]*[(]{0,1}[0-9]{1,4}[)]{0,1}[-\s\./0-9]*$", ErrorMessage = "Entered phone number is not correct")]
        [Required(ErrorMessage = "Please enter correct phone number for your business")]
        public string PhoneNumber {get; set;}
        [Required(ErrorMessage = "Please enter header for your business")]
        public string Header {get; set;}
        public string Longitude { get; set; }
        public string Latitude { get; set; }
        public string Picture { get; set; }
        public TimeSheet[] Workdays { get; set; } = new TimeSheet[7];
        public List<Product> Products { get; set; } = new List<Product>();

        public BusinessRegisterModel()
        {
            
        }

        public BusinessRegisterModel(Business business)
        {
            BusinessId = business.BusinessID;
            Description = business.Description;
            PhoneNumber = business.PhoneNumber;
            Header = business.Header;
            Longitude = business.Longitude;
            Latitude = business.Latitude;
            Picture = business.Picture;
            Products = business.Products;
            foreach (TimeSheet workday in business.Workdays)
            { 
                Workdays[workday.Weekday - 1] = workday;
            }
        }
    }
}
