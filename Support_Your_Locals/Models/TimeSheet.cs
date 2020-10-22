using System;

namespace Support_Your_Locals.Models
{
    public class TimeSheet
    {
        public long TimeSheetID { get; set; }
        public DateTime From { get; set; }
        public DateTime To { get; set; }
        public int Weekday { get; set; }
        public long BusinessID { get; set; }
    }
}
