using System.Collections.Generic;

namespace MSupportYourLocals.Models
{
    public class BusinessItem
    {
        public long Id { get; set; }
        public string Description { get; set; }
        public string Longitude { get; set; }
        public string Latitude { get; set; }
        public string PhoneNumber { get; set; }
        public string Header { get; set; }
        public List<WorkdayItem> Workdays { get; set; } = new List<WorkdayItem>();
        public List<ProductItem> Products { get; set; } = new List<ProductItem>();
    }
}
