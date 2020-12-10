using System;
using MSupportYourLocals.Infrastructure;

namespace MSupportYourLocals.Models
{
    public class Workday
    {
        public DateTime From { get; set; }
        public string FromTime => $"{From.TimeOfDay}";
        public DateTime To { get; set; }
        public string ToTime => $"{To.TimeOfDay}";
        public int Weekday { get; set; }
        public string WeekdayName => DayNames.Days[Weekday - 1];

    }
}
