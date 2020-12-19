using System.Collections.ObjectModel;

namespace MSupportYourLocals.Models
{
    public class Business
    {
        public long BusinessId { get; set; }
        public User User { get; set; }
        public string Description { get; set; }
        public string Longitude { get; set; }
        public string Latitude { get; set; }
        public string PhoneNumber { get; set; }
        public string Header { get; set; }
        public ObservableCollection<Product> Products { get; set; } = new ObservableCollection<Product>();
        public ObservableCollection<Workday> Workdays { get; set; } = new ObservableCollection<Workday>();
        public string ConcatenatedProducts { get; private set; }

        public void ConcatProducts()
        {
            foreach (var p in Products)
            {
                ConcatenatedProducts += (p.Name + "\n");
            }
        }
    }
}
