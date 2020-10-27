using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Support_Your_Locals.Models.Repositories;

namespace Support_Your_Locals.Models
{
    public class SearchResponse
    {

        public string OwnersSurname { get; set; }
        public string Header { get; set; }
        public bool SearchInDescription { get; set; }
        public bool[] WeekdaySelected { get; set; } = {true, true, true, true, true, true, true};

        public IEnumerable<UserBusinessTimeSheets> FilterBusinesses(IEnumerable<Business> businesses, IServiceRepository repository)
        {
            foreach (var b in businesses)
            {
                if (BusinessConditionsMet(b))
                {
                    User user = repository.Users.FirstOrDefault(u => u.UserID == b.UserID);
                    if (UserConditionsMet(user))
                    {
                        IEnumerable<TimeSheet> timeSheets =
                            repository.TimeSheets.Where(t => t.BusinessID == b.BusinessID);
                        if (ChosenWeekday(timeSheets))
                        {
                            yield return new UserBusinessTimeSheets {User = user, Business = b, TimeSheets = timeSheets};
                        }
                    }
                }
            }
        }

        private bool UserConditionsMet(User user)
        {
            if (!String.IsNullOrEmpty(OwnersSurname)) return ChosenOwnersSurname(user);
            return true;
        }

        private bool BusinessConditionsMet(Business business)
        {
            if (!String.IsNullOrEmpty(Header))
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
