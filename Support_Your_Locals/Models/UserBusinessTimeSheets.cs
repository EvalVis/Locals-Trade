using System.Collections.Generic;

namespace Support_Your_Locals.Models
{
    public class UserBusinessTimeSheets
    {
        public string[] days = { "", "Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday", "Sunday"};
        public User User { get; set; }
        public Business Business { get; set; }
        public IEnumerable<TimeSheet> TimeSheets { get; set; }
    }
}
