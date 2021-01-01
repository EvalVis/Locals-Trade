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

        private List<List<Business>> businessByProducts = new List<List<Business>>();
        private Route bestRoute = new Route { business = new List<Business>() };

        public List<Business> FindBusinesses(IServiceRepository repository)
        {
            if (ProductNames == null || ProductNames.Count == 0)
            {
                return new List<Business>();
            }
            foreach (var pName in ProductNames)
            {
                businessByProducts.Add(repository.Business.Include(b => b.Products).Where(b => b.Products.Any(p => p.Name.Contains(pName))).ToList());
            }
            foreach(var pack in businessByProducts)
            {
                if(pack.Count < 1)
                {
                    return new List<Business>();
                }
                bestRoute.business.Add(pack[0]);
            }
            bestRoute.distance = RouteCost(bestRoute.business);
            List<Business> best = GetBest(new List<Business>(), 0);
            return best;
        }

        private List<Business> GetBest(List<Business> current, int position)
        {
            if(position >= businessByProducts.Count)
            {
                List<List<Business>> permutations = (List<List<Business>>)GetPermutations(current, current.Count);
                return permutations.FirstOrDefault(list => RouteCost(list) == permutations.Min(x => RouteCost(x)));
            }
            foreach (var b in businessByProducts.ElementAt(position))
            {
                List<Business> newCurrent = new List<Business>();
                newCurrent.AddRange(current);
                newCurrent.Add(b);
                List<Business> bestPermutation = GetBest(newCurrent, position + 1);
                Route localBest = new Route { business = bestPermutation, distance = RouteCost(bestPermutation) };
                if(localBest.distance < bestRoute.distance)
                {
                    bestRoute = localBest;
                }
            }
            return bestRoute.business;
        }

        private IEnumerable<IEnumerable<T>> GetPermutations<T>(IEnumerable<T> list, int length)
        {
            if (length == 1) return list.Select(t => new T[] { t });
            return GetPermutations(list, length - 1)
                .SelectMany(t => list.Where(e => !t.Contains(e)),
                    (t1, t2) => t1.Concat(new T[] { t2 }));
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
                lastLat = cLat;
                lastLon = cLon;
            }
            distance += CalculateDistance(lastLat, lastLon, DestinationLatitude, DestinationLongitude);
            return distance;
        }

        private double CalculateDistance(double lat1, double lon1, double lat2, double lon2)
        {
            double radLat1 = ToRadians(lat1);
            double radLon1 = ToRadians(lon1);
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
