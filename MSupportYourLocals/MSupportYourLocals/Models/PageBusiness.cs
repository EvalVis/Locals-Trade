using System.Collections.ObjectModel;

namespace MSupportYourLocals.Models
{
    public class PageBusiness
    {
        public int TotalPages { get; set; }
        public ObservableCollection<Business> Businesses { get; set; }
    }
}
