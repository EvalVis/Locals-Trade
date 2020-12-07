using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Threading.Tasks;
using MSupportYourLocals.Models;
using MSupportYourLocals.Services;
using Xamarin.Forms;

namespace MSupportYourLocals.ViewModels
{
    public class BusinessesViewModel : INotifyPropertyChanged
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

        public BusinessesViewModel()
        {
            Task.Run(async () => await GetBusinesses()).Wait();
        }

        public async Task GetBusinesses()
        {
            businesses = await businessService.GetBusinesses();
        }

    }
}
