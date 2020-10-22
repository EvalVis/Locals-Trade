using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Support_Your_Locals.Models.ViewModels.BusinessBoard
{
    public class PagingInfo
    {
        public int TotalItems { get; set; }
        public int ItemsPerPage { get; set; }
        public int CurrentPage { get; set; }

        public int TotalPages => (int)Math.Ceiling((decimal)TotalItems / ItemsPerPage);
    }
}
