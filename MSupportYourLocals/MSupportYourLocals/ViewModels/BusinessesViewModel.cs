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

        private ObservableCollection<Business> business;

        public ObservableCollection<Business> Business
        {
            get { return business; }
            set
            {
                business = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Business"));
            }
        }

        public BusinessesViewModel()
        {
            Task.Run(async () => await GetBusinesses()).Wait();
        }

        public async Task GetBusinesses()
        {
            business = await businessService.GetBusinesses();
        }

    }
}
