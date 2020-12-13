using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using RestAPI.Models.Repositories;

namespace RestAPI.Models
{
    public class SearchEngine
    {
        public string OwnersSurname { get; set; }
        public string BusinessInfo { get; set; }
        public int SearchIn { get; set; }
        public bool[] WeekdaySelected { get; set; } = new bool[7];
        public DateTime OpenFrom { get; set; }
        public DateTime OpenTo { get; set; }


        public IEnumerable<Business> FilterBusinesses(int page, int pageSize, IServiceRepository repository)
        {
            return repository.Business.Include(b => b.User).
                Include(b => b.Workdays).Include(b => b.Products).OrderByDescending(b => b.BusinessID).
                Skip((page - 1) * pageSize).Take(pageSize);
            //Where(b => BusinessConditionsMet(b) && UserConditionsMet(b.User) && ChosenWeekday(b.Workdays) && ChosenTimeInterval(b.Workdays))
        }

        private bool UserConditionsMet(User user)
        {
            if (!string.IsNullOrEmpty(OwnersSurname)) return ChosenOwnersSurname(user);
            return true;
        }

        private bool BusinessConditionsMet(Business business)
        {
            if (!string.IsNullOrEmpty(BusinessInfo))
            {
                if (SearchIn == 0)
                {
                    if (!ChosenHeader(business)) return false;
                }
                else if (SearchIn == 1)
                {
                    if (!ChosenDescription(business)) return false;
                }
                else if (!ChosenDescription(business) || ChosenHeader(business)) return false;
            }
            return true;
        }

        private bool ChosenHeader(Business business)
        {
            return business.Header.ToLower().Contains(BusinessInfo.ToLower());
        }

        private bool ChosenDescription(Business business)
        {
            return business.Description.ToLower().Contains(BusinessInfo.ToLower()); // OK, BusinessInfo is Description if search in description is ticked.
        }

        private bool ChosenWeekday(IEnumerable<TimeSheet> timeSheets)
        {
            return timeSheets.Count(t => WeekdaySelected[t.Weekday - 1]) > 0;
        }

        private bool ChosenOwnersSurname(User user)
        {
            return user.Surname.ToLower().Contains(OwnersSurname.ToLower());
        }

        private bool ChosenTimeInterval(IEnumerable<TimeSheet> timeSheets)
        {
            return timeSheets.All(t => t.From.TimeOfDay <= OpenFrom.TimeOfDay && t.To.TimeOfDay <= OpenTo.TimeOfDay);
        }

    }
}
