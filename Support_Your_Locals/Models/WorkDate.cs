namespace Support_Your_Locals.Models
{
    public class WorkDate
    {

        public int Hours { get; set; }
        public int Minutes { get; set; }

        public WorkDate(int hours, int minutes)
        {
            Hours = hours;
            Minutes = minutes;
        }

        public override string ToString()
        {
            return Hours + ":" + Minutes;
        }
    }
}
