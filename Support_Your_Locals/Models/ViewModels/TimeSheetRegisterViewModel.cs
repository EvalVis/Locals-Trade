using System;

namespace Support_Your_Locals.Models.ViewModels
{
    public class TimeSheetRegisterViewModel
    {
        public DateTime? From { get; set; }
        public DateTime? To { get; set; }
        public int? Weekday { get; set; }

        public bool IsValid()
        {

            return From.HasValue && To.HasValue && From?.TimeOfDay.ToString() != To?.TimeOfDay.ToString() && Weekday.HasValue;
        }

    }
}
