using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace RestAPI.Models.Search
{
    public class OptimalCourierRoute
    {
        public double StartLatitude { get; set; }
        public double StartLongitude { get; set; }
        public double DestinationLatitude { get; set; }
        public double DestinationLongitude { get; set; }
        public int OrdersCount { get; set; }

        private async Task<double> RouteCost(List<Order> orders)
        {
            double distance = 0;
            double lastLon = StartLongitude;
            double lastLat = StartLatitude;
            foreach (var o in orders)
            {
                Geocode coordinates = await AddressToCoordinates(o.Address);
                if (coordinates != null)
                {
                    double cLat = coordinates.Latitude;
                    double cLon = coordinates.Longitude;
                    distance += CalculateDistance(lastLat, lastLon, cLat, cLon);
                    lastLat = cLat;
                    lastLon = cLon;
                }
            }
            distance += CalculateDistance(lastLat, lastLon, DestinationLatitude, DestinationLongitude);
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
                List<Geocode> data = JsonConvert.DeserializeObject<List<Geocode>>(result);
                if(data.Count > 0)
                {
                    return data[0];
                }
            }
            return null;
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

        private IEnumerable<IEnumerable<T>> GetPermutations<T>(IEnumerable<T> list, int length)
        {
            if (length == 1) return list.Select(t => new T[] { t });
            return GetPermutations(list, length - 1)
                .SelectMany(t => list.Where(e => !t.Contains(e)),
                    (t1, t2) => t1.Concat(new T[] { t2 }));
        }

    }

    class CourierTask
    {
        public Geocode Geocode { get; set; }
        public bool isBusiness { get; set; }
        public object Object { get; set; }
    }

    class Geocode
    {
        public double Latitude { get; set; }
        public double Longitude { get; set; }
    }

}
