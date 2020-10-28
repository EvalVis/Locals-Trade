using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Support_Your_Locals.Models.ViewModels.BusinessBoard
{
    public class BusinessListViewModel
    {
        public IEnumerable<Business> Businesses { get; set; }
        public PagingInfo PagingInfo { get; set; }
        public string CurrentCategory { get; set; }
    }
}
