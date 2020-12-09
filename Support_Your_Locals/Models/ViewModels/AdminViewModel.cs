using System.Collections.Generic;

namespace Support_Your_Locals.Models.ViewModels
{
    public class AdminViewModel
    {
        public int TotalBusiness { get; set; }
        public int TotalProducts { get; set; }
        public int TotalUsers { get; set; }
        public IEnumerable<Business> Businesses { get; set; }
        public IEnumerable<User> Users { get; set; }
    }
}
