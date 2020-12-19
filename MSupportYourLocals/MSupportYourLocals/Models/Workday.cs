using System;
using MSupportYourLocals.Infrastructure;

namespace MSupportYourLocals.Models
{
    public class Workday
    {
        public DateTime? From { get; set; }
        public DateTime? To { get; set; }
        public int Weekday { get; set; }
        public string WeekdayName => DayNames.Days[Weekday - 1];

    }
}
