using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Support_Your_Locals.Models.ViewModels
{
    public class BusinessRegisterModel
    {

        [Required(ErrorMessage = "Please add your business description")]
        public string Description {get; set;}
        [RegularExpression(@"^[+]*[(]{0,1}[0-9]{1,4}[)]{0,1}[-\s\./0-9]*$", ErrorMessage = "Bad phone number")]
        [Required(ErrorMessage = "Please enter your business phone number")]
        public string PhoneNumber {get; set;}
        [Required(ErrorMessage = "Please enter your business header")]
        public string Header {get; set;}
        public string Longitude { get; set; }
        public string Latitude { get; set; }
        public List<string> Pictures { get; set; }
        public TimeSheetRegisterViewModel[] Workdays { get; set; } = new TimeSheetRegisterViewModel[7];
        //Binded with add advertisement.
        public List<Product> Products { get; set; } = new List<Product>();
    }
}
