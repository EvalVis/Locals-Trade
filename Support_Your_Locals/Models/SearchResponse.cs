using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Microsoft.AspNetCore.Mvc;

namespace Support_Your_Locals.Models
{
    public class SearchResponse
    {
        [FromQuery(Name="os")]
        public string OwnersSurname { get; set; }
        [FromQuery(Name = "bi")]
        public string BusinessInfo { get; set; }
        [FromQuery(Name = "si")]
        public int SearchIn { get; set; }
        [FromQuery(Name = "w")]
        public string WeekSelected { get; set; } = "1111111";
        public bool[] WeekdaySelected { get; set; } = new bool[7];
        [DataType(DataType.Time)]
        [FromQuery(Name = "f")]
        public DateTime OpenFrom { get; set; } = new DateTime(1999, 12, 06, 23, 59, 00);
        [DataType(DataType.Time)]
        [FromQuery(Name= "t")]
        public DateTime OpenTo { get; set; } = new DateTime(1999, 12, 06, 18, 00, 00);
        private delegate bool Filter<T>(T item);
      
        public string ToQuery()
        {
            return $"/?os={OwnersSurname}&bi={BusinessInfo}&si={SearchIn}&w={WeekSelected}&f={OpenFrom.TimeOfDay}&t={OpenTo.TimeOfDay}";
        }


        public void SetWeekdaySelected()
        {
            System.Diagnostics.Debug.WriteLine(OpenFrom);
            if (WeekSelected.Length != 7) return;
            for (int i = 0; i < 7; i++)
            {
                if (WeekSelected[i] == '1') WeekdaySelected[i] = true;
                else WeekdaySelected[i] = false;
            }
        }

        public IEnumerable<Business> FilterBusinesses(IEnumerable<Business> businesses)
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
