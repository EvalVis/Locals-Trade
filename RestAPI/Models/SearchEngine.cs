using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using RestAPI.Models.Repositories;

namespace RestAPI.Models
{
    /// <summary>
    /// In case some of the properties are not included in the request query, filter always return true for that part.
    /// </summary>
    public class SearchEngine
    {
        public string OwnersSurname { get; set; }
        public string BusinessInfo { get; set; }
        public int? SearchIn { get; set; }
        public bool[] WeekdaySelected { get; set; } = new bool[7];
        public DateTime? OpenFrom { get; set; }
        public DateTime? OpenTo { get; set; }
        public double? Latitude { get; set; }
        public double? Longitude { get; set; }
        public double? DistanceKM { get; set; }
        public string productName { get; set; }


        public IEnumerable<Business> FilterBusinesses(int page, int pageSize, IServiceRepository repository, out int totalItems)
        {
            if (string.IsNullOrWhiteSpace(OwnersSurname))
            {
                OwnersSurname = null;
            }
            else {
                OwnersSurname = OwnersSurname.ToLower();
            }
            if(string.IsNullOrWhiteSpace(BusinessInfo))
            {
                BusinessInfo = null;
            }
            else
            {
                BusinessInfo = BusinessInfo.ToLower();
            }
            string selectedW = null;
            for(int i = 0; i < 7; i++)
            {
                if(WeekdaySelected[i])
                {
                    selectedW += ("" + (i + 1));
                }
            }
            var filtered = repository.Business.Include(b => b.User).
                Include(b => b.Workdays).Include(b => b.Products).OrderByDescending(b => b.BusinessID).
                Where(b => OwnersSurname == null || b.User.Surname.ToLower() == OwnersSurname).
                Where(b => productName == null || b.Products.Any(p => p.Name == productName)).
                Where(b => selectedW == null || b.Workdays.Any(w => selectedW.Contains(w.Weekday.ToString()))).
                Where(b => (OpenFrom == null || OpenTo == null) || b.Workdays.All(w => OpenFrom.Value.TimeOfDay <= w.From.TimeOfDay && OpenTo.Value.TimeOfDay >= w.To.TimeOfDay)).ToList();
            IEnumerable<Business> extraFilter = filtered.Where(b => BusinessConditionsMet(b) && GoodRange(b));
            totalItems = extraFilter.Count();
            return extraFilter.Skip((page - 1) * pageSize).Take(pageSize);
        }

        private bool GoodRange(Business business)
        {
            if (Latitude == null || Longitude == null || DistanceKM == null) return true;
            double.TryParse(business.Longitude, out double bLo);
            double.TryParse(business.Latitude, out double bLa);
            double bLon = ToRadians(bLo);
            double bLat = ToRadians(bLa);
            double requesterLon = ToRadians(Latitude.Value);
            double requesterLat = ToRadians(Longitude.Value);
            double difLon = bLon - requesterLon;
            double difLat = bLat - requesterLat;
            double calc = Math.Pow(Math.Sin(difLat / 2), 2) + Math.Cos(requesterLat) * Math.Cos(bLat) * Math.Pow(Math.Sin(difLon / 2), 2);
            double calculated = 2 * Math.Asin(Math.Sqrt(calc));
            double r = 6371;
            return (calculated * r) <= DistanceKM.Value;
        }

        private bool BusinessConditionsMet(Business business)
        {
            if (!string.IsNullOrWhiteSpace(BusinessInfo) && SearchIn != null)
            {
                if (SearchIn.Value == 0)
                {
                    if (!ChosenHeader(business)) return false;
                }
                else if (SearchIn.Value == 1)
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
            return business.Description.ToLower().Contains(BusinessInfo.ToLower());
        }

        private double ToRadians(double deg)
        {
            return (deg * Math.PI) / 180;
        }

    }
}
