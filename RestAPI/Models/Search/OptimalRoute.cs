using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using RestAPI.Models.Repositories;

namespace RestAPI.Models.Search
{
    public class OptimalRoute
    {
        public double StartLatitude { get; set; }
        public double StartLongitude { get; set; }
        public double DestinationLatitude { get; set; }
        public double DestinationLongitude { get; set; }
        public List<string> ProductNames { get; set; } = new List<string>();

        public List<Business> FindBusinesses(IServiceRepository repository)
        {
            if(ProductNames == null || ProductNames.Count == 0)
            {
                return new List<Business>();
            }
            List<List<Business>> businessByProducts = new List<List<Business>>();
            foreach (var pName in ProductNames)
            {
                businessByProducts.Add(repository.Business.Include(b => b.Products).Where(b => b.Products.Any(p => p.Name.Contains(pName))).ToList());
            }
            List<Business> tempBest = new List<Business>();
            for(int i = 0; i < businessByProducts.Count; i++)
            {
                tempBest.Add(businessByProducts[i][0]);
            }
            List<Business> best = Combination(tempBest, new List<Business>(), businessByProducts, 0);
            return best;
        }

        private List<Business> Combination(List<Business> best, List<Business> collected, List<List<Business>> grouped, int position)
        {
            if(position >= grouped.Count)
            {
                return collected;
            }
            List<Business> current = grouped[position];
            foreach(var c in current)
            {
                Business business = c;
                collected.Add(business);
                List<Business> collection = Combination(best, collected, grouped, position + 1);
                if(RouteCost(collection) < RouteCost(best))
                {
                    best = collection;
                }
            }
            return best;
        }

        private double RouteCost(List<Business> business)
        {
            double distance = 0;
            double lastLon = StartLongitude;
            double lastLat = StartLatitude;
            foreach(var b in business)
            {
                double.TryParse(b.Latitude, NumberStyles.Any, CultureInfo.InvariantCulture, out double cLat);
                double.TryParse(b.Longitude, NumberStyles.Any, CultureInfo.InvariantCulture, out double cLon);
                distance += CalculateDistance(lastLat, lastLon, cLat, cLon);
            }
            distance += CalculateDistance(lastLat, lastLon, DestinationLatitude, DestinationLongitude);
            return distance;
        }

        private double CalculateDistance(double lat1, double lon1, double lat2, double lon2)
        {
            double radLat1 = ToRadians(lon1);
            double radLon1 = ToRadians(lat1);
            double radLat2 = ToRadians(lat2);
            double radLon2 = ToRadians(lon2);
            double difLon = radLon1 - radLon2;
            double difLat = radLat1 - radLat2;
            double calc = Math.Pow(Math.Sin(difLat / 2), 2) + Math.Cos(radLat2) * Math.Cos(radLat1) * Math.Pow(Math.Sin(difLon / 2), 2);
            double calculated = 2 * Math.Asin(Math.Sqrt(calc));
            double r = 6371;
            return calculated * r;
        }

        private double ToRadians(double deg)
        {
            return (deg * Math.PI) / 180;
        }

    }
}
