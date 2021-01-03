using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using RestAPI.Infrastructure;
using RestAPI.Models.Repositories;

namespace RestAPI.Models.Search
{

    /// <summary>
    /// Dismissed this search because http://api.positionstack.com/v1/forward api shows some inacurate results, for example tujų g. -> Tujunga California. Please reopen, if you find any better api.
    /// </summary>
    public class OptimalCourierRoute
    {
        public double StartLatitude { get; set; }
        public double StartLongitude { get; set; }
        public double DestinationLatitude { get; set; }
        public double DestinationLongitude { get; set; }
        public int OrdersCount { get; set; }

        public async Task<List<CourierTask>> GetRoute(IServiceRepository repository)
        {
            List<Order> orders = repository.Orders.Include(o => o.Product).Where(o => o.Address != null && !o.Resolved).ToList();
            List<Business> business = new List<Business>();
            List<CourierTask> tasks = new List<CourierTask>();
            foreach (var order in orders)
            {
                CourierTask orderTask = new CourierTask { isBusiness = false, Object = order };
                orderTask.Geocode = await AddressToCoordinates(order.Address);
                if (orderTask.Geocode == null) continue;
                    Business b = repository.Business.FirstOrDefault(b => b.Products.Any(p => p.Orders.Any(o => o.Id == order.Id)));
                    if (b != null && b.Latitude != null && b.Longitude != null && !business.Contains(b))
                    {
                        b.EliminateDepth();
                        b.PictureData = null;
                        business.Add(b);
                        orderTask.parentBusiness = b;
                        double.TryParse(b.Latitude, NumberStyles.Any, CultureInfo.InvariantCulture, out double bLa);
                        double.TryParse(b.Longitude, NumberStyles.Any, CultureInfo.InvariantCulture, out double bLo);
                        CourierTask businessTask = new CourierTask { Geocode = new Geocode { Latitude = bLa, Longitude = bLo }, isBusiness = true, Object = b };
                        tasks.Add(businessTask);
                        tasks.Add(orderTask);
                }
            }
            List<CourierTask> best = GetBest(tasks);
            foreach(var b in best)
            {
                b.parentBusiness = null;
            }
            return best;
        }

        private List<CourierTask> GetBest(IEnumerable<CourierTask> tasks)
        {
            IEnumerable<IEnumerable<CourierTask>> validPermutations = GetPermutations(tasks, tasks.Count());
            if (validPermutations.Count() < 1)
            {
                return new List<CourierTask>();
            }
            IEnumerable<CourierTask> best = validPermutations.ElementAt(0);
            double bestRouteCost = RouteCost(best.ToList());
            foreach (var perm in validPermutations)
            {
                double localBestRouteCost = RouteCost(perm.ToList());
                if (localBestRouteCost < bestRouteCost)
                {
                    best = perm;
                    bestRouteCost = localBestRouteCost;
                }
            }
            return best.ToList();
        }

        private double RouteCost(List<CourierTask> tasks)
        {
            double distance = 0;
            double lastLon = StartLongitude;
            double lastLat = StartLatitude;
            foreach (var t in tasks)
            {
                if (t.Geocode != null)
                {
                    double cLat = t.Geocode.Latitude;
                    double cLon = t.Geocode.Longitude;
                    distance += RouteMath.CalculateDistance(lastLat, lastLon, cLat, cLon);
                    lastLat = cLat;
                    lastLon = cLon;
                }
            }
            distance += RouteMath.CalculateDistance(lastLat, lastLon, DestinationLatitude, DestinationLongitude);
            return distance;
        }

        private async Task<Geocode> AddressToCoordinates(string address)
        {
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri("http://api.positionstack.com/v1/forward");
            HttpResponseMessage response = await client.GetAsync($"?access_key=92989511fc17d3b311e63a098337a457&query={address}");
            if (response.StatusCode == HttpStatusCode.OK)
            {
                var result = await response.Content.ReadAsStringAsync();
                System.Diagnostics.Debug.WriteLine(result);
                Data data = JsonConvert.DeserializeObject<Data>(result);
                if (data.data.Count > 0)
                {
                    return data.data[0];
                }
            }
            return null;
        }

        private IEnumerable<IEnumerable<CourierTask>> GetPermutations(IEnumerable<CourierTask> list, int length)
        {
            if(length == 0) return list.Select(t => new CourierTask[] {});
            if (length == 1) return list.Select(t => new CourierTask[] { t });
            return GetPermutations(list, length - 1)
                .SelectMany(t => list.Where(e => {
                    if (t.Contains(e))
                    {
                        return false;
                    }
                    if (!e.isBusiness)
                    {
                        foreach (var element in t)
                        {
                            if (element.Object.Equals(e.parentBusiness))
                            {
                                return true;
                            }
                        }
                        return false;
                    }
                    return true;
                }),
                    (t1, t2) => t1.Concat(new CourierTask[] { t2 }));
        }

    }

    class Data
    {
        public List<Geocode> data { get; set; } = new List<Geocode>();
    }

}
