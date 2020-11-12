namespace Support_Your_Locals.Models
{
    public class Product
    {
        public long ProductID { get; set; }
        public string Name { get; set; }
        public decimal PricePerUnit { get; set; }
        public string Unit { get; set; }
        public string Comment { get; set; }
        public string Picture { get; set; }
        public long BusinessID { get; set; }
        public Business Business { get; set; }
    }
}
