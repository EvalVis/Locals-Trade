using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using RestAPI.Models.Repositories;

namespace RestAPI.Models.Search
{
    /// <summary>
    /// In case some of the properties are not included in the request query, filter always return true for that part.
    /// </summary>
    public class SearchEngine
    {
        public string OwnersSurname { get; set; }
        public string BusinessInfo { get; set; }
        public int? SearchIn { get; set; }
        /// <summary>
        /// Only objects that satisfy all array's true bools go throught.
        /// </summary>
        public bool[] WeekdaySelected { get; set; } = new bool[7];
        public DateTime? OpenFrom { get; set; }
        public DateTime? OpenTo { get; set; }
        /// <summary>
        /// Requester's latitude.
        /// </summary>
        public double? Latitude { get; set; }
        /// <summary>
        /// Requester's longitude.
        /// </summary>
        public double? Longitude { get; set; }
        public double? DistanceKM { get; set; }
        public string ProductName { get; set; }
        public decimal? PriceFrom { get; set; }
        public decimal? PriceTo { get; set; }


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
            var filtered = repository.Business.Include(b => b.User).
                Include(b => b.Workdays).Include(b => b.Products).OrderByDescending(b => b.BusinessID).
                Where(b => OwnersSurname == null || b.User.Surname.ToLower() == OwnersSurname).
                ToList();
            IEnumerable<Business> extraFilter = filtered.
                Where(b => BusinessConditionsMet(b) && GoodRange(b)).Where(b => GoodWeekdays(b.Workdays)).Where(b => GoodProducts(b.Products));
            totalItems = extraFilter.Count();
            return extraFilter.Skip((page - 1) * pageSize).Take(pageSize);
        }

        private bool GoodProducts(IEnumerable<Product> products)
        {
            return products.Where(p => ProductName == null || p.Name == ProductName).Any(p => PriceFrom == null || PriceTo == null || p.PricePerUnit >= PriceFrom.Value && p.PricePerUnit <= PriceTo.Value);
        }

        private bool GoodWeekdays(IEnumerable<TimeSheet> workdays)
        {
            for(int i = 0; i < 7; i++)
            {
                if (WeekdaySelected[i])
                {
                    TimeSheet day = workdays.FirstOrDefault(w => w.Weekday == i + 1);
                    if(day == null)
                    {
                        return false;
                    }
                    else
                    {
                        if (OpenFrom == null || OpenTo == null) continue;
                        if (day.From.TimeOfDay > OpenFrom.Value.TimeOfDay || day.To.TimeOfDay < OpenTo.Value.TimeOfDay) return false;
                    }
                }
            }
            return true;
        }

        private bool GoodRange(Business business)
        {
            if (Latitude == null || Longitude == null || DistanceKM == null) return true;
            double.TryParse(business.Longitude, NumberStyles.Any, CultureInfo.InvariantCulture, out double bLo);
            double.TryParse(business.Latitude, NumberStyles.Any, CultureInfo.InvariantCulture, out double bLa);
            double bLon = ToRadians(bLo);
            double bLat = ToRadians(bLa);
            double requesterLat = ToRadians(Latitude.Value);
            double requesterLon = ToRadians(Longitude.Value);
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
