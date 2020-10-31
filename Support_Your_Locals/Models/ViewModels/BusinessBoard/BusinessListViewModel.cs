using System.Collections.Generic;

namespace Support_Your_Locals.Models.ViewModels.BusinessBoard
{
    public class BusinessListViewModel
    {
        public IEnumerable<UserBusinessTimeSheets> UserBusinessTimeSheets { get; set; }
        public PagingInfo PagingInfo { get; set; }
        public string CurrentProduct { get; set; }
        public SearchResponse SearchResponse { get; set; }
    }
}
