using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Support_Your_Locals.Models.ViewModels
{
    public class ProductRegisterModel
    {
        [Required(ErrorMessage = "Please enter name for your product")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Please enter price for your product")]
        public decimal PricePerUnit { get; set; }
        public string Unit { get; set; }
        public string Comment { get; set; }
        public string Picture { get; set; }
        public long BusinessID { get; set; }
    }
}
