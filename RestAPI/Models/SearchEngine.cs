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
        private delegate bool Filter<T>(T item);
        private IServiceRepository repository;

        public SearchEngine(IServiceRepository repo)
        {
            repository = repo;
        }

        public IEnumerable<Business> FilterBusinesses()
        {
            Filter<User> ownersFilter = delegate(User user)
            {
                return (string.IsNullOrEmpty(OwnersSurname) || user.Surname.ToLower().Contains(OwnersSurname.ToLower()));
            };
            Filter<Business> businessFilter = delegate(Business business)
            {
                return string.IsNullOrEmpty(BusinessInfo) || (SearchIn == 0 && ChosenHeader(business))
                        && (SearchIn == 1 && ChosenDescription(business)) ||
                        (SearchIn == 2 && ChosenHeader(business) && ChosenDescription(business));
            };
            Filter<IEnumerable<TimeSheet>> timeFilter = delegate(IEnumerable<TimeSheet> workdays)
            {
                return workdays.All(w => (w.From.TimeOfDay <= OpenFrom.TimeOfDay
                                          && w.To.TimeOfDay <= OpenTo.TimeOfDay) || !WeekdaySelected[w.Weekday - 1]);
            };
            IEnumerable<Business> result = repository.Business.Where(b => ownersFilter(b.User) && businessFilter(b) && timeFilter(b.Workdays)).
                Include(b => b.User).Include(b => b.Products).Include(b => b.Workdays);
            foreach (var b in result)
            {
                b.EliminateDepth();
            }
            return result;
        }


        private bool ChosenHeader(Business business)
        {
            return business.Header.ToLower().Contains(BusinessInfo.ToLower());
        }

        private bool ChosenDescription(Business business)
        {
            return business.Description.ToLower().Contains(BusinessInfo.ToLower());
        }

    }
}
