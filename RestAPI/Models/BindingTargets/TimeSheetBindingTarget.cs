using System;
using System.ComponentModel.DataAnnotations;

namespace RestAPI.Models.BindingTargets
{
    public class TimeSheetBindingTarget
    {
        [Required(ErrorMessage ="Empty opening time")]
        public DateTime From { get; set; }
        [Required(ErrorMessage = "Empty closing time")]
        public DateTime To { get; set; }
        [Required(ErrorMessage="Please enter a weekday number from 1 to 7")]
        [Range(1, 7, ErrorMessage = "Weekday number must be between 1 and 7")]
        public int Weekday { get; set; }
        public TimeSheet ToWorkday() => new TimeSheet { From = From, To = To, Weekday = Weekday };

        public bool InvalidTime()
        {
            return From.Equals(To);
        }
    }
}
