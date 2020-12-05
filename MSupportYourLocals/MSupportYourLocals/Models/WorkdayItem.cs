using System;

namespace MSupportYourLocals.Models
{
    public class WorkdayItem
    {
        public DateTime From { get; set; }
        public DateTime To { get; set; }
        public int Weekday { get; set; }
    }
}
