using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Support_Your_Locals.Models
{
    public class SearchResponse
    {
        public string OwnersSurname { get; set; }
        public string BusinessInfo { get; set; }
        public int SearchIn { get; set; }
        public bool[] WeekdaySelected { get; set; } = new bool[7];
        [DataType(DataType.Time)]
        public DateTime OpenFrom { get; set; } = new DateTime(1999, 12, 06, 0, 00, 00);
        [DataType(DataType.Time)]
        public DateTime OpenTo { get; set; } = new DateTime(1999, 12, 06, 23, 59, 00);
        private delegate bool Filter<T>(T item);

        public string ToQuery()
        {
            var query = $"?ownersSurname={OwnersSurname}&businessInfo={BusinessInfo}&searchIn={SearchIn}&";
            for(int i = 0; i < WeekdaySelected?.Length; i++)
            {
                query += $"WeekdaySelected[{i}]={WeekdaySelected?[i]}&";
            }
            query += $"OpenFrom={OpenFrom.TimeOfDay}&OpenTo={OpenTo.TimeOfDay}";
            return query;
        }

        public IEnumerable<Business> FilterBusinesses(IEnumerable<Business> businesses)
        {
            if (WeekdaySelected?.Length != 7) return Enumerable.Empty<Business>();
            Filter<User> ownersFilter = delegate (User user)
            {
                return (string.IsNullOrEmpty(OwnersSurname) || user.Surname.ToLower().Contains(OwnersSurname.ToLower()));
            };
            Filter<Business> businessFilter = delegate (Business business)
            {
                return string.IsNullOrEmpty(BusinessInfo) || (SearchIn == 0 && ChosenHeader(business))
                        || (SearchIn == 1 && ChosenDescription(business)) ||
                        (SearchIn == 2 && ChosenHeader(business) && ChosenDescription(business));
            };
            Filter<IEnumerable<TimeSheet>> timeFilter = delegate (IEnumerable<TimeSheet> workdays)
            {
                return (workdays.All(w => w.From.TimeOfDay >= OpenFrom.TimeOfDay
                                          && w.To.TimeOfDay <= OpenTo.TimeOfDay) && workdays.Any(w => WeekdaySelected[w.Weekday - 1])) || !workdays.Any();
            };
            return businesses.Where(b => ownersFilter(b.User) && businessFilter(b) && timeFilter(b.Workdays));
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
