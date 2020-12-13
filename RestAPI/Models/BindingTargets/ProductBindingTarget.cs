namespace RestAPI.Models.BindingTargets
{
    public class ProductBindingTarget
    {
        public string Name { get; set; }
        public decimal PricePerUnit { get; set; }
        public string Unit { get; set; }
        public string Comment { get; set; }
        public string Picture { get; set; }
        public Product ToProduct => new Product { Name = Name, PricePerUnit = PricePerUnit, Unit = Unit, Comment = Comment, Picture = Picture };
    }
}
