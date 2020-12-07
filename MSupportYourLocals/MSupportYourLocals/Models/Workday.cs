using System;

namespace MSupportYourLocals.Models
{
    public class Workday
    {
        public DateTime From { get; set; }
        public DateTime To { get; set; }
        public int Weekday { get; set; }
    }
}
