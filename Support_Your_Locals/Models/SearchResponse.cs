using System.Collections.Generic;
using System.Linq;

namespace Support_Your_Locals.Models
{
    public class SearchResponse
    {

        public string OwnersSurname { get; set; }
        public string Header { get; set; }
        public bool SearchInDescription { get; set; }
        public bool[] WeekdaySelected { get; set; }

        public SearchResponse()
        {
            WeekdaySelected = new bool[7];
            for (int i = 0; i < 7; i++) WeekdaySelected[i] = true;
        }

        public IEnumerable<Business> FilterBusinesses(IEnumerable<Business> businesses)
        {
            return businesses.Where(b => BusinessConditionsMet(b) && UserConditionsMet(b.User) && ChosenWeekday(b.Workdays));
        }

        private bool UserConditionsMet(User user)
        {
            if (!string.IsNullOrEmpty(OwnersSurname)) return ChosenOwnersSurname(user);
            return true;
        }

        private bool BusinessConditionsMet(Business business)
        {
            if (!string.IsNullOrEmpty(Header))
            {
                if (SearchInDescription)
                {
                    if (!ChosenDescription(business)) return false;
                }
                else if (!ChosenHeader(business)) return false;
            }
            return true;
        }


        private bool ChosenHeader(Business business)
        {
            return business.Header.ToLower().Contains(Header.ToLower());
        }

        private bool ChosenDescription(Business business)
        {
            return business.Description.ToLower().Contains(Header.ToLower()); // OK, Header is Description if search in description is ticked.
        }

        //TODO: Search by working hours.

        private bool ChosenWeekday(IEnumerable<TimeSheet> timeSheets)
        {
            return timeSheets.Count(t => WeekdaySelected[t.Weekday - 1]) > 0;
        }

        private bool ChosenOwnersSurname(User user)
        {
            return user.Surname.ToLower().Contains(OwnersSurname.ToLower());
        }

    }
}
