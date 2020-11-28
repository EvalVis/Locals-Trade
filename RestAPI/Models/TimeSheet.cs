using System;
using System.ComponentModel.DataAnnotations;

namespace RestAPI.Models
{
    public class TimeSheet
    {
        public long TimeSheetID { get; set; }
        [DataType(DataType.Time)]
        public DateTime From { get; set; }
        [DataType(DataType.Time)]
        public DateTime To { get; set; }
        public int Weekday { get; set; }
        public long BusinessID { get; set; }
        public Business Business { get; set; }

        public bool InvalidTime()
        {
            return From.Equals(To);
        }

    }
}
