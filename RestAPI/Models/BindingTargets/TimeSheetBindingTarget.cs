using System;

namespace RestAPI.Models.BindingTargets
{
    public class TimeSheetBindingTarget
    {
        public DateTime From { get; set; }
        public DateTime To { get; set; }
        public int Weekday { get; set; }
        public TimeSheet ToWorkday => new TimeSheet { From = From, To = To, Weekday = Weekday};

        public bool InvalidTime()
        {
            return From.Equals(To);
        }
    }
}
