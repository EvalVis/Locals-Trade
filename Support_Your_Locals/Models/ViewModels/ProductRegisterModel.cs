using Microsoft.AspNetCore.Http;

namespace Support_Your_Locals.Models.ViewModels
{
    public class ProductRegisterModel
    {
        public string Name { get; set; }
        public decimal PricePerUnit { get; set; }
        public string Unit { get; set; }
        public string Comment { get; set; }
        public IFormFile Picture { get; set; }
    }
}
