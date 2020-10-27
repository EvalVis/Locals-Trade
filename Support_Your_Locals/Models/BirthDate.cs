namespace Support_Your_Locals.Models
{
    public class BirthDate
    {
        public string Year { get; set; }
        public string Month { get; set; }
        public string Day { get; set; }

        public BirthDate(string year, string month, string day)
        {
            Year = year;
            Month = month;
            Day = day;
        }

        public override string ToString()
        {
            return Year + "-" + Month + "-" + Day;
        }
    }
}
