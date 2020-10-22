namespace Support_Your_Locals.Models
{
    public class Business
    {
        public long BusinessID { get; set; }
        public long UserID { get; set; }
        public string Product { get; set; }
        public string Description { get; set; }
        public string Longitude { get; set; }
        public string Latitude { get; set; }
        public string PhoneNumber { get; set; }
        public string Header { get; set; }
    }
}
