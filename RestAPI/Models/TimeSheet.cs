using System;

namespace RestAPI.Models
{
    public class TimeSheet
    {
        public long TimeSheetID { get; set; }
        public DateTime From { get; set; }
        public DateTime To { get; set; }
        public int Weekday { get; set; }
        public long BusinessID { get; set; }
        public Business Business { get; set; }

        public void Update(TimeSheet workday)
        {
            From = workday.From;
            To = workday.To;
            Weekday = workday.Weekday;
        }

    }
}
