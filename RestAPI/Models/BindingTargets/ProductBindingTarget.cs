using System.ComponentModel.DataAnnotations;

namespace RestAPI.Models.BindingTargets
{
    public class ProductBindingTarget
    {
        [Required(ErrorMessage = "Please enter the name of the product")]
        [StringLength(100, ErrorMessage = "Name too large. Please add additional text in the product comment nearby")]
        public string Name { get; set; }
        [Range(0.01, long.MaxValue, ErrorMessage = "The price must be 0.01 or more")]
        public decimal PricePerUnit { get; set; }
        [Required(ErrorMessage = "Please enter the unit")]
        [StringLength(50, ErrorMessage = "Please enter up to 50 symbols")]
        public string Unit { get; set; }
        [StringLength(1000, ErrorMessage = "Comment too large. Please enter up to 1000 symbols")]
        public string Comment { get; set; }
        public string Picture { get; set; }
        public Product ToProduct() => new Product { Name = Name, PricePerUnit = PricePerUnit, Unit = Unit, Comment = Comment, Picture = Picture };
    }
}
