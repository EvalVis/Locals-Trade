using System;
using MSupportYourLocals.Infrastructure;

namespace MSupportYourLocals.Models
{
    public class Workday
    {
        public DateTime From { get; set; }
        public string FromTime => $"{From.Hour}:{From.Minute}";
        public DateTime To { get; set; }
        public string ToTime => $"{To.Hour}:{To.Minute}";
        public int Weekday { get; set; }
        public string WeekdayName()
        {
            if(Weekday > 0 && Weekday < 8) return DayNames.Days[Weekday];
            return null;
        }
    }
}
