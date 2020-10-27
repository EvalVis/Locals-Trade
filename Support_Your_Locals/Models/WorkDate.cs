namespace Support_Your_Locals.Models
{
    public class WorkDate
    {

        public string Hours { get; set; }
        public string Minutes { get; set; }

        public WorkDate(string hours, string minutes)
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
