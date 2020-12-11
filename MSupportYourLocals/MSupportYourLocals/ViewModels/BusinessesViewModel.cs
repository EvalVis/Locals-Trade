using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Threading.Tasks;
using MSupportYourLocals.Infrastructure;
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
        public int TotalPages;
        public int CurrentPage;

        public ObservableCollection<Business> Businesses
        {
            get { return businesses; }
            set
            {
                businesses = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Business"));
            }
        }

        public BusinessesViewModel(int currentPage)
        {
            CurrentPage = currentPage;
            Task.Run(async () => await GetBusinesses()).Wait();
        }

        public async Task GetBusinesses()
        {
            PageBusiness pageBusiness = await businessService.GetBusinesses(CurrentPage);
            businesses = pageBusiness?.Businesses;
            businesses = SortByWeekday.Sort(businesses);
            TotalPages = pageBusiness?.TotalPages ?? 1;
        }

    }
}
