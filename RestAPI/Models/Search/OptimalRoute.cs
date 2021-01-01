using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using RestAPI.Infrastructure;
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
                return GetBestPermutation(current);
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

        private List<Business> GetBestPermutation(List<Business> list)
        {
            Route best = new Route { business = list, distance = RouteCost(list) };
            IEnumerable<IEnumerable<Business>> permutations = RouteMath.GetPermutations(list, list.Count());
            foreach(var p in permutations)
            {
                List<Business> pList = p.ToList();
                Route candidate = new Route { business = pList, distance = RouteCost(pList) };
                if(candidate.distance < best.distance)
                {
                    best = candidate;
                }
            }
            return best.business;
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
                distance += RouteMath.CalculateDistance(lastLat, lastLon, cLat, cLon);
                lastLat = cLat;
                lastLon = cLon;
            }
            distance += RouteMath.CalculateDistance(lastLat, lastLon, DestinationLatitude, DestinationLongitude);
            return distance;
        }

    }

}
