using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace Support_Your_Locals.Models.ViewModels
{
    public class BusinessRegisterModel
    {

        public long BusinessId { get; set; } //Danger zone.
        [Required(ErrorMessage = "Please add your business description")]
        public string Description {get; set;}
        [RegularExpression(@"^[+]*[(]{0,1}[0-9]{1,4}[)]{0,1}[-\s\./0-9]*$", ErrorMessage = "Bad phone number")]
        [Required(ErrorMessage = "Please enter your business phone number")]
        public string PhoneNumber {get; set;}
        [Required(ErrorMessage = "Please enter your business header")]
        public string Header {get; set;}
        public string Longitude { get; set; }
        public string Latitude { get; set; }
        public IFormFile Picture { get; set; }
        public TimeSheetRegisterViewModel[] Workdays { get; set; } = new TimeSheetRegisterViewModel[7];
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
