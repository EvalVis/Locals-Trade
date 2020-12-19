using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Threading.Tasks;
using MSupportYourLocals.Models;
using MSupportYourLocals.Services;
using Xamarin.Forms;

namespace MSupportYourLocals.ViewModels
{
    public class UserBusinessViewModel : INotifyPropertyChanged
    {

        public event PropertyChangedEventHandler PropertyChanged;

        private IBusinessService businessService = DependencyService.Get<IBusinessService>();

        private ObservableCollection<Business> businesses;

        public ObservableCollection<Business> Businesses
        {
            get { return businesses; }
            set
            {
                businesses = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Business"));
            }
        }

        public UserBusinessViewModel()
        {
            Task.Run(async () => await GetBusinesses()).Wait();
        }

        public async Task GetBusinesses()
        {
            businesses = await businessService.GetUserBusinesses() ?? new ObservableCollection<Business>();
        }

        public void ConcatAllProducts()
        {
            foreach (var b in Businesses)
            {
                b?.ConcatProducts();
            }
        }

    }
}
