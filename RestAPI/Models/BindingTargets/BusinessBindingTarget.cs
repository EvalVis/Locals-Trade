using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace RestAPI.Models.BindingTargets
{
    public class BusinessBindingTarget
    {
        [Required(ErrorMessage = "Please enter a description")]
        [StringLength(10000, MinimumLength = 50, ErrorMessage = "The length of the description must be between 50 and 10000 symbols.")]
        public string Description { get; set; }
        public string Longitude { get; set; }
        public string Latitude { get; set; }
        [Required(ErrorMessage = "Please enter a phone number")]
        [Phone (ErrorMessage = "Invalid phone number")]
        public string PhoneNumber { get; set; }
        [Required(ErrorMessage = "Please enter a header")]
        [StringLength(100, ErrorMessage = "The length of the header must be up to 100 symbols.")]
        public string Header { get; set; }
        public string Picture { get; set; }
        public List<TimeSheetBindingTarget> WorkdayTargets { get; set; } = new List<TimeSheetBindingTarget>();
        public List<ProductBindingTarget> ProductTargets { get; set; } = new List<ProductBindingTarget>();
        public Business ToBusiness(long userId) => new Business
        {
            UserID = userId,
            Description = Description,
            Longitude = Longitude,
            Latitude = Latitude,
            PhoneNumber = PhoneNumber,
            Header = Header,
            Workdays = ToWorkdays().ToList(),
            Products = ToProducts().ToList()
        };

        public IEnumerable<TimeSheet> ToWorkdays()
        {
            foreach (var t in WorkdayTargets)
            {
                if (!t.InvalidTime())
                {
                    yield return new TimeSheet { From = t.From, To = t.To, Weekday = t.Weekday };
                }
            }
        }

        public IEnumerable<Product> ToProducts()
        {
            foreach (var t in ProductTargets)
            {
                yield return new Product { Name = t.Name, PricePerUnit = t.PricePerUnit, Unit = t.Unit, Comment = t.Comment };
            }
        }

    }
}
