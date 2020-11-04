namespace Support_Your_Locals.Models
{
    public class BirthDate
    {
        public int Year { get; set; }
        public int Month { get; set; }
        public int Day { get; set; }

        public BirthDate(int year, int month, int day)
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
