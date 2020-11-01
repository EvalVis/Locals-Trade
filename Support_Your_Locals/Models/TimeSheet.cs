using System;

namespace Support_Your_Locals.Models
{
    public class TimeSheet
    {
        public static string[] days = { "Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday", "Sunday" };
        public long TimeSheetID { get; set; }
        public DateTime From { get; set; }
        public DateTime To { get; set; }
        public int Weekday { get; set; }
        public long BusinessID { get; set; }

        public static bool FromEqualsTo(DateTime from, DateTime to)
        {

            return from.TimeOfDay.ToString() == to.TimeOfDay.ToString();
        }
    }
}
