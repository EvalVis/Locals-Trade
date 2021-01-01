namespace RestAPI.Models
{
    public class CourierTask
    {
        public Geocode Geocode { get; set; }
        public bool isBusiness { get; set; }
        public Business parentBusiness { get; set; }
        public object Object { get; set; }
    }
}
