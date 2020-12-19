using System.Collections.Generic;

namespace RestAPI.Models
{
    public class PageBusiness
    {
        public int TotalPages { get; set; }
        public IEnumerable<Business> Businesses { get; set; }
    }
}
