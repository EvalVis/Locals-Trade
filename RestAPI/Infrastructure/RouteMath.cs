using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using RestAPI.Models;

namespace RestAPI.Infrastructure
{
    public class RouteMath
    {

        public static double ToRadians(double deg)
        {
            return (deg * Math.PI) / 180;
        }

        public static double CalculateDistance(double lat1, double lon1, double lat2, double lon2)
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

        public static IEnumerable<IEnumerable<T>> GetPermutations<T>(IEnumerable<T> list, int length)
        {
            if (length == 0) return list.Select(t => new T[] { });
            if (length == 1) return list.Select(t => new T[] { t });
            return GetPermutations(list, length - 1)
                .SelectMany(t => list.Where(e => !t.Contains(e)),
                    (t1, t2) => t1.Concat(new T[] { t2 }));
        }

    }
}
