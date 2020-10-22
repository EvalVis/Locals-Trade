using System.ComponentModel.DataAnnotations;

namespace Support_Your_Locals.Models.ViewModels
{
    public class BusinessRegisterModel
    {

        [Required(ErrorMessage = "Please enter your product or activity")]
        public string Product {get; set;}
        [Required(ErrorMessage = "Please add your business description")]
        public string Description {get; set;}
        [Required(ErrorMessage = "Please enter your business phone number")]
        public string PhoneNumber {get; set;}
        [Required(ErrorMessage = "Please enter your business header")]
        public string Header {get; set;}
    }
}
